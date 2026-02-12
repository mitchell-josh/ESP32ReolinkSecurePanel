using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

/// <summary>
/// Represents a schedule configuration for AI-based detection events.
/// This structure is shared across Push, Buzzer, and Record modules.
/// </summary>
public class AiSchedule
{
    /// <summary>
    /// Gets or sets whether this specific schedule is active.
    /// 1: Active (Use the table), 0: Inactive.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    /// <summary>
    /// Gets or sets the camera channel this schedule applies to.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("channel")]
    public int? Channel { get; set; }
    
    /// <summary>
    /// Gets or sets the hourly grid defining when specific detections are active.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("table")]
    public AiScheduleTable? Table { get; set; }
}