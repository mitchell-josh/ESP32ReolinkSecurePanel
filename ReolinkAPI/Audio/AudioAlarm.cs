using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the audio alarm configuration for a Reolink device.
/// Maps to the JSON structure used in the Reolink GET/SET API calls.
/// </summary>
public class AudioAlarm
{
    /// <summary>
    /// Gets or sets the enabled state of the audio alarm.
    /// 0: Disabled, 1: Enabled.
    /// </summary>
    /// <remarks>Ignored during serialization if null.</remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    /// <summary>
    /// Gets or sets the command to stop the alarm siren manually.
    /// Usually used in 'Set' operations.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("stopAlarm")]
    public int? StopAlarm { get; set; }

    /// <summary>
    /// Gets or sets the weekly schedule for when the audio alarm is active.
    /// Uses the <see cref="AiSchedule"/> model for time-grid mapping.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("schedule")] 
    public AiSchedule? Schedule { get; set; }
}