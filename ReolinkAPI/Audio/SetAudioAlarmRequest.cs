using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class SetAudioAlarmRequest() : BaseRequest("SetAudioAlarmV20")
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public SetAudioAlarmParam? Param { get; set; }
}