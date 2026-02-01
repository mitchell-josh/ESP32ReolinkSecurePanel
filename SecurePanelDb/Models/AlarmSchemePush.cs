namespace SecurePanelDb.Models;

public class AlarmSchemePush
{
    public int AlarmSchemeId { get; set; }
    public virtual AlarmScheme? AlarmScheme { get; set; }
    
    public int BuzzerId { get; set; }
    public virtual Push? Buzzer { get; set; }
}