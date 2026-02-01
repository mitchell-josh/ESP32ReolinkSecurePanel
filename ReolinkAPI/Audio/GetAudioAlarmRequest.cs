using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class GetAudioAlarmRequest() : BaseRequest("GetAudioAlarmV20", 0)
{
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