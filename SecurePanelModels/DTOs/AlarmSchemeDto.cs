using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.DTOs;

public class AlarmSchemeDto
{
    public int? AlarmSchemeId { get; set; }
    
    public int? AlarmChannelId { get; set; }
    
    public int? AlarmSchemeTypeId { get; set; }
    
    public bool? Enabled { get; set; }

    public AlarmScheduleDto? Schedule { get; set; }
}