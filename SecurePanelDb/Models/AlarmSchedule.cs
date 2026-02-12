using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

/// <summary>
/// Database entity storing the AI detection preferences for a specific alarm profile.
/// </summary>
public class AlarmSchedule
{
    /// <summary>
    /// Internal Database Primary Key.
    /// </summary>
    [Key]
    public int AlarmScheduleId { get; set; }
    
    public required bool PetsEnabled { get; set; }
    
    public required bool OtherEnabled { get; set; }
    
    public required bool PeopleEnabled { get; set; }
    
    public required bool VehicleEnabled { get; set; }

    /// <summary>
    /// Navigation property back to the parent AlarmScheme.
    /// </summary>
    public virtual AlarmScheme? AlarmScheme { get; set; }
}