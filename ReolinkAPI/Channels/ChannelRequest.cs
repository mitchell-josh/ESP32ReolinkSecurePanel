using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Channels;

public class ChannelRequest
{
    [JsonPropertyName("cmd")] 
    public string Command { get; set; } = "GetChannelStatus";

    [JsonPropertyName("action")]
    public int Action { get; set; } = 0;

    [JsonPropertyName("param")] 
    public EmptyParam Param { get; set; } = new();
}