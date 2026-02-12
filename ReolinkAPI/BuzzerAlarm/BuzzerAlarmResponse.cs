using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the data structure returned by a Reolink device when querying buzzer alarm settings.
/// Inherits from <see cref="BaseRequest"/> to capture common fields like command status and error codes.
/// </summary>
public class BuzzerAlarmResponse() : BaseRequest()
{
    /// <summary>
    /// Gets or sets the container for the buzzer configuration values.
    /// This property holds the actual <see cref="BuzzerAlarm"/> settings.
    /// </summary>
    /// <remarks>
    /// If the camera returns an error code, this property may be null.
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public BuzzerAlarmValue? Value { get; set; }
}