using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

/// <summary>
/// Defines the parameters for a SET request to update the device's Push notification settings.
/// </summary>
public class SetPushParam
{
    /// <summary>
    /// Gets or sets the push configuration to be applied.
    /// Maps to the "Push" key in the Reolink SET JSON payload.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Push")]
    public PushValue? Push { get; set; }
}