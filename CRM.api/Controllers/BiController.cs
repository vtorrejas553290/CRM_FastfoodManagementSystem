using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using CRM.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class BiController : IBiController
{
    public BiSnapshot GetSnapshot(BiFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var now = filter.Now;
        var from = filter.FromDate;
        var to = filter.ToDate;

        var s = new BiSnapshot();

        // ================= KPI Row 1 =================
        s.TotalRevenue = db.Transactions
            .Where(t => (from == null || t.PaidAt >= from)
                     && (to == null || t.PaidAt <= to))
            .Select(t => (decimal?)t.AmountPaid).Sum() ?? 0m;

        s.TotalTransactions = db.Transactions
            .Count(t => (from == null || t.PaidAt >= from)
                     && (to == null || t.PaidAt <= to));

        // Today cards are literal today, not period-scoped
        s.TodaySales = db.Transactions
            .Where(t => t.PaidAt.Date == now.Date)
            .Select(t => (decimal?)t.AmountPaid).Sum() ?? 0m;

        s.TodayOrders = db.Orders.Count(o => o.OrderDate.Date == now.Date);

        s.TotalOrders = db.Orders
            .Count(o => (from == null || o.OrderDate >= from)
                     && (to == null || o.OrderDate <= to));

        decimal totalOrderValue = db.Orders
            .Where(o => (from == null || o.OrderDate >= from)
                     && (to == null || o.OrderDate <= to))
            .Select(o => (decimal?)o.TotalAmount).Sum() ?? 0m;

        s.AverageOrderValue = s.TotalOrders > 0 ? totalOrderValue / s.TotalOrders : 0m;

        s.ActiveCustomers = db.Customers.Count(c => c.IsActive);
        s.NewCustomersLast30Days = db.Customers.Count(c => c.CreatedAt >= now.AddDays(-30));
        s.TotalPoints = db.Customers.Where(c => c.IsActive)
            .Sum(c => (int?)c.CurrentPoints) ?? 0;

        // ================= KPI Row 2 =================
        s.LowStockCount = db.Inventories.Count(i => i.QuantityOnHand <= i.ReorderLevel);
        s.OpenFeedbackCount = db.CustomerFeedbacks.Count(f => f.Status == "New");

        var feedbackForPeriod = db.CustomerFeedbacks
            .Where(f => (from == null || f.SubmittedAt >= from)
                     && (to == null || f.SubmittedAt <= to));

        s.AverageRating = feedbackForPeriod.Any()
            ? feedbackForPeriod.Average(f => (double)f.Rating)
            : 0;
        s.TotalFeedbackCount = feedbackForPeriod.Count();

        s.ActivePromotionsCount = db.Promotions.Count(p =>
            p.IsActive && p.StartDate <= now && p.EndDate >= now);

        // ================= KPI Row 3: retention =================
        var cutoffAtRisk = now.AddDays(-30);
        var cutoffDormant = now.AddDays(-60);

        var customerStats = db.Customers
            .AsNoTracking()
            .Where(c => c.IsActive)
            .Select(c => new
            {
                c.CustomerId,
                c.CustomerName,
                c.CurrentPoints,
                LastOrderAt = c.Orders
                    .Where(o => o.Status != "Cancelled")
                    .Max(o => (DateTime?)o.OrderDate),
                TotalOrders = c.Orders.Count(o => o.Status != "Cancelled")
            })
            .ToList();

        int cntActive = 0, cntAtRisk = 0, cntDormant = 0, cntNever = 0;

        var atRiskBuffer = new List<(string Name, int Days, int Points, string Status)>();

        foreach (var c in customerStats)
        {
            string status;
            int days = 0;

            if (c.LastOrderAt == null)
            {
                status = "Never";
                cntNever++;
            }
            else
            {
                days = (int)(now - c.LastOrderAt.Value).TotalDays;
                if (c.LastOrderAt < cutoffDormant) { status = "Dormant"; cntDormant++; }
                else if (c.LastOrderAt < cutoffAtRisk) { status = "At Risk"; cntAtRisk++; }
                else { status = "Active"; cntActive++; }
            }

            if (status == "At Risk" || status == "Dormant")
                atRiskBuffer.Add((c.CustomerName, days, c.CurrentPoints, status));
        }

        s.ActiveForRetentionCount = cntActive;
        s.AtRiskCount = cntAtRisk;
        s.DormantCount = cntDormant;
        s.NeverOrderedCount = cntNever;
        s.PointsLiability = s.TotalPoints / 100m;

        s.RetentionSplit = new List<BiRetentionSplit>
        {
            new() { Status = "Active",   Count = cntActive },
            new() { Status = "At Risk",  Count = cntAtRisk },
            new() { Status = "Dormant",  Count = cntDormant },
            new() { Status = "Never",    Count = cntNever }
        };

        s.RetentionRecency = new int[6];
        foreach (var c in customerStats)
        {
            if (c.LastOrderAt == null) continue;
            int d = (int)(now - c.LastOrderAt.Value).TotalDays;
            if (d <= 7) s.RetentionRecency[0]++;
            else if (d <= 30) s.RetentionRecency[1]++;
            else if (d <= 60) s.RetentionRecency[2]++;
            else if (d <= 90) s.RetentionRecency[3]++;
            else if (d <= 180) s.RetentionRecency[4]++;
            else s.RetentionRecency[5]++;
        }

        s.AtRiskList = atRiskBuffer
            .OrderByDescending(x => x.Days)
            .Take(15)
            .Select(x => new BiRow
            {
                Cells =
                {
                    ["Customer"] = x.Name,
                    ["DaysSince"] = x.Days,
                    ["Points"] = x.Points,
                    ["Status"] = x.Status
                }
            })
            .ToList();

        // ================= Charts =================
        // Sales chart always shows the last 7 calendar days, independent of period.
        s.SalesLast7 = Enumerable.Range(0, 7)
            .Select(i => now.Date.AddDays(-6 + i))
            .Select(d => new BiSalesDay
            {
                Day = d,
                Total = db.Transactions
                    .Where(t => t.PaidAt.Date == d)
                    .Select(t => (decimal?)t.AmountPaid).Sum() ?? 0m
            })
            .ToList();

        s.TopProducts = db.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => (from == null || oi.Order!.OrderDate >= from)
                      && (to == null || oi.Order!.OrderDate <= to))
            .GroupBy(oi => oi.Product!.ProductName)
            .Select(g => new { Name = g.Key, Qty = g.Sum(oi => oi.Quantity) })
            .OrderByDescending(x => x.Qty)
            .Take(5)
            .ToList()
            .Select(x => new BiTopProduct { Name = x.Name, Qty = x.Qty })
            .ToList();

        s.RatingDistribution = new int[5];
        foreach (var fb in db.CustomerFeedbacks
            .Where(f => (from == null || f.SubmittedAt >= from)
                     && (to == null || f.SubmittedAt <= to)))
        {
            int r = Math.Clamp(fb.Rating, 1, 5);
            s.RatingDistribution[r - 1]++;
        }

        s.PaymentSplit = db.Transactions
            .Where(t => (from == null || t.PaidAt >= from)
                     && (to == null || t.PaidAt <= to))
            .GroupBy(t => t.PaymentMethod)
            .Select(g => new { Method = g.Key, Count = g.Count() })
            .ToList()
            .Select(x => new BiPaymentSplit { Method = x.Method, Count = x.Count })
            .ToList();

        // ================= Tables =================
        // Recent Orders always shows the 10 most recent, regardless of period.
        s.RecentOrders = db.Orders
            .Include(o => o.Customer)
            .OrderByDescending(o => o.OrderDate)
            .Take(10)
            .ToList()
            .Select(o => new BiRow
            {
                Cells =
                {
                    ["OrderCode"] = o.OrderCode,
                    ["Customer"] = o.Customer != null ? o.Customer.CustomerName : "(deleted)",
                    ["TotalAmount"] = o.TotalAmount,
                    ["Status"] = o.Status,
                    ["OrderDate"] = o.OrderDate
                }
            })
            .ToList();

        s.TopCustomers = db.Orders
            .Include(o => o.Customer)
            .Where(o => (from == null || o.OrderDate >= from)
                     && (to == null || o.OrderDate <= to))
            .ToList()
            .GroupBy(o => new
            {
                o.CustomerId,
                Name = o.Customer != null ? o.Customer.CustomerName : "(deleted)"
            })
            .Select(g => new
            {
                Customer = g.Key.Name,
                Orders = g.Count(),
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(10)
            .ToList()
            .Select(x => new BiRow
            {
                Cells =
                {
                    ["Customer"] = x.Customer,
                    ["Orders"] = x.Orders,
                    ["TotalSpent"] = x.TotalSpent
                }
            })
            .ToList();

        // ================= Insights =================
        BuildInsights(db, s, now, from, to);
        BuildRetentionInsights(s);

        return s;
    }

    private static void BuildInsights(
        TenantCrmDbContext db, BiSnapshot s, DateTime now,
        DateTime? from, DateTime? to)
    {
        var list = new List<BiInsight>();

        decimal thisWeek = db.Transactions
            .Where(t => t.PaidAt >= now.Date.AddDays(-6))
            .Select(t => (decimal?)t.AmountPaid).Sum() ?? 0m;

        decimal lastWeek = db.Transactions
            .Where(t => t.PaidAt >= now.Date.AddDays(-13) && t.PaidAt < now.Date.AddDays(-6))
            .Select(t => (decimal?)t.AmountPaid).Sum() ?? 0m;

        if (lastWeek > 0)
        {
            double pct = (double)((thisWeek - lastWeek) / lastWeek * 100);
            if (pct >= 5)
                list.Add(new BiInsight
                {
                    Icon = "📈",
                    Semantic = "success",
                    Text = $"Sales are trending UP — ₱{thisWeek:N0} this week vs ₱{lastWeek:N0} last week ({pct:N0}% increase)."
                });
            else if (pct <= -5)
                list.Add(new BiInsight
                {
                    Icon = "📉",
                    Semantic = "danger",
                    Text = $"Sales are DOWN — ₱{thisWeek:N0} this week vs ₱{lastWeek:N0} last week ({Math.Abs(pct):N0}% decrease). Consider a promotion."
                });
            else
                list.Add(new BiInsight
                {
                    Icon = "➖",
                    Semantic = "muted",
                    Text = $"Sales are steady — ₱{thisWeek:N0} this week vs ₱{lastWeek:N0} last week."
                });
        }
        else if (thisWeek > 0)
        {
            list.Add(new BiInsight
            {
                Icon = "📈",
                Semantic = "success",
                Text = $"Sales this week: ₱{thisWeek:N0}. No prior week to compare."
            });
        }

        if (s.TopProducts.Count > 0)
        {
            var top = s.TopProducts[0];
            list.Add(new BiInsight
            {
                Icon = "🏆",
                Semantic = "warning",
                Text = $"Best performer: '{top.Name}' — {top.Qty:N0} sold."
            });
        }

        if (s.AverageRating > 0)
        {
            int fiveStar = s.RatingDistribution.Length > 4 ? s.RatingDistribution[4] : 0;
            int totalRatings = s.RatingDistribution.Sum();
            double fiveStarPct = totalRatings > 0 ? fiveStar * 100.0 / totalRatings : 0;

            if (s.AverageRating >= 4.5)
                list.Add(new BiInsight
                {
                    Icon = "⭐",
                    Semantic = "success",
                    Text = $"Customer satisfaction is excellent — average {s.AverageRating:0.0}★ ({fiveStarPct:N0}% are 5-star)."
                });
            else if (s.AverageRating >= 3.5)
                list.Add(new BiInsight
                {
                    Icon = "⭐",
                    Semantic = "warning",
                    Text = $"Customer satisfaction is decent — average {s.AverageRating:0.0}★. {s.OpenFeedbackCount} reviews still unreviewed."
                });
            else
                list.Add(new BiInsight
                {
                    Icon = "⚠️",
                    Semantic = "danger",
                    Text = $"Customer satisfaction is LOW — average {s.AverageRating:0.0}★. Investigate recent complaints."
                });
        }

        if (s.LowStockCount > 0)
        {
            var items = db.Inventories
                .Include(i => i.Product)
                .Where(i => i.QuantityOnHand <= i.ReorderLevel)
                .Take(3)
                .ToList()
                .Select(i => $"{i.Product?.ProductName ?? "?"} ({i.QuantityOnHand:N0})")
                .ToList();

            list.Add(new BiInsight
            {
                Icon = "📦",
                Semantic = "danger",
                Text = $"{s.LowStockCount} product(s) need restocking: {string.Join(", ", items)}."
            });
        }

        var customerOrderCounts = db.Orders
            .Where(o => (from == null || o.OrderDate >= from)
                     && (to == null || o.OrderDate <= to))
            .GroupBy(o => o.CustomerId)
            .Select(g => new { Count = g.Count(), Spent = g.Sum(o => o.TotalAmount) })
            .ToList();

        int repeat = customerOrderCounts.Count(x => x.Count > 1);
        int once = customerOrderCounts.Count(x => x.Count == 1);

        if (repeat > 0 && once > 0)
        {
            decimal avgRepeat = customerOrderCounts.Where(x => x.Count > 1).Average(x => x.Spent);
            decimal avgOnce = customerOrderCounts.Where(x => x.Count == 1).Average(x => x.Spent);
            double ratio = avgOnce > 0 ? (double)(avgRepeat / avgOnce) : 0;

            list.Add(new BiInsight
            {
                Icon = "🔄",
                Semantic = "info",
                Text = $"{repeat} repeat customers ({repeat * 100 / (repeat + once)}% of base). Their average spend is ₱{avgRepeat:N0} — {ratio:0.0}x higher than one-time buyers."
            });
        }

        var promoStats = db.PromotionRedemptions
            .GroupBy(r => r.PromotionId)
            .Select(g => new { PromoId = g.Key, Uses = g.Count(), Total = g.Sum(r => r.DiscountApplied) })
            .OrderByDescending(x => x.Uses)
            .Take(1)
            .ToList();

        if (promoStats.Any())
        {
            var top = promoStats[0];
            var promo = db.Promotions.FirstOrDefault(p => p.PromotionId == top.PromoId);
            if (promo != null)
                list.Add(new BiInsight
                {
                    Icon = "🎁",
                    Semantic = "info",
                    Text = $"Top promo: '{promo.PromotionCode}' used {top.Uses} times, giving ₱{top.Total:N0} in discounts."
                });
        }

        int lowRatingCount = db.CustomerFeedbacks.Count(f => f.Rating <= 2);
        if (lowRatingCount > 0)
            list.Add(new BiInsight
            {
                Icon = "⚠️",
                Semantic = "warning",
                Text = $"{lowRatingCount} customer(s) gave low ratings (≤2★). Consider creating retention offers to win them back."
            });

        if (s.PaymentSplit.Count >= 2)
        {
            var top = s.PaymentSplit.OrderByDescending(x => x.Count).First();
            int total = s.PaymentSplit.Sum(x => x.Count);
            int pct = total > 0 ? top.Count * 100 / total : 0;
            list.Add(new BiInsight
            {
                Icon = "💳",
                Semantic = "muted",
                Text = $"Payment preference: '{top.Method}' is dominant at {pct}% of transactions."
            });
        }

        if (s.TotalPoints > 0)
        {
            int withPoints = db.Customers.Count(c => c.CurrentPoints > 0);
            list.Add(new BiInsight
            {
                Icon = "🎯",
                Semantic = "success",
                Text = $"{withPoints} customer(s) currently hold {s.TotalPoints:N0} loyalty points (≈ ₱{s.TotalPoints / 100m:N0} liability)."
            });
        }

        var lastSaleDate = db.Transactions
            .OrderByDescending(t => t.PaidAt)
            .Select(t => (DateTime?)t.PaidAt)
            .FirstOrDefault();

        if (lastSaleDate.HasValue)
        {
            int daysSince = (int)(now - lastSaleDate.Value).TotalDays;
            if (daysSince >= 3)
                list.Add(new BiInsight
                {
                    Icon = "💤",
                    Semantic = "danger",
                    Text = $"No sales recorded in {daysSince} days. Check system or staff activity."
                });
        }

        if (list.Count == 0)
            list.Add(new BiInsight
            {
                Icon = "ℹ️",
                Semantic = "muted",
                Text = "Not enough data yet to generate insights. Start recording orders to see recommendations."
            });

        s.Insights = list;
    }

    private static void BuildRetentionInsights(BiSnapshot s)
    {
        int total = s.ActiveForRetentionCount + s.AtRiskCount + s.DormantCount + s.NeverOrderedCount;
        if (total == 0) return;

        int atRiskPct = s.AtRiskCount * 100 / total;
        int dormantPct = s.DormantCount * 100 / total;

        if (s.DormantCount > 0)
            s.Insights.Add(new BiInsight
            {
                Icon = "🚨",
                Semantic = "danger",
                Text = $"{s.DormantCount} customer(s) are DORMANT ({dormantPct}% of base). Reach out with a win-back promotion."
            });

        if (s.AtRiskCount > 0)
            s.Insights.Add(new BiInsight
            {
                Icon = "⚠️",
                Semantic = "warning",
                Text = $"{s.AtRiskCount} customer(s) are AT RISK ({atRiskPct}% of base). They ordered 30–60 days ago — a reminder could bring them back."
            });

        if (s.NeverOrderedCount > 0)
            s.Insights.Add(new BiInsight
            {
                Icon = "👤",
                Semantic = "muted",
                Text = $"{s.NeverOrderedCount} customer(s) signed up but NEVER ordered. Consider a first-purchase incentive."
            });

        if (s.ActiveForRetentionCount > 0 && s.DormantCount + s.AtRiskCount > s.ActiveForRetentionCount)
            s.Insights.Add(new BiInsight
            {
                Icon = "📊",
                Semantic = "danger",
                Text = $"Retention concern: {s.AtRiskCount + s.DormantCount} customers are slipping away vs {s.ActiveForRetentionCount} still active."
            });

        if (s.PointsLiability > 500)
            s.Insights.Add(new BiInsight
            {
                Icon = "💰",
                Semantic = "warning",
                Text = $"Outstanding loyalty liability: ₱{s.PointsLiability:N0} ({s.TotalPoints:N0} points). Consider a redemption campaign to reduce liability."
            });
    }
}