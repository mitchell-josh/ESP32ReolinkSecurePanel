using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmSchemeType
{
    [Key]
    public int AlarmSchemeTypeId { get; set; }
    
    [StringLength(50)]
    public required string Key { get; set; }
}