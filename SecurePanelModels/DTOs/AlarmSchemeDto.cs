using System.Text.Json.Serialization;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.DTOs;

/// <summary>
/// Data Transfer Object representing a specific alarm configuration for a camera channel.
/// Maps a security scheme (Home/Away) to specific hardware actions and schedules.
/// </summary>
public record AlarmSchemeDto(
    int? AlarmSchemeId,
    int? AlarmChannelId,
    
    // Links to <see cref="AlarmSchemeTypes"/> (Disarmed, Partial, Full).
    int? AlarmSchemeTypeId,
    
    // Master toggle for the alarm (e.g. the Buzzer/Siren/Push)
    bool? Enabled,
    
    // Toggle for mobile push notifications
    bool? PushEnabled,
    
    // The specific AI detection rules (People, Vehicle, etc.) for this scheme.
    AlarmScheduleDto? Schedule);