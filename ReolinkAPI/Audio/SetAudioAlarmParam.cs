using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Audio;

/// <summary>
/// Defines the parameters for a SET request to update audio alarm settings.
/// This acts as the container for the specific configuration being sent to the device.
/// </summary>
public record SetAudioAlarmParam(
    // Gets or sets the target camera channel index (0-based).
    int? Channel,

    // Gets or sets the audio alarm configuration details to be applied.
    [property: JsonPropertyName("Audio")] AudioAlarm? Audio);