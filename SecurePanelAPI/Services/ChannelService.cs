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

/// <summary>
/// Synchronizes physical hardware channels with the local database records.
/// Ensures the UI displays accurate names and connectivity status.
/// </summary>
public class ChannelService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IChannelService
{
    /// <summary>
    /// The high-level workflow: Discovers new hardware, updates status of old hardware, 
    /// and returns the final mapped DTO list.
    /// </summary>
    public async Task<AlarmResult<List<ChannelDto>>> GetChannels()
    {
        if (!(await this.CreateChannels()).Succeeded)
        {
            return AlarmResult<List<ChannelDto>>.Failure("Unable to create channels.");
        }

        if (!(await this.UpdateChannels()).Succeeded)
        {
            return AlarmResult<List<ChannelDto>>.Failure("Unable to update channels.");
        }
        
        var channels = await db.AlarmChannels
            .Select(c => new ChannelDto(
                c.AlarmChannelId,
                c.Name!,
                c.Identifier,
                c.Online))
            .ToListAsync();
        
        return AlarmResult<List<ChannelDto>>.Success(channels);
    }

    /// <summary>
    /// Compares Hardware IDs (Identifiers) against the DB. 
    /// Adds any cameras that have been newly plugged into the NVR.
    /// </summary>
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

    /// <summary>
    /// Detects changes in Names or Online status for existing records.
    /// </summary>
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