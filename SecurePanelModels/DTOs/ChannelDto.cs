using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

/// <summary>
/// Represents a specific camera channel/lens in the security system.
/// Bridges the gap between database records and physical hardware indices.
/// </summary>
public class ChannelDto
{
    /// <summary>
    /// The unique primary key in your application database.
    /// </summary>
    [JsonPropertyName("channelId")]
    public required int ChannelId { get; set; }
    
    /// <summary>
    /// A user-friendly name (e.g., "Front Door", "Back Yard").
    /// </summary>
    [JsonPropertyName("channelName")]
    public required string ChannelName { get; set; }
    
    /// <summary>
    /// The physical channel index on the Reolink device (0-indexed).
    /// Used in the 'channel' field of Reolink API requests.
    /// </summary>
    [JsonPropertyName("channelKey")]
    public required int ChannelKey { get; set; }
    
    /// <summary>
    /// Whether this channel is currently active in the security panel.
    /// </summary>
    [JsonPropertyName("channelEnabled")]
    public required bool ChannelEnabled { get; set; }
}