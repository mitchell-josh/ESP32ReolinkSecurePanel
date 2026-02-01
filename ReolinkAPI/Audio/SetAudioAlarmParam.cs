using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class SetAudioAlarmParam
{
    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }
    
    [JsonPropertyName("schedule")]
    public AiScheduleTable? Schedule { get; set; }
}