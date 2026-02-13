using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the user credentials required for device authentication.
/// </summary>
public record ReoLinkAuthUser(
    // Version code (usually 0)
    string? Version,
    
    // Gets or sets the account username (e.g. "Admin")
    [property: JsonPropertyName("userName")] string? Username,
    
    // Gets or sets the account password. Value is sent as plain text in the JSON body.
    string? Password);