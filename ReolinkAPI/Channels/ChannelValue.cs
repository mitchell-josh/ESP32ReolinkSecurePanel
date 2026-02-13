using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents the payload containing the list of camera channels and their current count.
/// This object is nested within the 'value' property of a <see cref="ChannelResponse"/>.
/// </summary>
public record ChannelValue(
    // Gets or sets the total number of channels reported by the device.
    int? Count, 
    
    // Gets or sets the list of status details for each individual channel.
    List<ChannelStatuses>? Statuses);