using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmSchedule
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    [JsonPropertyName("table")]
    public BuzzerAlarmTable? Table { get; set; }
}