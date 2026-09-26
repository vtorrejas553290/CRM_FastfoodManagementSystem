using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class ActivityLogController : IActivityLogController
{
    public List<ActivityLogRow> GetActivityLogs(ActivityLogFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var action = filter.Action ?? "All";
        var from = filter.FromDate;
        var to = filter.ToDate;

        var query = db.ActivityLogs.AsNoTracking().AsQueryable();

        if (action != "All")
            query = query.Where(x => x.ActionType == action);

        if (from != null)
            query = query.Where(x => x.PerformedAt >= from);
        if (to != null)
            query = query.Where(x => x.PerformedAt <= to);

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                (x.Username != null && x.Username.ToLower().Contains(search)) ||
                x.EntityName.ToLower().Contains(search) ||
                (x.Description != null && x.Description.ToLower().Contains(search)))
            .OrderByDescending(x => x.PerformedAt)
            .Take(1000)
            .Select(x => new ActivityLogRow
            {
                ActivityLogId = x.ActivityLogId,
                PerformedAt = x.PerformedAt,
                Username = x.Username ?? "",
                RoleCode = x.RoleCode ?? "",
                ActionType = x.ActionType ?? "",
                EntityName = x.EntityName ?? "",
                EntityId = x.EntityId,
                Description = x.Description ?? ""
            })
            .ToList();
    }
}