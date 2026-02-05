using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.Queries;

public class AlarmSchemeQuery
{
    public int? AlarmSchemeId { get; set; }
    
    public int? AlarmSchemeTypeId { get; set; }
    
    public int? ChannelId { get; set; }
}