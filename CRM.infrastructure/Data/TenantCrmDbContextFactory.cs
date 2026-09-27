using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CRM.infrastructure.Data;

/// <summary>
/// Design-time factory used only by EF Core tools (Add-Migration,
/// Update-Database). At runtime the app uses
/// AppServices.CreateTenantContext() which reads from Configuration.
///
/// EF looks for a class that implements IDesignTimeDbContextFactory<T>
/// in the same assembly as the DbContext. When found, it uses this to
/// build the context instead of asking the startup project to do it.
/// </summary>
public class TenantCrmDbContextFactory : IDesignTimeDbContextFactory<TenantCrmDbContext>
{
    public TenantCrmDbContext CreateDbContext(string[] args)
    {
        // Look for appsettings.json next to the compiled assembly first,
        // then fall back to the project folder (Package Manager Console
        // working directory).
        var basePath = AppContext.BaseDirectory;

        if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            basePath = Directory.GetCurrentDirectory();

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var cs = config.GetConnectionString("TenantCrm")
            ?? throw new InvalidOperationException(
                $"TenantCrm connection string missing from " +
                $"{Path.Combine(basePath, "appsettings.json")}.");

        var options = new DbContextOptionsBuilder<TenantCrmDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new TenantCrmDbContext(options);
    }
}