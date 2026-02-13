using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

/// <summary>
/// Represents a schedule configuration for AI-based detection events.
/// This structure is shared across Push, Buzzer, and Record modules.
/// </summary>
public record AiSchedule(
    /* Gets or sets whether this specific schedule is active.
     1: Active (use the table), 0: Inactive. */
    int? Enable,
    
    // Gets or sets the camera channel thsi schedule applies to.
    int? Channel,
    
    // Gets or sets the hourly grid defining when specific detections are active.
    AiScheduleTable? Table);