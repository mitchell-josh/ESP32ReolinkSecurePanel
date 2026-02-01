using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

public class PushValue
{
    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }
    
    [JsonPropertyName("schedule")]
    public AiSchedule? Schedule { get; set; }
    
    [JsonPropertyName("scheduleEnable")]
    public int? ScheduleEnable { get; set; }
}