using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class ChannelService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IChannelService
{
    public async Task<List<ChannelDto>> GetChannels()
    {
        var reolinkChannels = await reolinkClient.GetChannelStatus();
        
        return reolinkChannels?.Value?.Statuses?.Select(c => new ChannelDto
        {
            ChannelName = c.Name!,
            ChannelKey = c.Channel ?? -1,
            ChannelEnabled = (c.Online == 1),
        }).ToList() ?? [];
    }

    public async Task CreateChannels()
    {
        var response = await reolinkClient.GetChannelStatus();
        var reolinkStatuses = response?.Value?.Statuses;

        if (reolinkStatuses == null) return;

        var existingKeys = await db.Channels
            .Select(c => c.Key)
            .ToHashSetAsync();

        var newEntities = reolinkStatuses
            .Where(rc => !existingKeys.Contains(rc.Channel))
            .Select(rc => new Channel
            {
                Key = rc.Channel,
                Name = rc.Name,
            })
            .ToList();

        if (newEntities.Count != 0)
        {
            db.Channels.AddRange(newEntities);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateChannels()
    {
        var response = await reolinkClient.GetChannelStatus();
        var reolinkStatuses = response?.Value?.Statuses;
        
        if (reolinkStatuses == null) return;

        // Fetch db channels. No need for .ToList() yet, we can iterate the tracked entities
        var dbChannels = await db.Channels.ToListAsync();

        bool hasChanges = false;
        foreach (var dbChannel in dbChannels)
        {
            var reolinkMatch = reolinkStatuses.SingleOrDefault(rc => rc.Channel == dbChannel.Key);

            if (reolinkMatch != null && !string.IsNullOrWhiteSpace(reolinkMatch.Name) && dbChannel.Name != reolinkMatch.Name)
            {
                dbChannel.Name = reolinkMatch.Name;
                hasChanges = true;
            }
        }
        
        if (hasChanges)
        {
            await db.SaveChangesAsync();
        }
    }
}