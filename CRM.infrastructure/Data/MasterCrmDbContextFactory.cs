using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CRM.infrastructure.Data;

public class MasterCrmDbContextFactory : IDesignTimeDbContextFactory<MasterCrmDbContext>
{
    public MasterCrmDbContext CreateDbContext(string[] args)
    {
        var config = BuildConfiguration();

        var cs = config.GetConnectionString("MasterCrm")
            ?? throw new InvalidOperationException(
                "Connection string 'MasterCrm' not found in appsettings.json.");

        var options = new DbContextOptionsBuilder<MasterCrmDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new MasterCrmDbContext(options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        // Search these folders in order. First one that contains appsettings.json wins.
        // This makes the factory work whether EF runs from CRM.api's bin, CRM.winForms's bin,
        // or anywhere else.
        var candidates = new[]
        {
            AppContext.BaseDirectory,                                                    // bin of the startup project
            Directory.GetCurrentDirectory(),                                             // PMC's current folder
            Path.Combine(AppContext.BaseDirectory, "..", "..", ".."),                    // climb out of bin/Debug/netX
            Path.Combine(Directory.GetCurrentDirectory(), "..", "CRM.api"),              // sibling project
            Path.Combine(Directory.GetCurrentDirectory(), "..", "CRM.winForms"),         // sibling project
            Path.Combine(Directory.GetCurrentDirectory(), "..", "CRM.infrastructure"),   // sibling project
        };

        foreach (var dir in candidates)
        {
            if (string.IsNullOrWhiteSpace(dir)) continue;

            string full;
            try { full = Path.GetFullPath(dir); }
            catch { continue; }

            var candidate = Path.Combine(full, "appsettings.json");
            if (File.Exists(candidate))
            {
                return new ConfigurationBuilder()
                    .SetBasePath(full)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();
            }
        }

        throw new FileNotFoundException(
            "Could not locate appsettings.json. Searched: " +
            string.Join(" | ", candidates));
    }
}