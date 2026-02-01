using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

public class GetPushRequest
{
    [JsonPropertyName("cmd")]
    public string Command => "GetPushV20";

    [JsonPropertyName("code")]
    public int Code => 0;

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