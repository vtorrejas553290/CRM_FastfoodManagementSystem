using System.Linq;
using CRM.domain.Entities;
using CRM.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CRM.infrastructure;

namespace CRM.infrastructure;

public static class AppServices
{
    public static IConfigurationRoot Configuration { get; private set; } = null!;

    /// <summary>
    /// Ambient tenant context. Set by FrmLogin after successful tenant login.
    /// The no-arg CreateTenantContext() uses this to route to the correct
    /// tenant server. SuperAdmin leaves this at 0 (no tenant context).
    /// </summary>
    public static int CurrentCompanyId { get; set; } = 0;

    // ---- NEW — ambient branch context (Wave 3: branch filtering) ----

    /// <summary>
    /// The BranchId of the currently logged-in user, or null if none.
    /// Set by FrmLogin after successful tenant login.
    /// Controllers use this to filter lists by branch when the user is
    /// not an admin.
    /// </summary>
    public static int? CurrentBranchId { get; set; } = null;

    /// <summary>
    /// True when the currently logged-in user is an Admin or SuperAdmin.
    /// Admins see everything, so controllers skip the branch filter for them.
    /// </summary>
    public static bool CurrentUserIsAdmin { get; set; } = false;

    public static void Initialize()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile(
                Path.Combine(AppContext.BaseDirectory, "appsettings.json"),
                optional: false,
                reloadOnChange: true)
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

    /// <summary>
    /// Returns a TenantCrmDbContext routed to the currently logged-in tenant.
    /// The tenant is determined by <see cref="CurrentCompanyId"/>, which is
    /// set by FrmLogin after successful authentication.
    ///
    /// This overload preserves backward compatibility with every existing
    /// call site in the codebase — they continue to call CreateTenantContext()
    /// with no arguments, but now get the correct per-tenant context.
    /// </summary>
    public static TenantCrmDbContext CreateTenantContext()
    {
        if (CurrentCompanyId <= 0)
            throw new InvalidOperationException(
                "No tenant context is active. Either the user has not logged in " +
                "as a tenant user, or CurrentCompanyId was not set. " +
                "(SuperAdmin has no tenant context — do not call tenant methods " +
                "while logged in as SuperAdmin.)");

        return CreateTenantContext(CurrentCompanyId);
    }

    /// <summary>
    /// Builds a tenant context for the given company using the routing info in
    /// DB_MasterCRM.dbo.CompanyDatabases. This is how silo routing works: the
    /// company's row tells us which server + database hold its tenant data.
    /// </summary>
    public static TenantCrmDbContext CreateTenantContext(int companyId)
    {
        if (companyId <= 0)
            throw new ArgumentOutOfRangeException(nameof(companyId),
                "companyId must be a positive integer.");

        // 1. Look up the routing row from the master DB.
        CompanyDatabase? routing;
        using (var master = CreateMasterContext())
        {
            routing = master.CompanyDatabases
                .AsNoTracking()
                .FirstOrDefault(x => x.CompanyId == companyId && x.IsActive);
        }

        if (routing is null)
            throw new InvalidOperationException(
                $"No active CompanyDatabase routing row found for companyId={companyId}. " +
                "Check DB_MasterCRM.dbo.CompanyDatabases.");

        // 2. Build the tenant connection string from ServerName + DatabaseName.
        //    LocalDB uses Windows Authentication (no password), so we use
        //    Trusted_Connection=True. If we later move to SQL Express with
        //    per-tenant logins, we read the password from
        //    Configuration["TenantCredentials:" + routing.CredentialKey].
        var cs =
            $"Server={routing.ServerName};" +
            $"Database={routing.DatabaseName};" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;" +
            "MultipleActiveResultSets=True;";

        var options = new DbContextOptionsBuilder<TenantCrmDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new TenantCrmDbContext(options);
    }

    public static CRM.domain.Entities.TermsAndCondition? GetUnacceptedSuperAdminTerms(int userId)
    {
        using var db = CreateTenantContext();

        var latest = db.TermsAndConditions
            .AsNoTracking()
            .Where(x => x.IsActive && x.CreatedByRoleCode == "SUPERADMIN")
            .OrderByDescending(x => x.EffectiveFrom)
            .FirstOrDefault();

        if (latest is null) return null;

        bool alreadyAccepted = db.UserTermsAcceptances
            .AsNoTracking()
            .Any(x => x.TermsAndConditionId == latest.TermsAndConditionId
                   && x.UserId == userId);

        return alreadyAccepted ? null : latest;
    }
}