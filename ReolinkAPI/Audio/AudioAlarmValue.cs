using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class AudioAlarmValue
{
    [JsonPropertyName("audio")]
    public AudioAlarm? Audio { get; set; }
}