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
    
    public DbSet<Audio> Audios { get; set; }
    
    public DbSet<Buzzer> Buzzers { get; set; }
}