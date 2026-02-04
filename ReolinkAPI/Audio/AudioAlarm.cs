using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class AudioAlarm
{
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    [JsonPropertyName("stopAlarm")]
    public int? StopAlarm { get; set; }

    [JsonPropertyName("schedule")] 
    public AiSchedule? Schedule { get; set; }
}