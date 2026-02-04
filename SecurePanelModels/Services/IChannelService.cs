using SecurePanelModels.DTOs;

namespace SecurePanelModels.Services;

public interface IChannelService
{
    Task<List<ChannelDto>> GetChannels();

    Task CreateChannels();

    Task UpdateChannels();
}