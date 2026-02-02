using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class Buzzer
{
    [Key]
    public int BuzzerId { get; set; }
    
    public int? AiScheduleId { get; set; }
    
    public int? ChannelId { get; set; }
    
    [Required]
    public bool? DiskErrorAlert { get; set; }
    
    [Required]
    public bool? DiskFullAlert { get; set; }
    
    [Required]
    public bool? Enabled { get; set; }
    
    [Required]
    public bool? IpConfigAlert { get; set; }
    
    [Required]
    public bool? NvrDisconnectAlert { get; set; }
    
    public virtual AiSchedule? AiSchedule { get; set; }
    
    public virtual Channel? Channel { get; set; }
}