using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

/// <summary>
/// Defines the parameters for a GET request to retrieve audio alarm settings.
/// Used to specify which camera channel the command should target.
/// </summary>
public class GetAudioAlarmParam
{
    /// <summary>
    /// Gets or sets the camera channel index.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}