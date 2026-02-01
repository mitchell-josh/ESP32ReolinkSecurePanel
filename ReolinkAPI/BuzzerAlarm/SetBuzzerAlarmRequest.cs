using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

public class SetBuzzerAlarmRequest() : BaseRequest("SetBuzzerAlarmV20")
{
    [JsonPropertyName("param")]
    public SetBuzzerAlarmParam? Param { get; set; }
}