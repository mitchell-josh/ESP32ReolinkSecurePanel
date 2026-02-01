using System.Text.Json.Serialization;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarmRequest
{
    [JsonPropertyName("cmd")]
    public string Command { get; } = "GetBuzzerAlarmV20";

    [JsonPropertyName("action")]
    public int? Action { get; } = 0;

    [JsonPropertyName("param")]
    public BuzzerAlarmParam Param { get; set; } = new();

    public static BuzzerAlarmRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}