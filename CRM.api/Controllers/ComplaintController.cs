using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class ComplaintController : IComplaintController
{
    public List<ComplaintRow> GetComplaints(ComplaintFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var category = filter.Category ?? "All";
        var status = filter.Status ?? "All";
        var showArchived = filter.ShowArchived;

        var query = db.Complaints
            .Include(x => x.Customer)
            .Include(x => x.AssignedToUser)
            .AsNoTracking()
            .AsQueryable();

        // Archived filter
        query = showArchived
            ? query.Where(x => x.IsArchived)
            : query.Where(x => !x.IsArchived);

        if (category != "All")
            query = query.Where(x => x.Category == category);
        if (status != "All")
            query = query.Where(x => x.Status == status);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.ComplaintCode.ToLower().Contains(search) ||
                x.Subject.ToLower().Contains(search) ||
                (x.Customer != null && x.Customer.CustomerName.ToLower().Contains(search)));
        }

        return query
            .OrderByDescending(x => x.SubmittedAt)
            .Select(x => new ComplaintRow
            {
                ComplaintId = x.ComplaintId,
                ComplaintCode = x.ComplaintCode,
                Category = x.Category,
                Severity = x.Severity,
                Subject = x.Subject,
                CustomerName = x.Customer != null ? x.Customer.CustomerName : "(deleted)",
                Status = x.Status,
                AssignedTo = x.AssignedToUser != null ? x.AssignedToUser.FullName : "—",
                SubmittedAt = x.SubmittedAt,
                IsArchived = x.IsArchived
            })
            .ToList();
    }
}