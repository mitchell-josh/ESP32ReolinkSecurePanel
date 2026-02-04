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

    private static async Task SeedAlarmChannels(DbContext context, ReolinkClient reolinkClient)
    {
        var channelStatuses = await GetChannels(reolinkClient);
        
        var existingKeys = await context.Set<Channel>().Select(c => c.Key).ToListAsync();

        var newChannels = channelStatuses
            .Where(cs => !existingKeys.Contains(cs.Channel))
            .Select(cs => new Channel { Key = cs.Channel, Name = cs.Name })
            .ToList();

        if (newChannels.Any())
        {
            // Start a manual transaction to bypass the framework's "hanging" lock
            using var transaction = await context.Database.BeginTransactionAsync();
            try 
            {
                context.Set<Channel>().AddRange(newChannels);
                await context.SaveChangesAsync();
                await transaction.CommitAsync(); // This forces SQLite to write the WAL file
                Console.WriteLine($"[Seeder] Successfully committed {newChannels.Count} channels.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"[Seeder] Error saving channels: {ex.Message}");
                throw;
            }
        }
    }
    
    private static async Task<List<ChannelStatuses>> GetChannels(ReolinkClient reolinkClient)
    {
        var reolinkChannels = await reolinkClient?.GetChannelStatus()!;

        return reolinkChannels?.Value?.Statuses?.ToList() ?? [];
    }
}