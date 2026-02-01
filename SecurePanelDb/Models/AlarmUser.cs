using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmUser
{
    [Key]
    public int AlarmUserId { get; set; }
    
    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? PinCodeHash { get; set; }
}