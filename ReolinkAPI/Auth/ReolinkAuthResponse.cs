using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

public class ReolinkAuthResponse
{
    [JsonPropertyName("cmd")]
    public string? Command { get; set; }
    
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    [JsonPropertyName("value")]
    public ReolinkAuthValue? Value { get; set; }
}