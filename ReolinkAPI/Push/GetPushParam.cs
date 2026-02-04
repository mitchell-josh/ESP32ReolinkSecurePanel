using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

public class GetPushParam
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}