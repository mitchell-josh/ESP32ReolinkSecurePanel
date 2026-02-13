using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents the response received from a Reolink device when querying channel information.
/// Inherits from <see cref="BaseRequest"/> to provide access to command execution status.
/// </summary>
public record ChannelResponse(
    // Gets or sets the total number of channels reported by the device.
    int? Count, 
    
    // Gets or sets the list of status details for each individual channel.
    [property: JsonPropertyName("status")] List<ChannelStatuses>? Statuses);