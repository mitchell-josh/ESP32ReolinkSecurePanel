using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SecurePanelDb;

/// <summary>
/// Provides a way for EF Core CLI tools to create a DbContext instance at design time.
/// This enables 'dotnet ef migrations add' to work correctly outside of the main app.
/// </summary>
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
    
    /// <summary>
    /// Builds a temporary configuration root to read the connection string.
    /// </summary>
    private IConfigurationRoot GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}