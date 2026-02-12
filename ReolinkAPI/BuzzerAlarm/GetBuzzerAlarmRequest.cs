using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the request payload to retrieve buzzer alarm settings.
/// Specifically targets the "GetBuzzerAlarmV20" API command.
/// </summary>
public class GetBuzzerAlarmRequest() : BaseRequest("GetBuzzerAlarmV20", 0)
{
    /// <summary>
    /// Gets or sets the target parameters for the query.
    /// Defaults to a new instance to allow for immediate property assignment.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public BuzzerAlarmParam Param { get; set; } = new();

    /// <summary>
    /// Factory method to generate a request for a specific camera channel.
    /// </summary>
    /// <param name="channel">The index of the camera channel (0 for most standalone cameras).</param>
    /// <returns>A configured <see cref="GetBuzzerAlarmRequest"/> ready for serialisation.</returns>
    public static GetBuzzerAlarmRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}