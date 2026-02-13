using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents the detailed status of an individual camera channel.
/// </summary>
public record ChannelStatuses(
    // Gets or sets the channel index (0-Based)
    int? Channel,
    
    // Gets or sets the user-defined name of the camera/channel.
    string? Name,

    // Gets or sets the connection status. 1: Online, 0: Offline.
    int? Online,

    // Gets or sets the power state for battery-operated devices. 1: Sleeping, 0: Awake.
    int? Sleep,

    // Gets or sets the Unique Identifier (UID) of the specific camera.
    string? UID);