using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using CRM.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class UserController : IUserController
{
    public List<UserRow> GetUsers(UserFilter filter)
    {
        if (filter.ShowAllTenants)
            return GetUsersAcrossTenants(filter);

        return GetUsersFromSingleTenant(filter);
    }

    // ================================================================
    // SuperAdmin path — loop over every active tenant in master DB
    // ================================================================
    private List<UserRow> GetUsersAcrossTenants(UserFilter filter)
    {
        var result = new List<UserRow>();
        var search = (filter.Search ?? "").Trim().ToLower();

        List<(int CompanyId, string CompanyName, string ServerName, string DbName)> tenants;

        using (var master = AppServices.CreateMasterContext())
        {
            tenants = (
                from c in master.Companies
                join d in master.CompanyDatabases on c.CompanyId equals d.CompanyId
                where c.IsActive && d.IsActive
                orderby c.CompanyId
                select new
                {
                    c.CompanyId,
                    c.CompanyName,
                    d.ServerName,
                    d.DatabaseName
                })
                .AsNoTracking()
                .ToList()
                .Select(x => (x.CompanyId, x.CompanyName, x.ServerName, x.DatabaseName))
                .ToList();
        }

        foreach (var t in tenants)
        {
            try
            {
                var cs = $"Server={t.ServerName};Database={t.DbName};Trusted_Connection=True;" +
                         "TrustServerCertificate=True;MultipleActiveResultSets=True;";

                var options = new DbContextOptionsBuilder<TenantCrmDbContext>()
                    .UseSqlServer(cs)
                    .Options;

                using var db = new TenantCrmDbContext(options);

                var adminRows = db.Users
                    .Include(x => x.Role)
                    .AsNoTracking()
                    .Where(x => x.Role != null && x.Role.RoleCode == "ADMIN")
                    .Where(x =>
                        string.IsNullOrWhiteSpace(search) ||
                        x.Username.ToLower().Contains(search) ||
                        x.FullName.ToLower().Contains(search))
                    .Select(x => new UserRow
                    {
                        UserId = x.UserId,
                        Username = x.Username,
                        FullName = x.FullName,
                        Email = x.Email ?? "",
                        Role = x.Role!.RoleName,
                        Status = x.IsActive ? "Active" : "Archived",
                        CreatedAt = x.CreatedAt,
                        CompanyId = t.CompanyId,
                        TenantName = t.CompanyName
                    })
                    .ToList();

                result.AddRange(adminRows);
            }
            catch
            {
                // Skip a tenant that can't be reached.
            }
        }

        return result
            .OrderBy(x => x.CompanyId)
            .ThenBy(x => x.UserId)
            .ToList();
    }

    // ================================================================
    // Tenant admin path — single tenant DB via filter.CompanyId
    // ================================================================
    private List<UserRow> GetUsersFromSingleTenant(UserFilter filter)
    {
        using var db = AppServices.CreateTenantContext(filter.CompanyId);

        var search = (filter.Search ?? "").Trim().ToLower();
        var showArchived = filter.ShowArchived;
        var roleFilter = (filter.RoleCodeFilter ?? "All").Trim();

        var query = db.Users.Include(x => x.Role).AsNoTracking();

        if (filter.IsAdmin)
            query = query.Where(x =>
                x.Role!.RoleCode == "MANAGER" ||
                x.Role!.RoleCode == "STAFF");
        else
            return new List<UserRow>();

        // Apply role filter (ignored when "All")
        if (roleFilter != "All" &&
            (roleFilter == "MANAGER" || roleFilter == "STAFF"))
        {
            query = query.Where(x => x.Role!.RoleCode == roleFilter);
        }

        // NEW — branch filter
        // null   → no filter
        // > 0    → users assigned to that branch
        // -1     → users with no branch (Unassigned)
        if (filter.BranchIdFilter.HasValue)
        {
            if (filter.BranchIdFilter.Value == -1)
                query = query.Where(x => x.BranchId == null);
            else
                query = query.Where(x => x.BranchId == filter.BranchIdFilter.Value);
        }

        if (showArchived)
            query = query.Where(x => !x.IsActive);
        else
            query = query.Where(x => x.IsActive);

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.Username.ToLower().Contains(search) ||
                x.FullName.ToLower().Contains(search))
            .OrderBy(x => x.UserId)
            .Select(x => new UserRow
            {
                UserId = x.UserId,
                Username = x.Username,
                FullName = x.FullName,
                Email = x.Email ?? "",
                Role = x.Role!.RoleName,
                Status = x.IsActive ? "Active" : "Archived",
                CreatedAt = x.CreatedAt
            })
            .ToList();
    }
}