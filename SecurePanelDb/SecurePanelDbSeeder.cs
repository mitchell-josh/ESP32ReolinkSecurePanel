using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelDb.Models;
using SecurePanelDb.Seeding;

namespace SecurePanelDb;

public class SecurePanelDbSeeder(DbContext context, ReolinkClient reolinkClient)
{
    public async Task SeedData()
    {
        AlarmSchemeTypeSeeder.SeedAlarmSchemeTypes(context);

        await AlarmChannelSeeder.SeedData(context, reolinkClient);
    }

    public void SeedDefaultUser(IPasswordHasher<AlarmUser> hasher)
    {
        if (!context.Set<AlarmUser>().Any(u => u.Username == "Admin")) 
        {
            var defaultUser = new AlarmUser
            {
                Username = "Admin"
            };
            defaultUser.AlarmCodeHash = hasher.HashPassword(defaultUser, "0000");
        
            context.Set<AlarmUser>().Add(defaultUser);
            
        }
    }
}