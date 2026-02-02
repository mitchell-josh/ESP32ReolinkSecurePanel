using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.Queries;

public class AlarmSchemeQuery
{
    public AlarmSchemeTypes? AlarmSchemeType { get; set; }
    
    public int Channel { get; set; }
}