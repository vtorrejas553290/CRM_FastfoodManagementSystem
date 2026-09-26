using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class UserController : IUserController
{
    public List<UserRow> GetUsers(UserFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var showArchived = filter.ShowArchived;

        var query = db.Users.Include(x => x.Role).AsNoTracking();

        if (filter.IsSuperAdmin)
            query = query.Where(x => x.Role!.RoleCode == "ADMIN" || x.UserId == filter.CurrentUserId);
        else if (filter.IsAdmin)
            query = query.Where(x => x.Role!.RoleCode == "MANAGER" || x.Role!.RoleCode == "STAFF");
        else
            return new List<UserRow>();

        // Show only the relevant set: archived when toggled, active otherwise
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