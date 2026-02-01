using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

public class SetPushParam
{
    [JsonPropertyName("Push")]
    public PushValue? Push { get; set; }
}