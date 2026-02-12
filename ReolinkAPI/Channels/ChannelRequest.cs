using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents a request to retrieve the current status of all channels.
/// Inherits from <see cref="BaseRequest"/> to ensure consistent command and action handling.
/// </summary>
public class ChannelRequest() : BaseRequest("GetChannelStatus", 0)
{
    /// <summary>
    /// Gets or sets the parameters for the request.
    /// Since this command queries the global state, an <see cref="EmptyParam"/> is provided 
    /// to satisfy the Reolink firmware's JSON structure requirements.
    /// </summary>
    [JsonPropertyName("param")] 
    public EmptyParam Param { get; set; } = new();
}