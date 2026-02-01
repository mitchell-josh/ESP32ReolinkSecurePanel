using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmResponse() : BaseRequest()
{
    [JsonPropertyName("value")]
    public BuzzerAlarmValue? Value { get; set; }
}