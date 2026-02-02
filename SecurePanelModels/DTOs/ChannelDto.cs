namespace SecurePanelModels.DTOs;

public class ChannelDto
{
    public required string Name { get; set; }
    
    public required int ChannelId { get; set; }
    
    public required bool Enabled { get; set; }
}