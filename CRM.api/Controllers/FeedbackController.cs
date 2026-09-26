using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class FeedbackController : IFeedbackController
{
    public List<FeedbackRow> GetFeedback(FeedbackFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var status = filter.Status ?? "All";
        var starsText = filter.StarsText ?? "All";

        var query = db.CustomerFeedbacks
            .Include(x => x.Customer)
            .AsNoTracking();

        if (status != "All")
            query = query.Where(x => x.Status == status);

        if (starsText != "All" && starsText.Length > 0)
        {
            var firstChar = starsText[0];
            if (char.IsDigit(firstChar))
            {
                int stars = firstChar - '0';
                query = query.Where(x => x.Rating == stars);
            }
        }

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                (x.Customer != null && x.Customer.CustomerName.ToLower().Contains(search)) ||
                (x.Comments != null && x.Comments.ToLower().Contains(search)))
            .OrderByDescending(x => x.SubmittedAt)
            .Select(x => new FeedbackRow
            {
                CustomerFeedbackId = x.CustomerFeedbackId,
                Customer = x.Customer != null ? x.Customer.CustomerName : "(deleted)",
                Rating = x.Rating,
                Category = x.Category ?? "",
                Comments = x.Comments ?? "",
                Status = x.Status,
                SubmittedAt = x.SubmittedAt
            })
            .ToList();
    }
}