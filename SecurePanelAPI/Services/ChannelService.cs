using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Channels;
using ReolinkAPI.Clients;
using ReolinkAPI.Handlers;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class ChannelService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IChannelService
{
    public async Task<AlarmResult<List<ChannelDto>>> GetChannels()
    {
        var channels = await db.AlarmChannels
            .Select(c => new ChannelDto
            {
                ChannelId = c.AlarmChannelId,
                ChannelName = c.Name!,
                ChannelKey = c.Identifier,
                ChannelEnabled = c.Online,
            })
            .ToListAsync();
        
        return AlarmResult<List<ChannelDto>>.Success(channels);
    }

    public async Task<AlarmResult<bool>> CreateChannels()
    {
        var result = await reolinkClient.GetChannelStatus();
        
        var response = ReolinkHandler.ProcessResponse(result);

        if (!response.Succeeded)
        {
            return AlarmResult<bool>.Failure(response.ErrorMessage!);
        }
        
        var reolinkStatuses = result?.Value?.Value?.Statuses
            ?.Where(s => s.Channel.HasValue)
            ?.ToList() ?? [];

        if (!reolinkStatuses.Any())
        {
            return AlarmResult<bool>.Success(true);
        }

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
        
        return AlarmResult<bool>.Success(true);
    }

    public async Task<AlarmResult<bool>> UpdateChannels()
    {
        var result = await reolinkClient.GetChannelStatus();
        
        var response = ReolinkHandler.ProcessResponse(result);

        if (!response.Succeeded)
        {
            return AlarmResult<bool>.Failure(response.ErrorMessage!);
        }

        var reolinkStatuses = result?.Value?.Value?.Statuses ?? [];
        
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
        
        return AlarmResult<bool>.Success(true);
    }
}