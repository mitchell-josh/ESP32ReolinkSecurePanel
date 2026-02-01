using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class AudioAlarmSchedule
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    [JsonPropertyName("table")]
    public AudioAlarmTable? Table { get; set; }
}