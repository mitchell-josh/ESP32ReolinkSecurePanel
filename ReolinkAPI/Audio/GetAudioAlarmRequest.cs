using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Represents the specific request payload for the "GetAudioAlarmV20" command.
/// Inherits from <see cref="BaseRequest"/> to include mandatory fields like command name and action.
/// </summary>
public record GetAudioAlarmRequest(
    // Gets or sets the parameters for the request, specifically the target channel.
    GetAudioAlarmParam Param) : BaseRequest("GetAudioAlarmV20", 0);