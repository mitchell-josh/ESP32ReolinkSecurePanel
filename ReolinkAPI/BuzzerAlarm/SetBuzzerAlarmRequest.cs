using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class SetBuzzerAlarmRequest
{
    [JsonPropertyName("cmd")]
    public string Command => "SetBuzzerAlarmV20";
    
    [JsonPropertyName("param")]
    public SetBuzzerAlarmParam? Param { get; set; }
}