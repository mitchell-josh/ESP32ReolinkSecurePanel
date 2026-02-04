using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class GetAudioAlarmParam
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}