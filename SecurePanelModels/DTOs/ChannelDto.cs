namespace SecurePanelModels.DTOs;

public class ChannelDto
{
    public required string ChannelName { get; set; }
    
    public required int ChannelKey { get; set; }
    
    public required bool ChannelEnabled { get; set; }
}