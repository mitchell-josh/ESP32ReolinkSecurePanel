using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelDb;
using SecurePanelDb.Models;

namespace SecurePanelAPI.Handlers;

/// <summary>
/// Handles the initial setup of the database, including schema migrations 
/// and the seeding of default administrative and hardware data.
/// </summary>
public static class DbMigrationHandler
{
    /// <summary>
    /// Executes database migrations and orchestrates the seeding process for users and hardware.
    /// </summary>
    /// <param name="serviceProvider">The root service provider used to resolve scoped dependencies.</param>
    public static async Task ConfigureDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<SecurePanelDbContext>();
    
        // Ensure schema is applied first
        await context.Database.MigrateAsync();

        // Initialize your seeder with the required services
        var reolink = services.GetRequiredService<ReolinkClient>();
        var seeder = new SecurePanelDbSeeder(context, reolink);

        // Seed User (Local/Fast)
        seeder.SeedDefaultUser(new PasswordHasher<AlarmUser>());

        // Seed Hardware (External/Async)
        // Wrap this in a try-catch so an offline camera doesn't crash the app
        try 
        {
            await seeder.SeedData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not fetch cameras during seeding: {ex.Message}");
        }

        // SQLite supports only one writer at a time. To prevent 'Database is locked' 
        // errors during startup, we stage all seeding changes in the ChangeTracker 
        // and commit them in a single atomic transaction here.
        await context.SaveChangesAsync();
    }
}