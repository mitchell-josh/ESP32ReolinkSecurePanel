using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

public class AiSchedule
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    [JsonPropertyName("table")]
    public AiScheduleTable? Table { get; set; }
}