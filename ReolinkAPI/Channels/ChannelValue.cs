using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

public class ChannelValue
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("count")]
    public int? Count { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public List<ChannelStatuses>? Statuses { get; set; }
}