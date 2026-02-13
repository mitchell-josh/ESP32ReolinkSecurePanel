using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Defines the parameters for a request to interact with the device's buzzer settings.
/// Primarily used to target a specific channel on multi-channel systems or NVRs.
/// </summary>
public record BuzzerAlarmParam(
    // Gets or sets the camera channel index.
    int? Channel);