using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AiSchedule
{
    [Key]
    public int AiScheduleId { get; set; }
    
    public bool AiDogCat { get; set; }
    
    public bool AiOther { get; set; }
    
    public bool AiPeople { get; set; }
    
    public bool AiVehicle { get; set; }
}