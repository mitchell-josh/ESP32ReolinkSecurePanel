using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

public class ChannelValue
{
    [JsonPropertyName("count")]
    public int? Count { get; set; }
    
    [JsonPropertyName("status")]
    public List<ChannelStatuses>? Statuses { get; set; }
}