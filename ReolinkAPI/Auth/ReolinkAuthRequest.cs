using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

public class ReolinkAuthRequest
{
    [JsonPropertyName("cmd")] 
    public string Command { get; set; } = "Login";

    [JsonPropertyName("param")]
    public required ReolinkAuthParam? Param { get; set; }
    
    public class ReolinkAuthParam
    {
        public required ReoLinkAuthUser? User { get; set; }
    }

    public class ReoLinkAuthUser
    {
        [JsonPropertyName("userName")] 
        public required string? Username { get; set; }
        
        [JsonPropertyName("password")]
        public required string? Password { get; set; }
    }
}