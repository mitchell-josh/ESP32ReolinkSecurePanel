using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class Audio
{
    [Key]
    public int AudioId { get; set; }
    
    public int? AiScheduleId { get; set; }
    
    [Required]
    public bool? Enabled { get; set; }
    
    public virtual AiSchedule? AiSchedule { get; set; }
}