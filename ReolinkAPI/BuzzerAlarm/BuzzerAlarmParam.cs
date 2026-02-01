using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmParam
{
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}