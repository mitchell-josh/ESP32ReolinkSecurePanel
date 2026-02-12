using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

/// <summary>
/// A simplified representation of AI detection toggles for use in UI or external APIs.
/// </summary>
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

    /// <summary>
    /// Ensures all required detection toggles are present before processing.
    /// </summary>
    public bool Validate() =>
        this.PeopleEnabled.HasValue 
        && this.VehicleEnabled.HasValue 
        && this.PetsEnabled.HasValue 
        && this.OtherEnabled.HasValue;
}