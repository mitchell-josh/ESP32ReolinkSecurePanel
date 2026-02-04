using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class SetAudioAlarmRequest() : BaseRequest()
{
    public override string? Command { get; set; } = "SetAudioAlarmV20";

    public override int? Code { get; set; } = 0;

    [JsonPropertyName("param")]
    public SetAudioAlarmParam? Param { get; set; }
}