using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmParam
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}