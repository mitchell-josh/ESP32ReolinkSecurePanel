namespace SecurePanelModels.DTOs;

public class AlarmScheduleDto
{
    public bool? PeopleEnabled { get; set; }
    
    public bool? VehicleEnabled { get; set; }
    
    public bool? PetsEnabled { get; set; }
    
    public bool? OtherEnabled { get; set; }
}