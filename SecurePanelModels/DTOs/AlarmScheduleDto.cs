using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

public class AlarmScheduleDto
{
    [JsonPropertyName("peopleEnabled")]
    public bool? PeopleEnabled { get; set; }
    
    [JsonPropertyName("vehicleEnabled")]
    public bool? VehicleEnabled { get; set; }
    
    [JsonPropertyName("petsEnabled")]
    public bool? PetsEnabled { get; set; }
    
    [JsonPropertyName("otherEnabled")]
    public bool? OtherEnabled { get; set; }

    public bool Validate() =>
        this.PeopleEnabled.HasValue 
        && this.VehicleEnabled.HasValue 
        && this.PetsEnabled.HasValue 
        && this.OtherEnabled.HasValue;
}