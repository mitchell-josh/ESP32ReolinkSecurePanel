using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

/// <summary>
/// Database entity representing a physical Reolink camera channel or lens.
/// </summary>
public class AlarmChannel
{
    /// <summary>
    /// Internal Database Primary Key.
    /// </summary>
    [Key]
    public int AlarmChannelId { get; set; }
    
    /// <summary>
    /// User-defined name for the camera (e.g., "Front Door").
    /// </summary>
    [StringLength(256)]
    public required string Name { get; set; }
    
    /// <summary>
    /// The physical hardware index on the Reolink device (0, 1, 2...).
    /// Corresponds to 'channel' in API requests.
    /// </summary>
    public required int Identifier { get; set; }
    
    /// <summary>
    /// Tracks the last known availability of the hardware.
    /// </summary>
    public required bool Online { get; set; }
    
    /// <summary>
    /// Navigation property to the various alarm configurations for this specific camera.
    /// </summary>
    public virtual ICollection<AlarmScheme> AlarmSchemes { get; set; } = [];
}