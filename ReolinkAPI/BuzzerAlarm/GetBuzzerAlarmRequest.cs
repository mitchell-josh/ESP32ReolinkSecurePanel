using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

public class GetBuzzerAlarmRequest() : BaseRequest("GetBuzzerAlarmV20", 0)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public BuzzerAlarmParam Param { get; set; } = new();

    public static GetBuzzerAlarmRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}