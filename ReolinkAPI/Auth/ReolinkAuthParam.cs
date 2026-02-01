using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

public class ReolinkAuthParam
{
    [JsonPropertyName("User")]
    public ReoLinkAuthUser? User { get; set; }
}