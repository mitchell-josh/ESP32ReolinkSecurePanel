using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

/// <summary>
/// A lookup table defining the different security modes available in the system.
/// Matches the values found in the AlarmSchemeTypes enum.
/// </summary>
public class AlarmSchemeType
{
    /// <summary>
    /// The primary key, typically 0 for Disarmed, 1 for Partial, etc.
    /// </summary>
    [Key]
    public int AlarmSchemeTypeId { get; set; }
    
    /// <summary>
    /// The unique string identifier (e.g., "FullAlarm").
    /// </summary>
    [StringLength(50)]
    public required string Key { get; set; }
}