using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class RetentionController : IRetentionController
{
    private const int AtRiskDays = 30;
    private const int DormantDays = 60;

    public List<RetentionRow> GetRetention(RetentionFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var statusFilter = filter.Status ?? "All";
        var now = filter.Now;
        var cutoffAtRisk = now.AddDays(-AtRiskDays);
        var cutoffDormant = now.AddDays(-DormantDays);

        var raw = db.Customers
            .AsNoTracking()
            .Where(c => c.IsActive)
            .Where(c => string.IsNullOrWhiteSpace(search) ||
                        c.CustomerCode.ToLower().Contains(search) ||
                        c.CustomerName.ToLower().Contains(search))
            .Select(c => new
            {
                c.CustomerId,
                c.CustomerCode,
                c.CustomerName,
                c.ContactNumber,
                c.CurrentPoints,
                LastOrderAt = c.Orders
                    .Where(o => o.Status != "Cancelled")
                    .Max(o => (DateTime?)o.OrderDate),
                TotalOrders = c.Orders.Count(o => o.Status != "Cancelled")
            })
            .ToList();

        var rows = raw.Select(x =>
        {
            int? days = x.LastOrderAt.HasValue
                ? (int)(now - x.LastOrderAt.Value).TotalDays
                : (int?)null;

            string status;
            if (x.LastOrderAt == null) status = "Never";
            else if (x.LastOrderAt < cutoffDormant) status = "Dormant";
            else if (x.LastOrderAt < cutoffAtRisk) status = "At Risk";
            else status = "Active";

            return new RetentionRow
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                ContactNumber = x.ContactNumber ?? "-",
                CurrentPoints = x.CurrentPoints,
                PointsValue = x.CurrentPoints / 100m,
                LastOrderAt = x.LastOrderAt,
                DaysSince = days,
                TotalOrders = x.TotalOrders,
                Retention = status
            };
        }).ToList();

        if (statusFilter != "All")
            rows = rows.Where(r => r.Retention == statusFilter).ToList();

        return rows;
    }
}