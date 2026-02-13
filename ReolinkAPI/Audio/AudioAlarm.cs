using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the audio alarm configuration for a Reolink device.
/// Maps to the JSON structure used in the Reolink GET/SET API calls.
/// </summary>
public record AudioAlarm(
    /* Gets or sets the enabled state of the audio alarm.
    0: Disabled, 1: Enabled. */
    int? Enable,
    
    /* Gets or sets the command to stop the alarm siren manually.
    Usually used in 'Set' operations. */
    int? StopAlarm,
    
    /* Gets or sets the weekly schedule for when the audio alarm is active.
    Uses the <see cref="AiSchedule"/> model for time-grid mapping. */
    AiSchedule? Schedule);