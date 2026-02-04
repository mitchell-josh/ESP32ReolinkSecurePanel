using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class AudioAlarmValue
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("audio")]
    public AudioAlarm? Audio { get; set; }
}