using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

public interface IChannelService
{
    Task<AlarmResult<List<ChannelDto>>> GetChannels();

    Task<AlarmResult<bool>> CreateChannels();

    Task<AlarmResult<bool>> UpdateChannels();
}