using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class SetAudioAlarmRequest() : BaseRequest()
{
    [JsonPropertyName("param")]
    public SetAudioAlarmParam? Param { get; set; }
}