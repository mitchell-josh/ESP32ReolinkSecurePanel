using ReolinkAPI.Clients;
using ReolinkAPI.Services;
using SecurePanelAPI.Utils;
using SecurePanelModels.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

// Add settings as singleton
builder.Services.AddSingleton<ISettings, Settings>();

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
