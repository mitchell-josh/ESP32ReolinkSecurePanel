using Microsoft.AspNetCore.Identity;
using ReolinkAPI.Clients;
using ReolinkAPI.Services;
using SecurePanelAPI.Utils;
using SecurePanelDb;
using SecurePanelModels.Utils;
using Microsoft.EntityFrameworkCore;
using SecurePanelAPI.Handlers;
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

// Add Sqlite database
builder.Services.AddDbContext<SecurePanelDbContext>((provider, options) =>
{
    options.UseSqlite(provider.GetService<ISettings>()!.ConnectionString!);
    options.UseSeeding((dbContext, _) =>
    {
        var seeder = new SecurePanelDbSeeder(dbContext);
        seeder.SeedData();
        seeder.SeedDefaultUser(new PasswordHasher<AlarmUser>());
    });
});

// Add settings as singleton
builder.Services.AddSingleton<ISettings, Settings>();

// Add password hasher
builder.Services.AddScoped<IAlarmCodeService, AlarmCodeService>();

// Add simple alarm code authorisation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AlarmCode", policy => 
        policy.Requirements.Add(new AlarmCodeRequirement()));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
