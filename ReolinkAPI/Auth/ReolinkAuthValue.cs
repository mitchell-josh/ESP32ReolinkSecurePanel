using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

public class ReolinkAuthValue
{
    [JsonPropertyName("Token")]
    public ReolinkAuthToken? Token { get; set; }
}