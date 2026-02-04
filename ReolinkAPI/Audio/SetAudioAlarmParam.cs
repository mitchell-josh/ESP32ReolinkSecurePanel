using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class SetAudioAlarmParam
{
    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }

    [JsonPropertyName("stopAlarm")]
    public bool? StopAlarm { get; set; }
    
    [JsonPropertyName("Audio")]
    public AudioAlarm? Audio { get; set; }
}