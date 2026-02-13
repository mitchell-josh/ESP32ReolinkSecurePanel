using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

/// <summary>
/// Represents the top-level response from a Reolink device when querying push notification settings.
/// </summary>
public record PushResponse(
    /* Gets or sets the container for the push configuration.
     This will be null if the device returns a non-zero error code. */
    PushValue? Value) : BaseRequest;