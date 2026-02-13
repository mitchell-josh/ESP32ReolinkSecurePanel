using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the middle-layer data container for buzzer settings.
/// This object is nested inside the 'value' property of a <see cref="BuzzerAlarmResponse"/>.
/// </summary>
public record BuzzerAlarmValue(
    /* Gets or sets the specific buzzer configuration.
     Maps to the "Buzzer" key in the Reolink JSON response. */
    [property: JsonPropertyName("Buzzer")] BuzzerAlarm? Buzzer);