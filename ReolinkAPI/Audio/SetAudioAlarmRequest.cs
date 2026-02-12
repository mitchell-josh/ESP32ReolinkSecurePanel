using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the request body for updating audio alarm settings on a Reolink device.
/// Uses the "SetAudioAlarmV20" command.
/// </summary>
public class SetAudioAlarmRequest() : BaseRequest("SetAudioAlarmV20")
{
    /// <summary>
    /// Gets or sets the parameters containing the channel and the new audio configuration.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public SetAudioAlarmParam? Param { get; set; }
}