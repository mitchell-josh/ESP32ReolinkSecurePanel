using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ReolinkAPI.Clients;
using ReolinkAPI.Services;
using SecurePanelAPI.Utils;
using SecurePanelDb;
using SecurePanelModels.Utils;
using Microsoft.EntityFrameworkCore;
using SecurePanelAPI.Handlers;
using SecurePanelAPI.Models;
using SecurePanelAPI.Services;
using SecurePanelDb.Models;
using SecurePanelModels.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add features for caching
builder.Services.AddMemoryCache();

// // Add Sqlite database
builder.Services.AddDbContext<SecurePanelDbContext>((provider, options) =>
{
    options.UseSqlite(provider.GetService<ISettings>()!.ConnectionString!)
        .LogTo(Console.WriteLine, LogLevel.Information) 
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

builder.Services.AddScoped<IPasswordHasher<AlarmUser>, PasswordHasher<AlarmUser>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, AlarmCodeHandler>();

// Add settings as singleton
builder.Services.AddSingleton<ISettings, Settings>();

// Add password hasher
builder.Services.AddScoped<IAlarmCodeService, AlarmCodeService>();

// Add alarm code authentication scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Consts.AlarmCodeScheme;
    options.DefaultChallengeScheme = Consts.AlarmCodeScheme;
}).AddCookie(Consts.AlarmCodeScheme, options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

// Add simple alarm code authorisation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AlarmCode", policy =>
    {
        policy.AuthenticationSchemes.Add(Consts.AlarmCodeScheme);
        policy.RequireAssertion(_ => true);
        policy.Requirements.Add(new AlarmCodeRequirement());
    });
});

builder.Services.AddTransient<ReolinkAuthClient>();

// Add Reolink Auth token handler
builder.Services.AddHttpClient<ReolinkAuthService>((provider, client) =>
{
    client.BaseAddress = new Uri(provider.GetService<ISettings>()!.ReolinkURL!);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{ // Skip SSL errors for locally hosted service
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

// Add reolink client
builder.Services.AddHttpClient<ReolinkClient>((provider, client) =>
{
    client.BaseAddress = new Uri(provider.GetService<ISettings>()!.ReolinkURL!);
}).AddHttpMessageHandler<ReolinkAuthClient>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{ // Skip SSL errors for locally hosted service
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

// Add business logic services
builder.Services.AddScoped<IAlarmSettingsService, AlarmSettingsService>();
builder.Services.AddScoped<IAudioAlarmService, AudioAlarmService>();
builder.Services.AddScoped<IBuzzerAlarmService, BuzzerAlarmService>();
builder.Services.AddScoped<IChannelService, ChannelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Program.cs (at the bottom, after app.Build())
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SecurePanelDbContext>();
    
    // 1. Ensure schema is applied first
    await context.Database.MigrateAsync();

    // 2. Initialize your seeder with the required services
    var reolink = services.GetRequiredService<ReolinkClient>();
    var seeder = new SecurePanelDbSeeder(context, reolink);

    // 3. Seed User (Local/Fast)
    seeder.SeedDefaultUser(new PasswordHasher<AlarmUser>());

    // 4. Seed Hardware (External/Async)
    // Wrap this in a try-catch so an offline camera doesn't crash the app
    try 
    {
        await seeder.SeedData();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not fetch cameras during seeding: {ex.Message}");
    }

    // 5. Final Flush
    await context.SaveChangesAsync();
}

app.Run();
