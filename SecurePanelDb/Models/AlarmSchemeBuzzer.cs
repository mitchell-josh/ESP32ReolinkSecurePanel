namespace SecurePanelDb.Models;

public class AlarmSchemeBuzzer
{
    public int AlarmSchemeId { get; set; }
    public virtual AlarmScheme? AlarmScheme { get; set; }
    
    public int BuzzerId { get; set; }
    public virtual Buzzer? Buzzer { get; set; }
}