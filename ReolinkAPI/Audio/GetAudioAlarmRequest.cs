using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the specific request payload for the "GetAudioAlarmV20" command.
/// Inherits from <see cref="BaseRequest"/> to include mandatory fields like command name and action.
/// </summary>
public class GetAudioAlarmRequest() : BaseRequest("GetAudioAlarmV20", 0)
{
    /// <summary>
    /// Gets or sets the parameters for the request, specifically the target channel.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")] 
    public GetAudioAlarmParam Param => new();
    
    /// <summary>
    /// Factory method to quickly generate a request payload for a specific camera channel.
    /// </summary>
    /// <param name="channel">The index of the camera channel (usually 0).</param>
    /// <returns>A populated <see cref="GetAudioAlarmRequest"/> object.</returns>
    public static GetAudioAlarmRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}