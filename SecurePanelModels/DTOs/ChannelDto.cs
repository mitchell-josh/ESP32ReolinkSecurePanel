using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

/// <summary>
/// Represents a specific camera channel/lens in the security system.
/// Bridges the gap between database records and physical hardware indices.
/// </summary>
public record ChannelDto(
    // The unique primary key in the application database.
    int ChannelId,
    
    // User friendly channel name (e.g. "Front Door")
    string ChannelName,
    
    // The physical channel index of the Reolink device (0-indexed). Used in the "channel" field of Reolink API requests.
    int ChannelKey,
    
    // Whether this channel is currently active in the security panel.
    bool ChannelEnabled);