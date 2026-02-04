using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmSchedule
{
    [Key]
    public int AlarmScheduleId { get; set; }
    
    public bool AiDogCat { get; set; }
    
    public bool AiOther { get; set; }
    
    public bool AiPeople { get; set; }
    
    public bool AiVehicle { get; set; }
}