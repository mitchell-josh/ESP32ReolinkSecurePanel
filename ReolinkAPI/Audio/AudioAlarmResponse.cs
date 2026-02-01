using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

public class AudioAlarmResponse() : BaseRequest()
{
    [JsonPropertyName("value")]
    public AudioAlarmValue Value => new();
}