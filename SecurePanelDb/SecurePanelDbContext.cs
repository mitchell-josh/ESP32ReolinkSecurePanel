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
    
    public DbSet<AlarmSettings> AlarmSettings { get; set; }
    
    public DbSet<AlarmUser> AlarmUsers { get; set; }
    
    public DbSet<Channel> Channels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlarmScheme>()
            .Property<DateTime>("LastModified");
        
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = this.ChangeTracker
            .Entries()
            .Where(e => (e.State is EntityState.Added or EntityState.Modified) && (e.Entity is AlarmScheme));
        
        foreach (var entry in entries)
        {
            entry.Property("LastModified").CurrentValue = DateTime.UtcNow;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}