using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class GetAudioAlarmParam
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}