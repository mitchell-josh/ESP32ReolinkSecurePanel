using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.Queries;

public class AlarmSchemeQuery
{
    public int? AlarmSchemeId { get; set; }
    
    public AlarmSchemeTypes? AlarmSchemeType { get; set; }
    
    public int? ChannelId { get; set; }
}