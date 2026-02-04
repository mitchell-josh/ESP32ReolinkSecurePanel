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
        var channels = await db.AlarmChannels.ToListAsync();
        
        return channels.Select(c => new ChannelDto
        {
            ChannelName = c.Name!,
            ChannelKey = c.Identifier,
            ChannelEnabled = c.Online,
        }).ToList();
    }

    public async Task CreateChannels()
    {
        var response = await reolinkClient.GetChannelStatus();

        var reolinkStatuses = response?.Value?.Statuses
            ?.Where(s => s.Channel.HasValue)
            ?.ToList() ?? [];

        if (!reolinkStatuses.Any()) return;

        var existingKeys = await db.AlarmChannels
            .Select(c => c.Identifier)
            .ToHashSetAsync();

        var newEntities = reolinkStatuses
            .Where(rc => !existingKeys.Contains(rc.Channel!.Value))
            .Select(rc => new AlarmChannel
            {
                Identifier = rc.Channel!.Value,
                Name = rc.Name ?? string.Empty,
                Online = rc.Online == 1
            })
            .ToList();

        if (newEntities.Count != 0)
        {
            db.AlarmChannels.AddRange(newEntities);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateChannels()
    {
        var response = await reolinkClient.GetChannelStatus();
        var reolinkStatuses = response?.Value?.Statuses;
        
        if (reolinkStatuses == null) return;

        // Fetch db channels. No need for .ToList() yet, we can iterate the tracked entities
        var dbChannels = await db.AlarmChannels.ToListAsync();

        bool hasChanges = false;
        foreach (var dbChannel in dbChannels)
        {
            var reolinkMatch = reolinkStatuses.SingleOrDefault(rc => rc.Channel == dbChannel.Identifier);

            if (reolinkMatch != null && !string.IsNullOrWhiteSpace(reolinkMatch.Name) && dbChannel.Name != reolinkMatch.Name)
            {
                dbChannel.Name = reolinkMatch.Name;
                hasChanges = true;
            }

            if (reolinkMatch != null && (dbChannel.Online != (reolinkMatch.Online == 1)))
            {
                dbChannel.Online = (reolinkMatch.Online != 1);
                hasChanges = true;
            }
        }
        
        if (hasChanges)
        {
            await db.SaveChangesAsync();
        }
    }
}