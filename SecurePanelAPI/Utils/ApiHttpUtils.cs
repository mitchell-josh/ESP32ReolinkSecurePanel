using Microsoft.EntityFrameworkCore;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Utils;

/// <summary>
/// Shared HTTP utilities accessed by various parts of the SecurePanelAPI, including service classes.
/// </summary>
public static class ApiHttpUtils
{
    /// <summary>
    /// Fetch <see cref="AlarmScheme"/> database model from internal SecurePanelAPI db.
    /// </summary>
    public static async Task<AlarmScheme?> GetScheme(SecurePanelDbContext db, AlarmSchemeQuery query)
    { 
        string? alarmSchemeType = query.AlarmSchemeType!.ToString();
        return (await GetChannel(db, query.ChannelId!.Value))
            ?.AlarmSchemes
            ?.Where(s => s.AlarmSchemeType!.Key == alarmSchemeType)
            ?.OrderByDescending(s => s.DateCreated)
            ?.FirstOrDefault();
    }
    
    /// <summary>
    /// Fetch <see cref="Channel"/> database model from internal SecurePanelAPI db.
    /// </summary>
    private static async Task<AlarmChannel?> GetChannel(SecurePanelDbContext db, int channelId)
        => await db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchedule)
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchemeType)
            .SingleOrDefaultAsync(c => c.AlarmChannelId == channelId);
}