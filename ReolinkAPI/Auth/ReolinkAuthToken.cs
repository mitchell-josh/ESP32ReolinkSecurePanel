using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the session token details provided by the device upon successful authentication.
/// </summary>
public class ReolinkAuthToken
{
    /// <summary>
    /// Gets or sets the duration (in seconds) that the current session remains valid.
    /// </summary>
    /// <remarks>
    /// After this time expires, a new login request is typically required.
    /// </remarks>
    [JsonPropertyName("leaseTime")]
    public double LeaseTime { get; set; }
        
    /// <summary>
    /// Gets or sets the session token string. 
    /// Despite the JSON key being "name", this field contains the authorization token.
    /// </summary>
    /// <example>
    /// This value should be appended to the URL of subsequent requests: ?token=YourTokenValue
    /// </example>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}