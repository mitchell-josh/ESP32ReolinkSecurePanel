using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

/// <summary>
/// Represents the payload containing the list of camera channels and their current count.
/// This object is nested within the 'value' property of a <see cref="ChannelResponse"/>.
/// </summary>
public class ChannelValue
{
    /// <summary>
    /// Gets or sets the total number of channels reported by the device.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("count")]
    public int? Count { get; set; }
    
    /// <summary>
    /// Gets or sets the list of status details for each individual channel.
    /// Maps to the "status" array in the Reolink JSON response.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public List<ChannelStatuses>? Statuses { get; set; }
}