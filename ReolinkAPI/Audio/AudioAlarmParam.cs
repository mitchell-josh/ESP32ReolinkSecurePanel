using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class AudioAlarmParam
{
    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }
    
    [JsonPropertyName("schedule")]
    public AudioAlarmSchedule? Schedule { get; set; }
}