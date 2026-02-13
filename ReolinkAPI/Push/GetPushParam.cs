using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

/// <summary>
/// Defines the parameters for a request to retrieve Push Notification settings.
/// Used to target a specific channel
/// </summary>
public record GetPushParam(
    // Gets or sets the camera channel index.
    int? Channel);