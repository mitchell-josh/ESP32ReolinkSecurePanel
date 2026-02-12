using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

/// <summary>
/// Represents the hourly activation grid for different AI detection types.
/// Each string typically contains 168 characters ('1' or '0').
/// </summary>
public class AiScheduleTable
{
    /// <summary>
    /// Gets or sets whether the table-based scheduling is enabled.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    /// <summary>
    /// Schedule for Pet (Dog/Cat) detection.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_DOG_CAT")]
    public string? AiDogCat { get; set; }
    
    /// <summary>
    /// Schedule for generic "Other" motion detections.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_OTHER")]
    public string? AiOther { get; set; }
    
    /// <summary>
    /// Schedule for Human/People detection.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_PEOPLE")]
    public string? AiPeople { get; set; }
    
    /// <summary>
    /// Schedule for Vehicle detection.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_VEHICLE")]
    public string? AiVehicle { get; set; }
}