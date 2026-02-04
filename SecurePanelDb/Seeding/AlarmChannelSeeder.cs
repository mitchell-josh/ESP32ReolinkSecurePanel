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
        var channelStatuses = await GetChannels(reolinkClient);
        
        var existingKeys = await db.Set<AlarmChannel>().Select(c => c.Identifier).ToListAsync();

        var newChannels = channelStatuses
            .Where(cs => !existingKeys.Contains(cs.Channel!.Value)) 
            .Select(cs => new AlarmChannel
            {
                Identifier = cs.Channel!.Value, 
                Name = cs.Name ?? string.Empty, 
                Online = cs.Online == 1
            })
            .ToList();

        if (newChannels.Any())
        {
            // Start a manual transaction to bypass the framework's "hanging" lock
            using var transaction = await db.Database.BeginTransactionAsync();
            try 
            {
                db.Set<AlarmChannel>().AddRange(newChannels);
                await db.SaveChangesAsync();
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
        return reolinkChannels?.Value?.Statuses?.Where(s => s.Channel.HasValue).ToList() ?? [];
    }
}