using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Defines the parameters for a SET request to update audio alarm settings.
/// This acts as the container for the specific configuration being sent to the device.
/// </summary>
public class SetAudioAlarmParam
{
    /// <summary>
    /// Gets or sets the target camera channel index (0-based).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    /// <summary>
    /// Gets or sets the audio alarm configuration details to be applied.
    /// </summary>
    /// <remarks>
    /// Note: Reolink's authentication JSON keys are often PascalCase ("User") 
    /// unlike their hardware settings which are usually camelCase.
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Audio")]
    public AudioAlarm? Audio { get; set; }
}