using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

public class SetPushParam
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Push")]
    public PushValue? Push { get; set; }
}