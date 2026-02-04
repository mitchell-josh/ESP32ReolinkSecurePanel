using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class Channel
{
    [Key]
    public int ChannelId { get; set; }
    
    public int? Key { get; set; }
    
    public string? Name { get; set; }
}