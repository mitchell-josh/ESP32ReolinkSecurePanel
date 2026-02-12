using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents the response received from a Reolink device when querying channel information.
/// Inherits from <see cref="BaseRequest"/> to provide access to command execution status.
/// </summary>
public class ChannelResponse() : BaseRequest()
{
    /// <summary>
    /// Gets or sets the container for the channel status data.
    /// This property is only populated if the request was successful.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("value")]
    public ChannelValue? Value { get; set; }
}