namespace SecurePanelModels.DTOs;

public class BuzzerDto
{
    public int? BuzzerId { get; set; }
    
    public int? AiScheduleId { get; set; }
    
    public int? Channel { get; set; }
    
    public bool? DiskErrorAlert { get; set; }
    
    public bool? DiskFullAlert { get; set; }
    
    public bool? Enabled { get; set; }
    
    public bool? IpConfigAlert { get; set; }
    
    public bool? NvrDisconnectAlert { get; set; }
}