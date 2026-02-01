using System.Text.Json.Serialization;

namespace ReolinkAPI.Channels;

public class ChannelResponse
{
    [JsonPropertyName("cmd")]
    public string? Command { get; set; }
    
    [JsonPropertyName("code")]
    public int? Code { get; set; }
    
    [JsonPropertyName("value")]
    public ChannelValue? Value { get; set; }
}