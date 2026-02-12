using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

/// <summary>
/// Represents the request payload to update push notification settings on the device.
/// Targets the "SetPushV20" command.
/// </summary>
public class SetPushRequest() : BaseRequest("SetPushV20")
{
    /// <summary>
    /// Gets or sets the parameters containing the push configuration to be updated.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("param")]
    public SetPushParam? Param { get; set; }
}