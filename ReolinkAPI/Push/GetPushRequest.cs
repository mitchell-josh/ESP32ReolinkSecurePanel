using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

/// <summary>
/// Represents the request payload to retrieve push notification settings.
/// </summary>
public class GetPushRequest() : BaseRequest("GetPushV20", 0)
{
    /// <summary>
    /// Gets or sets the parameter targeting a specific channel.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public GetPushParam Param => new();

    /// <summary>
    /// Factory method to generate a request for a specific camera channel.
    /// </summary>
    public static GetPushRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}