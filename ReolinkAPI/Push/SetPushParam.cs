using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

/// <summary>
/// Defines the parameters for a SET request to update the device's Push notification settings.
/// </summary>
public record SetPushParam(
    // Gets or sets the push configuration to be applied.
    [property: JsonPropertyName("Push")] PushValue? Push);