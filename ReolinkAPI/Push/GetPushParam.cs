using System.Text.Json.Serialization;

namespace ReolinkAPI.Push;

/// <summary>
/// Defines the parameters for a request to retrieve Push Notification settings.
/// Used to target a specific channel
/// </summary>
public class GetPushParam
{
    /// <summary>
    /// Gets or sets the camera channel index. 
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
}