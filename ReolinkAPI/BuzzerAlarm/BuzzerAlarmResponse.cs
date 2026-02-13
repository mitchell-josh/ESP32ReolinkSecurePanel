using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the data structure returned by a Reolink device when querying buzzer alarm settings.
/// Inherits from <see cref="BaseRequest"/> to capture common fields like command status and error codes.
/// </summary>
public record BuzzerAlarmResponse(
    /* Gets or sets the specific buzzer configuration.
     Maps to the "Buzzer" key in the Reolink JSON response. */
    [property: JsonPropertyName("Buzzer")] BuzzerAlarm? Buzzer);