using System.ComponentModel.DataAnnotations;

namespace SecurePanelDb.Models;

public class Channel
{
    [Key]
    public int ChannelId { get; set; }
    
    public int? ChannelKey { get; set; }
    
    public string? ChannelName { get; set; }
}