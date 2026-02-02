using Microsoft.EntityFrameworkCore;
using SecurePanelDb.Models;

namespace SecurePanelDb;

public class SecurePanelDbContext : DbContext
{
    public SecurePanelDbContext(DbContextOptions<SecurePanelDbContext> options) : base(options)
    {
    }
    
    public DbSet<AiSchedule> AiSchedules { get; set; }
    
    public DbSet<AlarmScheme> AlarmSchemes { get; set; }
    
    public DbSet<AlarmSchemeAudio> AlarmSchemeAudios { get; set; }
    
    public DbSet<AlarmSchemeBuzzer> AlarmSchemeBuzzers { get; set; }
    
    public DbSet<AlarmSchemePush> AlarmSchemePushes { get; set; }
    
    public DbSet<AlarmSchemeType> AlarmSchemeTypes { get; set; }
    
    public DbSet<AlarmUser> AlarmUsers { get; set; }
    
    public DbSet<Audio> Audios { get; set; }
    
    public DbSet<Buzzer> Buzzers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlarmSchemeAudio>().HasKey(e => new { e.AlarmSchemeId, e.AudioId });
        modelBuilder.Entity<AlarmSchemeBuzzer>().HasKey(e => new { e.AlarmSchemeId, e.BuzzerId });
        modelBuilder.Entity<AlarmSchemePush>().HasKey(e => new { e.AlarmSchemeId, e.PushId });
        
        base.OnModelCreating(modelBuilder);
    }
}