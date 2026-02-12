using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelDb.Seeding;

public static class AlarmSchemeTypeSeeder
{
    /// <summary>
    /// Populates the lookup table for security modes (Disarmed, Home, Away).
    /// This ensures the database referential integrity for AlarmSchemes is satisfied.
    /// </summary>
    public static void SeedAlarmSchemeTypes(DbContext context)
    {   
        // Prevents primary key violations or duplicate entries on subsequent application restarts.
        if (!context.Set<AlarmSchemeType>().Any())
        {
            new List<string>
            {
                nameof(AlarmSchemeTypes.Disarmed),
                nameof(AlarmSchemeTypes.FullAlarm),
                nameof(AlarmSchemeTypes.PartialAlarm),
            }.ForEach(s => context.Add(new AlarmSchemeType { Key = s }));  
        } 
    }
}