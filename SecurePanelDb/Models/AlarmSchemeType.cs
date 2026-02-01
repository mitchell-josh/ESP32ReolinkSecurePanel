using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmSchemeType
{
    [Key]
    public int AlarmSchemeTypeId { get; set; }
    
    [Required]
    public string? Key { get; set; }
}