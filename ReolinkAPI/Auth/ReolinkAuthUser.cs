using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

public class ReoLinkAuthUser
{
    [JsonPropertyName("userName")] 
    public required string? Username { get; set; }
        
    [JsonPropertyName("password")]
    public required string? Password { get; set; }
}