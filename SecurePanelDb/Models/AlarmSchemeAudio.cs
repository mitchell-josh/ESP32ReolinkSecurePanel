namespace SecurePanelDb.Models;

public class AlarmSchemeAudio
{
    public int AlarmSchemeId { get; set; }
    public virtual AlarmScheme? AlarmScheme { get; set; }
    
    public int AudioId { get; set; }
    public virtual Audio? Audio { get; set; }
}