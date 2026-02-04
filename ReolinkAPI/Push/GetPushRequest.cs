using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

public class GetPushRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cmd")]
    public string Command => "GetPushV20";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("code")]
    public int Code => 0;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public GetPushParam Param => new();

    public static GetPushRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}