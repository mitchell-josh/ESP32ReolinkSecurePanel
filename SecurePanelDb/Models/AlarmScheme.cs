using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmScheme
{
    [Key]
    public int AlarmSchemeId { get; set; }
    
    public int? AlarmSchemeTypeId { get; set; }
    
    public virtual AlarmSchemeType? AlarmSchemeType { get; set; }
}