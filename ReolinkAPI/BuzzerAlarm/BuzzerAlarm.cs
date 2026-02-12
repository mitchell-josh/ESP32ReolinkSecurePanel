using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the hardware buzzer configuration for a Reolink device.
/// Controls whether the device emits a sound upon motion or AI detection.
/// </summary>
public class BuzzerAlarm
{
    /// <summary>
    /// Gets or sets the master toggle for the buzzer alarm.
    /// 0: Disabled, 1: Enabled.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    /// <summary>
    /// Gets or sets whether the buzzer should follow a specific time schedule.
    /// If disabled (0), the buzzer may trigger at any time the alarm is active.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("scheduleEnabled")]
    public int? ScheduleEnabled { get; set; }
    
    /// <summary>
    /// Gets or sets the weekly activation schedule for the buzzer.
    /// Only utilized if <see cref="ScheduleEnabled"/> is set to 1.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("schedule")]
    public AiSchedule? Schedule { get; set; }
}