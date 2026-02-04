using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

public class ChannelDto
{
    [JsonPropertyName("channelId")]
    public required int ChannelId { get; set; }
    
    [JsonPropertyName("channelName")]
    public required string ChannelName { get; set; }
    
    [JsonPropertyName("channelKey")]
    public required int ChannelKey { get; set; }
    
    [JsonPropertyName("channelEnabled")]
    public required bool ChannelEnabled { get; set; }
}