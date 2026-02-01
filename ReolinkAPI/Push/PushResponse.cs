using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

public class PushResponse() : BaseRequest()
{
    [JsonPropertyName("value")]
    public PushValue? Value { get; set; }
}