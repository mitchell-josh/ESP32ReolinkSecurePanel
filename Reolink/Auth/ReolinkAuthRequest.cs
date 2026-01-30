using System.Text.Json.Serialization;
using ReoAlarmModels.Reolink;

namespace Reolink.Auth;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "User")]
[JsonDerivedType(typeof(ReolinkAuthRequest), typeDiscriminator: "User")]
public class ReolinkAuthRequest(string version, string username, string password) : IReolinkAuthRequest
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = version ?? ReolinkConsts.Version0;

    [JsonPropertyName("userName")] 
    public string? Username { get; set; } = username;

    [JsonPropertyName("password")] 
    public string? Password { get; set; } = password;
}