using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Channels;
using ReolinkAPI.Clients;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelDb.Seeding;

public class AlarmChannelSeeder
{
    /// <summary>
    /// Entry point for the seeding process. 
    /// Coordinates the synchronization between Reolink Hardware and the SQL Database.
    /// </summary>
    public static async Task SeedData(DbContext context, ReolinkClient reolinkClient)
    {
        await SeedAlarmChannels(context, reolinkClient);
    }

    private static async Task SeedAlarmChannels(DbContext db, ReolinkClient reolinkClient)
    {
        var channelStatuses = await GetChannels(reolinkClient);
        var existingChannels = await db.Set<AlarmChannel>().ToListAsync();

        bool hasChanges = false;

        foreach (var cs in channelStatuses.Where(x => x.Channel.HasValue))
        {
            var existing = existingChannels.FirstOrDefault(c => c.Identifier == cs.Channel.Value);
        
            System.Diagnostics.Debug.WriteLine($"[SEEDER] Seeding Alarm Channel: {cs.Channel} {cs.Name}");
            
            if (existing == null)
            {
                System.Diagnostics.Debug.WriteLine($"[SEEDER] Adding missing Alarm Channel: {cs.Channel} {cs.Name}");
                // NEW CHANNEL
                db.Set<AlarmChannel>().Add(new AlarmChannel
                {
                    Identifier = cs.Channel.Value,
                    Name = cs.Name ?? $"Channel {cs.Channel}",
                    Online = cs.Online == 1
                });
                hasChanges = true;
            }
            else if (existing.Name != cs.Name || existing.Online != (cs.Online == 1))
            {
                System.Diagnostics.Debug.WriteLine($"[SEEDER] Updating Alarm Channel: {cs.Channel} {cs.Name}");
                // UPDATE EXISTING
                existing.Name = cs.Name ?? existing.Name;
                existing.Online = cs.Online == 1;
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            using var transaction = await db.Database.BeginTransactionAsync();
            try 
            {
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
                System.Diagnostics.Debug.WriteLine("[Seeder] Database synchronized with camera state.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Diagnostics.Debug.WriteLine($"[Seeder] Error: {ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// Private helper to wrap the Reolink Client call. 
    /// Gracefully handles API failures by returning an empty list instead of null.
    /// </summary>
    private static async Task<List<ChannelStatuses>> GetChannels(ReolinkClient reolinkClient)
    {
        var result = await reolinkClient.GetChannelStatus();

        if (result.Code != 0)
        { // Failed to grab channels - seed empty array.
            return [];
        }

        return result.Value?.Statuses?.Where(s => s.Channel.HasValue)?.ToList() ?? [];
    }
}