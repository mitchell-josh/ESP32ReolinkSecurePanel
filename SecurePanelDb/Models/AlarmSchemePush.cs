namespace SecurePanelDb.Models;

public class AlarmSchemePush
{
    public int AlarmSchemeId { get; set; }
    public virtual AlarmScheme? AlarmScheme { get; set; }
    
    public int PushId { get; set; }
    public virtual Push? Push { get; set; }
}