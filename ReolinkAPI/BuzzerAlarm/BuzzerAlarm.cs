using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the hardware buzzer configuration for a Reolink device.
/// Controls whether the device emits a sound upon motion or AI detection.
/// </summary>
public record BuzzerAlarm(
    /* Gets or sets the master toggle for the buzzer alarm.
     0: Disabled, 1: Enabled. */
    int? Enable,
    
    /* Gets or sets whether the buzzer should follow a specific time schedule.
     If disabled (0), the buzzer may trigger at any time the alarm is active. */
    int? ScheduleEnabled,
    
    /* Gets or sets the weekly activation schedule for the buzzer.
     Only utilised if ScheduleEnabled is set to 1 */
    AiSchedule? Schedule);