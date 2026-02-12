using System.Text.Json.Serialization;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.DTOs;

/// <summary>
/// Data Transfer Object representing a specific alarm configuration for a camera channel.
/// Maps a security scheme (Home/Away) to specific hardware actions and schedules.
/// </summary>
public class AlarmSchemeDto
{
    [JsonPropertyName("alarmSchemeId")]
    public int? AlarmSchemeId { get; set; }
    
    [JsonPropertyName("alarmChannelId")]
    public int? AlarmChannelId { get; set; }
    
    /// <summary>
    /// Links to <see cref="AlarmSchemeTypes"/> (Disarmed, Partial, Full).
    /// </summary>
    [JsonPropertyName("alarmSchemeTypeId")]
    public int? AlarmSchemeTypeId { get; set; }
    
    /// <summary>
    /// Master toggle for the alarm (e.g., the Buzzer/Siren/Push)
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
    
    /// <summary>
    /// Toggle for mobile push notifications.
    /// </summary>
    [JsonPropertyName("pushEnabled")]
    public bool? PushEnabled { get; set; }

    /// <summary>
    /// The specific AI detection rules (People, Vehicle, etc.) for this scheme.
    /// </summary>
    [JsonPropertyName("schedule")]
    public AlarmScheduleDto? Schedule { get; set; }
    
    /// <summary>
    /// Performs a recursive validation to ensure the entire configuration tree is valid.
    /// </summary>
    public bool Validate() =>
        this.AlarmSchemeId.HasValue
        && this.AlarmChannelId.HasValue
        && this.AlarmSchemeTypeId.HasValue
        && this.Enabled.HasValue
        && this.PushEnabled.HasValue
        && (this.Schedule?.Validate() ?? false);
}