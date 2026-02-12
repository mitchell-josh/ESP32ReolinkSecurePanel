using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents the detailed status of an individual camera channel.
/// </summary>
public class ChannelStatuses
{
    /// <summary>
    /// Gets or sets the channel index (0-based).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    /// <summary>
    /// Gets or sets the user-defined name of the camera/channel.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the connection status. 
    /// 1: Online, 0: Offline.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("online")]
    public int? Online { get; set; }
    
    /// <summary>
    /// Gets or sets the power state for battery-operated devices.
    /// 1: Sleeping, 0: Awake.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("sleep")]
    public int? Sleep { get; set; }
    
    /// <summary>
    /// Gets or sets the Unique Identifier (UID) of the specific camera.
    /// Useful for P2P connections.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("uid")]
    public string? UID { get; set; }
}