using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

public class AlarmSchemeTypeDto
{
    [JsonPropertyName("alarmSchemeTypeId")]
    public int? AlarmSchemeTypeId { get; set; }
    
    [JsonPropertyName("key")]
    public string? Key { get; set; }
}