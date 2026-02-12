namespace SecurePanelModels.Utils;

/// <summary>
/// Provides a centralized contract for application configuration.
/// Ensures all necessary credentials and endpoints are available to the service layer.
/// </summary>
public interface ISettings
{
    /// <summary>
    /// The base IP address or Hostname of the Reolink NVR/Camera (e.g., "http://192.168.1.50").
    /// </summary>
    string? ReolinkURL { get; }
    
    /// <summary>
    /// The admin-level username for the Reolink device.
    /// </summary>
    string? Username { get; }
    
    /// <summary>
    /// The password used to generate session tokens via the Auth API.
    /// </summary>
    string? Password { get; }
    
    /// <summary>
    /// The database connection string for storing Alarm Schemes and Channel metadata.
    /// </summary>
    string? ConnectionString { get; }
}