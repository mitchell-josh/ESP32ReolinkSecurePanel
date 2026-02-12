using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the root response object returned by the Reolink API for audio alarm queries.
/// Inherits common request/response properties from <see cref="BaseRequest"/>.
/// </summary>
public class AudioAlarmResponse : BaseRequest
{
    /// <summary>
    /// Gets the container for the audio alarm configuration data.
    /// </summary>
    /// <remarks>
    /// Note: This property currently initializes a new instance on every access.
    /// If being used for deserialization, ensure the setter logic matches your API's requirements.
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public AudioAlarmValue Value => new();
}