using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmValue
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Buzzer")]
    public BuzzerAlarm? Buzzer { get; set; }
}