using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

/// <summary>
/// Represents a user authorized to interact with the security panel.
/// Stores identification and hashed authentication credentials.
/// </summary>
public class AlarmUser
{
    [Key]
    public int AlarmUserId { get; set; }
    
    /// <summary>
    /// The unique username or display name for the individual.
    /// </summary>
    [Required]
    public string? Username { get; set; }
    
    /// <summary>
    /// A salted hash of the 4-6 digit alarm PIN.
    /// Never store the raw code in the database.
    /// </summary>
    [Required]
    public string? AlarmCodeHash { get; set; }
}