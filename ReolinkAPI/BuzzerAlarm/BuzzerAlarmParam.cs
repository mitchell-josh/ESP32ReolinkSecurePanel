using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Defines the parameters for a request to interact with the device's buzzer settings.
/// Primarily used to target a specific channel on multi-channel systems or NVRs.
/// </summary>
public class BuzzerAlarmParam
{
    /// <summary>
    /// Gets or sets the camera channel index.
    /// Typically 0 for single-lens cameras.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}