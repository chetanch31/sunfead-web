using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SunfeadApi.Data;

/// <summary>
/// design-time factory for creating ApplicationDbContext during migrations
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // use a default connection string for design-time operations
        optionsBuilder.UseSqlServer("Server=localhost;Database=sunfead_dev;Trusted_Connection=True;TrustServerCertificate=True;");
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
