using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

public class SetBuzzerAlarmParam : BaseRequest
{
    public override string? Command { get; set; } = "SetBuzzerAlarmV20";

    public override int? Code { get; set; } = 0;

    [JsonPropertyName("Buzzer")]
    public BuzzerAlarm? Buzzer { get; set; }
}