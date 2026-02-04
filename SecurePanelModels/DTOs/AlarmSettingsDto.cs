using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.DTOs;

public class AlarmSettingsDto
{
    public int? ChannelId { get; set; }
    
    public string? ChannelName { get; set; }
    
    public AlarmSchemeTypes AlarmSchemeType { get; set; }
    
    public bool? Enabled { get; set; }

    public AiScheduleDto AiSchedule { get; set; }
}