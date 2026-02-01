using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

public class SetPushRequest() : BaseRequest("SetPushV20", 0)
{
    [JsonPropertyName("param")]
    public SetPushParam? Param { get; set; }
}