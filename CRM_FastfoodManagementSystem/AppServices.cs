using CRM.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.winForms;

public static class AppServices
{
    public static IConfigurationRoot Configuration { get; private set; } = null!;

    public static void Initialize()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static MasterCrmDbContext CreateMasterContext()
    {
        var cs = Configuration.GetConnectionString("MasterCrm")
            ?? throw new InvalidOperationException("MasterCrm connection string missing.");

        var options = new DbContextOptionsBuilder<MasterCrmDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new MasterCrmDbContext(options);
    }

    public static TenantCrmDbContext CreateTenantContext()
    {
        var cs = Configuration.GetConnectionString("TenantCrm")
            ?? throw new InvalidOperationException("TenantCrm connection string missing.");

        var options = new DbContextOptionsBuilder<TenantCrmDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new TenantCrmDbContext(options);
    }

    /// <summary>
    /// Returns the latest active SuperAdmin T&C that the given user has NOT accepted yet.
    /// Returns null if the user has already accepted the latest.
    /// </summary>
    public static CRM.domain.Entities.TermsAndCondition? GetUnacceptedSuperAdminTerms(int userId)
    {
        using var db = CreateTenantContext();

        // 1. Find the latest active SuperAdmin T&C
        var latest = db.TermsAndConditions
            .AsNoTracking()
            .Where(x => x.IsActive && x.CreatedByRoleCode == "SUPERADMIN")
            .OrderByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (latest is null) return null;

        // 2. Has this user already accepted THIS version?
        bool alreadyAccepted = db.UserTermsAcceptances
            .AsNoTracking()
            .Any(x => x.TermsAndConditionId == latest.TermsAndConditionId
                   && x.UserId == userId);

        return alreadyAccepted ? null : latest;
    }
}