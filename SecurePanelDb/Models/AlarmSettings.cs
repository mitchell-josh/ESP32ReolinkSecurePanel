using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmSettings
{
    [Key]
    public int AlarmSettingsId { get; set; }
    
    public int? AlarmSchemeId { get; set; }
    
    public int? AlarmScheduleId { get; set; }
    
    public int? ChannelId { get; set; }
    
    public bool? Enabled { get; set; }
    
    public virtual AlarmScheme? AlarmScheme { get; set; }
    
    public virtual Channel? Channel { get; set; }
    
    public virtual AlarmSchedule? AlarmSchedule { get; set; }
}