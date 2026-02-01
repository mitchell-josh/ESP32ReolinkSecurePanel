using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Auth;

public class ReolinkAuthRequest() : BaseRequest("Login")
{
    [JsonPropertyName("param")]
    public ReolinkAuthParam? Param { get; set; }
}