using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the middle-layer data container for buzzer settings.
/// This object is nested inside the 'value' property of a <see cref="BuzzerAlarmResponse"/>.
/// </summary>
public class BuzzerAlarmValue
{
    /// <summary>
    /// Gets or sets the specific buzzer configuration.
    /// Maps to the "Buzzer" key in the Reolink JSON response.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Buzzer")]
    public BuzzerAlarm? Buzzer { get; set; }
}