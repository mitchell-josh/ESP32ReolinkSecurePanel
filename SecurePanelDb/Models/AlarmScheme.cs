using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmScheme
{
    [Key]
    public int AlarmSchemeId { get; set; }
    
    public required int AlarmChannelId { get; set; }
    
    public required int AlarmSchemeTypeId { get; set; }
    
    public int? AlarmScheduleId { get; set; }
    
    public required bool Enabled { get; set; }
    
    public required bool PushEnabled { get; set; }
    
    public required DateTime DateCreated { get; set; }
    
    public virtual AlarmSchemeType? AlarmSchemeType { get; set; }
    
    public virtual AlarmChannel? AlarmChannel { get; set; }
    
    public virtual AlarmSchedule? AlarmSchedule { get; set; }
}