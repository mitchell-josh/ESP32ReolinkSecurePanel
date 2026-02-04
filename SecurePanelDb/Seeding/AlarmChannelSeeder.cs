using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Channels;
using ReolinkAPI.Clients;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelDb.Seeding;

public class AlarmChannelSeeder
{
    public static async Task SeedData(DbContext context, ReolinkClient reolinkClient)
    {
        await SeedAlarmChannels(context, reolinkClient);
    }

    private static async Task SeedAlarmChannels(DbContext db, ReolinkClient reolinkClient)
    {
        // Get reolink channels from reolink API
        var reolinkChannels = await GetChannels(reolinkClient);
        
        // Fetch existing keys from database
        var existingKeys = await db.Set<AlarmChannel>().Select(c => c.Identifier).ToListAsync();

        // Add missing keys to AlarmChannels table.
        reolinkChannels
            .Where(cs => !existingKeys.Contains(cs.Channel!.Value)) 
            .Select(cs => new AlarmChannel
            {
                Identifier = cs.Channel!.Value, 
                Name = cs.Name ?? string.Empty, 
                Online = cs.Online == 1
            })
            .ToList()
            .ForEach(ac => db.Set<AlarmChannel>().Add(ac));
        
        await db.SaveChangesAsync();
    }
    
    private static async Task<List<ChannelStatuses>> GetChannels(ReolinkClient reolinkClient)
    {
        var reolinkChannels = await reolinkClient?.GetChannelStatus()!;
        return reolinkChannels?.Value?.Statuses?.Where(s => s.Channel.HasValue).ToList() ?? [];
    }
}