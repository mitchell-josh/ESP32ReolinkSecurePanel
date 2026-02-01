using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Auth;

public class ReolinkAuthResponse() : BaseRequest()
{
    [JsonPropertyName("value")]
    public ReolinkAuthValue? Value { get; set; }
}