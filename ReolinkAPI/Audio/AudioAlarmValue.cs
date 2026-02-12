using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the nested data container within an <see cref="AudioAlarmResponse"/>.
/// This acts as a wrapper for the specific hardware configuration objects.
/// </summary>
public class AudioAlarmValue
{
    /// <summary>
    /// Gets or sets the core audio alarm settings.
    /// Maps to the "audio" key in the Reolink JSON response.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("audio")]
    public AudioAlarm? Audio { get; set; }
}