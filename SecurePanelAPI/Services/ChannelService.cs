using ReolinkAPI.Clients;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class ChannelService(ReolinkClient reolinkClient) : IChannelService
{
    public async Task<List<ChannelDto>> GetChannels()
    {
        var reolinkChannels = await reolinkClient.GetChannelStatus();

        return reolinkChannels?.Value?.Statuses?.Select(c => new ChannelDto
        {
            Name = c.Name!,
            ChannelId = c.Channel ?? -1,
            Enabled = (c.Online == 1),
        }).ToList() ?? [];
    }
}