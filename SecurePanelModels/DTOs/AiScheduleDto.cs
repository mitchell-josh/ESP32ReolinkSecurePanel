namespace SecurePanelModels.DTOs;

public class AiScheduleDto
{
    public bool? PeopleEnabled { get; set; }
    
    public bool? CarsEnabled { get; set; }
    
    public bool? PetsEnabled { get; set; }
    
    public bool? OtherEnabled { get; set; }
}