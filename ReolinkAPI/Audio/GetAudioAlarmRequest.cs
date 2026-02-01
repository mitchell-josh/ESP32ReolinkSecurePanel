using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class GetAudioAlarmRequest
{
    [JsonPropertyName("cmd")]
    public string Command => "GetAudioAlarmV20";

    [JsonPropertyName("code")]
    public int Code => 0;

    [JsonPropertyName("value")]
    public AudioAlarmValue Value => new();
}