using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class AlarmChannel
{
    [Key]
    public int AlarmChannelId { get; set; }
    
    [StringLength(256)]
    public required string Name { get; set; }
    
    public required int Identifier { get; set; }
    
    public required bool Online { get; set; }
    
    public virtual ICollection<AlarmScheme> AlarmSchemes { get; set; } = [];
}