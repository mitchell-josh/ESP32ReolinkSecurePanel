using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

/// <summary>
/// Represents the configuration values for Push notifications.
/// Contained within the <see cref="PushResponse"/>.
/// </summary>
public class PushValue
{
    /// <summary>
    /// Gets or sets the master enable switch for Push notifications.
    /// 1: Enabled, 0: Disabled.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    /// <summary>
    /// Gets or sets the detailed AI-aware schedule.
    /// This defines which types of detections (Person, Vehicle, etc.) trigger pushes at specific times.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("schedule")]
    public AiSchedule? Schedule { get; set; }
    
    /// <summary>
    /// Gets or sets whether the schedule is currently active.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("scheduleEnable")]
    public int? ScheduleEnable { get; set; }
}