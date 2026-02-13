using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

/// <summary>
/// Defines the parameters for a GET request to retrieve audio alarm settings.
/// Used to specify which camera channel the command should target.
/// </summary>
public record GetAudioAlarmParam(
    // Gets or sets the camera channel index
    int? Channel);