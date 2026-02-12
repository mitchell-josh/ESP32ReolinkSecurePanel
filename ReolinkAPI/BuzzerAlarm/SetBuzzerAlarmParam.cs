using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Defines the parameters for a SET request to update the device's buzzer settings.
/// Wraps the high-level buzzer configuration for the API payload.
/// </summary>
public class SetBuzzerAlarmParam
{
    /// <summary>
    /// Gets or sets the buzzer configuration to be applied to the device.
    /// Maps to the "Buzzer" key in the Reolink SET JSON payload.
    /// </summary>
    /// <remarks>
    /// Note: Reolink's authentication JSON keys are often PascalCase ("Buzzer") 
    /// unlike their hardware settings which are usually camelCase.
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Buzzer")]
    public BuzzerAlarm? Buzzer { get; set; }
}