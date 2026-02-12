using SecurePanelModels.Utils;

namespace SecurePanelAPI.Utils;

/// <summary>
/// A strongly-typed wrapper for application settings.
/// Facilitates the "Options Pattern" and centralizes hardware/database credentials.
/// </summary>
public class Settings(IConfiguration configuration) : ISettings
{
    // Fetches the IP or Domain of your Reolink NVR/Camera
    public string? ReolinkURL => configuration.GetValue<string>("Settings:ReolinkURL");

    // The NVR administrative credentials (used by ReolinkClient to get session tokens)
    public string? Username =>  configuration.GetValue<string>("Settings:Username");
    public string? Password => configuration.GetValue<string>("Settings:Password");
    
    // The path to your SQLite database file
    public string? ConnectionString => configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
}