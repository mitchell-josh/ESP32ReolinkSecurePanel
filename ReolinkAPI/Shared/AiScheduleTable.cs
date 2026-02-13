using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

/// <summary>
/// Represents the hourly activation grid for different AI detection types.
/// Each string typically contains 168 characters ('1' or '0').
/// </summary>
public record AiScheduleTable(
    // Gets or sets whether the table-based schedule is enabled
    int? Enable,

    // Schedule for pet detection.
    [property:JsonPropertyName("AI_DOG_CAT")] string? AiDogCat,

    // Schedule for generic "other" motion detections.
    [property: JsonPropertyName("AI_OTHER")] string? AiOther,

    // Schedule for human/people detection
    [property: JsonPropertyName("AI_PEOPLE")] string? AiPeople,

    // Schedule for vehicle detection.
    [property: JsonPropertyName("AI_VEHICLE")] string? AiVehicle);