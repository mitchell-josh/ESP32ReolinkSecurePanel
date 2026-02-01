using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmValue
{
    [JsonPropertyName("Buzzer")]
    public BuzzerAlarm? Buzzer { get; set; }
}