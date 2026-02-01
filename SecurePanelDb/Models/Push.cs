using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class Push
{
    [Key]
    public int PushId { get; set; }
    
    public int? AiScheduleId { get; set; }
    
    [Required]
    public bool? Enabled { get; set; }
    
    [Required]
    public bool? ScheduleEnabled { get; set; }
    
    public virtual AiSchedule? AiSchedule { get; set; }
}