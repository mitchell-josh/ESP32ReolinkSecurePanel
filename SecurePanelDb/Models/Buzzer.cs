using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class Buzzer
{
    [Key]
    public int BuzzerId { get; set; }
    
    public int? AiScheduleId { get; set; }
    
    [Required]
    public int? Channel { get; set; }
    
    [Required]
    public bool? DiskErrorAlert { get; set; }
    
    [Required]
    public bool? DiskFullAlert { get; set; }
    
    [Required]
    public bool? Enable { get; set; }
    
    [Required]
    public bool? IpConfigAlert { get; set; }
    
    [Required]
    public bool? NvrDisconnectAlert { get; set; }
    
    public virtual AiSchedule? AiSchedule { get; set; }
}