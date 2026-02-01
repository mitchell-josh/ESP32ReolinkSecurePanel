using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

public class ChannelStatuses
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("online")]
    public int? Online { get; set; }
    
    [JsonPropertyName("sleep")]
    public int? Sleep { get; set; }
    
    [JsonPropertyName("uid")]
    public string? UID { get; set; }
}