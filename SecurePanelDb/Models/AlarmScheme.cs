using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

/// <summary>
/// The core configuration entity that defines how a specific camera 
/// behaves during a specific security mode.
/// </summary>
public class AlarmScheme
{
    [Key]
    public int AlarmSchemeId { get; set; }
 
    /// <summary>
    /// Foreign Key to the physical camera channel.
    /// </summary>
    public required int AlarmChannelId { get; set; }
    
    /// <summary>
    /// Foreign Key to the mode (Disarmed, Partial, Full).
    /// </summary>
    public required int AlarmSchemeTypeId { get; set; }
    
    /// <summary>
    /// Foreign Key to the AI detection schedule. 
    /// Nullable in case a scheme doesn't require AI configuration.
    /// </summary>
    public int? AlarmScheduleId { get; set; }
    
    /// <summary>
    /// Master toggle for hardware deterrents (Buzzer/Siren).
    /// </summary>
    public required bool Enabled { get; set; }
    
    /// <summary>
    /// Toggle for mobile push notifications.
    /// </summary>
    public required bool PushEnabled { get; set; }
    
    public required DateTime DateCreated { get; set; }
    
    // --- Navigation Properties ---
    
    public virtual AlarmSchemeType? AlarmSchemeType { get; set; }
    
    public virtual AlarmChannel? AlarmChannel { get; set; }
    
    public virtual AlarmSchedule? AlarmSchedule { get; set; }
}