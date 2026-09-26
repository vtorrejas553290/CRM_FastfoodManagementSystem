using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class PromotionController : IPromotionController
{
    public List<PromotionRow> GetPromotions(PromotionFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var showInactive = filter.ShowInactive;
        var now = filter.Now;

        var query = db.Promotions.AsNoTracking();

        // Show only the relevant set: archived when toggled, active otherwise
        if (showInactive)
            query = query.Where(x => !x.IsActive);
        else
            query = query.Where(x => x.IsActive);

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.PromotionName.ToLower().Contains(search) ||
                x.PromotionCode.ToLower().Contains(search))
            .OrderByDescending(x => x.CreatedAt)
            .ToList()
            .Select(x => new PromotionRow
            {
                PromotionId = x.PromotionId,
                PromotionCode = x.PromotionCode,
                PromotionName = x.PromotionName,
                Discount = x.DiscountType == "Percent"
                    ? $"{x.DiscountValue:N0}%"
                    : $"₱{x.DiscountValue:N2}",
                MinPurchase = x.MinimumPurchase.HasValue
                    ? $"₱{x.MinimumPurchase:N2}"
                    : "-",
                Validity = $"{x.StartDate:yyyy-MM-dd} → {x.EndDate:yyyy-MM-dd}",
                State = !x.IsActive ? "Archived"
                      : (now < x.StartDate ? "Upcoming"
                      : (now > x.EndDate ? "Expired" : "Active"))
            })
            .ToList();
    }
}