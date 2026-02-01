using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmResponse
{
    [JsonPropertyName("cmd")]
    public string? Command { get; set; }
    
    [JsonPropertyName("code")]
    public int? Code { get; set; }
    
    [JsonPropertyName("value")]
    public BuzzerAlarmValue? Value { get; set; }
}