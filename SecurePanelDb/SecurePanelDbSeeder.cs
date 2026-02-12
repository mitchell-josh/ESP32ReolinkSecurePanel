using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelDb.Models;
using SecurePanelDb.Seeding;

namespace SecurePanelDb;

/// <summary>
/// The top-level orchestrator for database initialization.
/// Combines hardware discovery, lookup data, and identity seeding.
/// </summary>
public class SecurePanelDbSeeder(DbContext context, ReolinkClient reolinkClient)
{
    /// <summary>
    /// Executes the primary data synchronization tasks.
    /// Ensures security modes exist and cameras are discovered.
    /// </summary>
    public async Task SeedData()
    {
        AlarmSchemeTypeSeeder.SeedAlarmSchemeTypes(context);

        await AlarmChannelSeeder.SeedData(context, reolinkClient);
    }

    /// <summary>
    /// Ensures at least one administrative user exists.
    /// Uses ASP.NET Core Identity hashing to protect the numeric PIN.
    /// </summary>
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