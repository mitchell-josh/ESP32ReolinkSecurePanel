using Microsoft.EntityFrameworkCore;
using SecurePanelDb.Models;

namespace SecurePanelDb;

public class SecurePanelDbContext : DbContext
{
    public SecurePanelDbContext(DbContextOptions<SecurePanelDbContext> options) : base(options)
    {
    }
    
    public DbSet<AlarmSchedule> AlarmSchedules { get; set; }
    
    public DbSet<AlarmScheme> AlarmSchemes { get; set; }
    
    public DbSet<AlarmSchemeType> AlarmSchemeTypes { get; set; }
    
    public DbSet<AlarmUser> AlarmUsers { get; set; }
    
    public DbSet<AlarmChannel> AlarmChannels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlarmScheme>()
            .Property<DateTime>("LastModified");
        
        base.OnModelCreating(modelBuilder);
    }
}