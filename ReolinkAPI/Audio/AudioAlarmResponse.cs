using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the root response object returned by the Reolink API for audio alarm queries.
/// Inherits common request/response properties from <see cref="BaseRequest"/>.
/// </summary>
public record AudioAlarmResponse(
    // Gets the container for the audio alarm configuration data.
    AudioAlarmValue Value) : BaseRequest
{
}