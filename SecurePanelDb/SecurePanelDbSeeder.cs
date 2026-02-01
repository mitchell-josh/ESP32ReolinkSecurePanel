using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurePanelDb.Models;

namespace SecurePanelDb;

public class SecurePanelDbSeeder(DbContext context)
{
    public void SeedData()
    {
    }

    public void SeedDefaultUser(IPasswordHasher<AlarmUser> hasher)
    {
        var defaultUser = new AlarmUser
        {
            Username = "Admin"
        };
        defaultUser.AlarmCodeHash = hasher.HashPassword(defaultUser, "0000");
        
        context.Set<AlarmUser>().Add(defaultUser);
        context.SaveChanges();
    }
}