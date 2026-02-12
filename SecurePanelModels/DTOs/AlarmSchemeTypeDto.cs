using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

/// <summary>
/// Data Transfer Object representing the metadata for a security mode.
/// This maps the integer ID used in logic to a string identifier used in UI/DB.
/// </summary>
public class AlarmSchemeTypeDto
{
    /// <summary>
    /// The unique identifier for the scheme type.
    /// Typically corresponds to values in the <see cref="AlarmSchemeTypes"/> enum.
    /// </summary>
    [JsonPropertyName("alarmSchemeTypeId")]
    public int? AlarmSchemeTypeId { get; set; }
    
    /// <summary>
    /// The string representation of the scheme (e.g., "Disarmed", "Partial", "Full").
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }
}