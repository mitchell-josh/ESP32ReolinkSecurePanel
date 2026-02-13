using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents a request to retrieve the current status of all channels.
/// Inherits from <see cref="BaseRequest"/> to ensure consistent command and action handling.
/// </summary>
public record ChannelRequest(
    /* Gets or sets the parameters for the request.
     Since this command queries the global state, an <see cref="EmptyParam"/> is provided */
    EmptyParam Param) : BaseRequest("GetChannelStatus", 0);