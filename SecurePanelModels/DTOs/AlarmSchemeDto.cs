using System.Text.Json.Serialization;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.DTOs;

public class AlarmSchemeDto
{
    [JsonPropertyName("alarmSchemeId")]
    public int? AlarmSchemeId { get; set; }
    
    [JsonPropertyName("alarmChannelId")]
    public int? AlarmChannelId { get; set; }
    
    [JsonPropertyName("alarmSchemeTypeId")]
    public int? AlarmSchemeTypeId { get; set; }
    
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
    
    [JsonPropertyName("pushEnabled")]
    public bool? PushEnabled { get; set; }

    [JsonPropertyName("schedule")]
    public AlarmScheduleDto? Schedule { get; set; }

    public bool Validate() =>
        this.AlarmSchemeId.HasValue
        && this.AlarmChannelId.HasValue
        && this.AlarmSchemeTypeId.HasValue
        && this.Enabled.HasValue
        && this.PushEnabled.HasValue
        && (this.Schedule?.Validate() ?? false);
}