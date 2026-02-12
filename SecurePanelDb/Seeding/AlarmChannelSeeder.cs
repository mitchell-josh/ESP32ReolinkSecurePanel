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
        // Fetch current hardware state from the NVR/Camera
        var channelStatuses = await GetChannels(reolinkClient);
        
        // Identify what we already know. We use 'Identifier' (Hardware Index) 
        // rather than 'AlarmChannelId' (Database PK) to track identity.
        var existingKeys = await db.Set<AlarmChannel>().Select(c => c.Identifier).ToListAsync();

        // Only prepare to insert channels that don't exist in our DB yet.
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

        return result.Value?.Value?.Statuses?.Where(s => s.Channel.HasValue)?.ToList() ?? [];
    }
}