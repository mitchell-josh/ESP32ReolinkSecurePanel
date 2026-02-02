using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;

namespace SecurePanelDb.Seeding;

public static class AlarmSchemeTypeSeeder
{
    public static void SeedAlarmSchemeTypes(DbContext context)
    {
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