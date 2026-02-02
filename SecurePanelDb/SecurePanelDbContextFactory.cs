using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SecurePanelDb;

public class SecurePanelDbContextFactory : IDesignTimeDbContextFactory<SecurePanelDbContext>
{
    public SecurePanelDbContext CreateDbContext(string[] args)
    {
        var configuration = this.GetConfiguration();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        var optionsBuilder = new DbContextOptionsBuilder<SecurePanelDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new SecurePanelDbContext(optionsBuilder.Options);
    }
    
    private IConfigurationRoot GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}