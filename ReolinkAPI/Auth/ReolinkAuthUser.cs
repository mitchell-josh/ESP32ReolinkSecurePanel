using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the user credentials required for device authentication.
/// </summary>
public class ReoLinkAuthUser
{
    /// <summary>
    /// Gets or sets the account username (e.g., "Admin").
    /// </summary>
    [JsonPropertyName("userName")] 
    public required string? Username { get; set; }
        
    /// <summary>
    /// Gets or sets the account password.
    /// </summary>
    /// <remarks>
    /// This value is sent as plain text in the JSON body. 
    /// </remarks>
    [JsonPropertyName("password")]
    public required string? Password { get; set; }
}