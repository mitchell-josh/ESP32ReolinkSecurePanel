using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Defines the parameters for a SET request to update the device's buzzer settings.
/// Wraps the high-level buzzer configuration for the API payload.
/// </summary>
public record SetBuzzerAlarmParam(
    /* Gets or sets the buzzer configuration to be applied to the device.
     Maps the "Buzzer" key in the Reolink SET JSON payload. */
    [property: JsonPropertyName("Buzzer")] BuzzerAlarm? Buzzer);