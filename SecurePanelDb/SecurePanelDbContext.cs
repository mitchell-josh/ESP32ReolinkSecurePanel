using Microsoft.EntityFrameworkCore;
using SecurePanelDb.Models;

namespace SecurePanelDb;

/// <summary>
/// The primary gateway for interacting with the security system database.
/// Manages the lifecycle of users, hardware channels, and alarm configurations.
/// </summary>
public class SecurePanelDbContext : DbContext
{
    public SecurePanelDbContext(DbContextOptions<SecurePanelDbContext> options) : base(options)
    {
    }
    
    // Core hardware and logic tables
    
    public DbSet<AlarmSchedule> AlarmSchedules { get; set; }
    
    public DbSet<AlarmScheme> AlarmSchemes { get; set; }
    
    public DbSet<AlarmSchemeType> AlarmSchemeTypes { get; set; }
    
    // Identity and physical mapping tables
    
    public DbSet<AlarmUser> AlarmUsers { get; set; }
    
    public DbSet<AlarmChannel> AlarmChannels { get; set; }

    /// <summary>
    /// Configures the relational mappings and shadow properties.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlarmScheme>()
            .Property<DateTime>("LastModified");
        
        base.OnModelCreating(modelBuilder);
    }
}