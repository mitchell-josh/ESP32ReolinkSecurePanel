using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmSchedule
{
    [Key]
    public int AlarmScheduleId { get; set; }
    
    public required bool PetsEnabled { get; set; }
    
    public required bool OtherEnabled { get; set; }
    
    public required bool PeopleEnabled { get; set; }
    
    public required bool VehicleEnabled { get; set; }

    public virtual AlarmScheme? AlarmScheme { get; set; }
}