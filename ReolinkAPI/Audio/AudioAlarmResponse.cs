using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the root response object returned by the Reolink API for audio alarm queries.
/// Inherits common request/response properties from <see cref="BaseRequest"/>.
/// </summary>
public record AudioAlarmResponse(
    /* Gets or sets the core audio alarm settings.
     Maps to the "audio" key in the Reolink JSON response. */
    [property: JsonPropertyName("Audio")] AudioAlarm? Audio)
{
}