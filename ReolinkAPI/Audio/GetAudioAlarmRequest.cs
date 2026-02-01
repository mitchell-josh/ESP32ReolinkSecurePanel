using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class GetAudioAlarmRequest
{
    [JsonPropertyName("cmd")]
    public string Command => "GetAudioAlarmV20";

    [JsonPropertyName("action")]
    public int? Action => 0;

    [JsonPropertyName("param")] 
    public GetAudioAlarmParam Param => new();
    
    public static GetAudioAlarmRequest CreatePayload(int channel) =>
        new()
        {
            Param =
            {
                Channel = channel
            }
        };
}