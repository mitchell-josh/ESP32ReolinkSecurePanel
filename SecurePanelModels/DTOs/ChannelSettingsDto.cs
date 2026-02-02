namespace SecurePanelModels.DTOs;

public class ChannelSettingsDto
{
    public int? ChannelId { get; set; }
    
    public bool? Enabled { get; set; }
    
    public bool? PeopleEnabled { get; set; }
    
    public bool? CarsEnabled { get; set; }
    
    public bool? PetsEnabled { get; set; }
    
    public bool? OtherEnabled { get; set; }
}