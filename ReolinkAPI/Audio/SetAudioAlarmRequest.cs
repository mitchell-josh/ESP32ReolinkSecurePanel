using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class SetAudioAlarmRequest
{
    [JsonPropertyName("cmd")]
    public string Command => "SetAudioAlarmV20";

    [JsonPropertyName("code")]
    public int Code => 0;
    
    [JsonPropertyName("param")]
    public AudioAlarmParam? Param { get; set; }
}