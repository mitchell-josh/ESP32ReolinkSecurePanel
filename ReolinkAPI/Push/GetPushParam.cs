using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

public class GetPushParam
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}