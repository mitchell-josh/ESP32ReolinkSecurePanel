using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class SetBuzzerAlarmParam
{
    [JsonPropertyName("Buzzer")]
    public BuzzerAlarmValue? Buzzer { get; set; }
}