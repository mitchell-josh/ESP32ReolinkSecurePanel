using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

/// <summary>
/// Represents the request payload to retrieve push notification settings.
/// </summary>
public record GetPushRequest(
    // Gets or sets the parameter targeting a specific channel.
    GetPushParam Param) : BaseRequest("GetPushV20", 0);