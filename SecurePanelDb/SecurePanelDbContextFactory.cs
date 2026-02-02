using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SecurePanelDb;

public class SecurePanelDbContextFactory : IDesignTimeDbContextFactory<SecurePanelDbContext>
{
    public SecurePanelDbContext CreateDbContext(string[] args)
    {
        throw new NotImplementedException();
    }
    //
    // private IConfigurationRoot GetConfiguration()
    // {
    //     return new ConfigurationBuilder()
    //         .SetBasePath(Directory.GetCurrentDirectory())
    //         .AddJsonFile("appsettings.json")
    //         .Build();
    // }
}