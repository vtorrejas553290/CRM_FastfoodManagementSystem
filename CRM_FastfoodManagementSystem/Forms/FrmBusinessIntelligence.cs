using CRM.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace CRM.winForms.Forms;

public partial class FrmBusinessIntelligence : Form
{
    public event Action<string>? NavigateRequested;

    public FrmBusinessIntelligence()
    {
        InitializeComponent();
        ApplyTheme();

        Load += FrmBusinessIntelligence_Load;
        btnRefresh.Click += (_, __) => LoadAll();
        cmbDateRange.SelectedIndexChanged += (_, __) => LoadAll();

        Resize += (_, __) => ResizeInsightRows();

        WireClickNavigation();
    }

    // ============================================================
    //  CLICK NAVIGATION
    // ============================================================

    private void WireClickNavigation()
    {
        MakeCardClickable(cardRevenue, "ViewTransactions", "Total Revenue");
        MakeCardClickable(cardTodaySales, "ViewTransactions", "Today's Sales");
        MakeCardClickable(cardOrders, "ViewTransactions", "Total Orders");
        MakeCardClickable(cardAvgOrder, "ViewTransactions", "Avg Order Value");
        MakeCardClickable(cardCustomers, "CustomerManagement", "Active Customers");
        MakeCardClickable(cardPoints, "CustomerRetention", "Points in Circulation");
        MakeCardClickable(cardLowStock, "InventoryManagement", "Low Stock Items");
        MakeCardClickable(cardOpenFeedback, "CustomerFeedback", "Open Feedback");
        MakeCardClickable(cardAvgRating, "CustomerFeedback", "Average Rating");
        MakeCardClickable(cardPromos, "Promotions", "Active Promotions");

        MakeCardClickable(cardAtRisk, "CustomerRetention", "At Risk Customers");
        MakeCardClickable(cardDormant, "CustomerRetention", "Dormant Customers");
        MakeCardClickable(cardNeverOrdered, "CustomerRetention", "Never Ordered");
        MakeCardClickable(cardPointsLiability, "CustomerRetention", "Points Liability");

        MakePanelClickable(pnlSalesChart, "ViewTransactions", "Sales chart");
        MakePanelClickable(pnlTopProductsChart, "ProductManagement", "Top Products chart");
        MakePanelClickable(pnlRatingChart, "CustomerFeedback", "Rating Distribution chart");
        MakePanelClickable(pnlPaymentChart, "ViewTransactions", "Payment Split chart");

        MakePanelClickable(pnlRetentionSplitChart, "CustomerRetention", "Retention Split chart");
        MakePanelClickable(pnlRetentionRecencyChart, "CustomerRetention", "Retention Recency chart");

        MakePanelClickable(pnlRecentOrders, "ViewTransactions", "Recent Orders");
        MakePanelClickable(pnlTopCustomers, "CustomerManagement", "Top Customers");

        MakePanelClickable(pnlAtRiskList, "CustomerRetention", "At-Risk Customers list");
    }

    private void MakeCardClickable(Panel card, string pageKey, string label)
    {
        card.Cursor = Cursors.Hand;
        card.Click += (_, __) => RaiseNavigation(pageKey, label);

        foreach (Control child in card.Controls)
        {
            child.Cursor = Cursors.Hand;
            child.Click += (_, __) => RaiseNavigation(pageKey, label);
        }

        card.MouseEnter += (_, __) => card.BackColor = AppTheme.Primary90;
        card.MouseLeave += (_, __) => card.BackColor = AppTheme.Surface;

        foreach (Control child in card.Controls)
        {
            child.MouseEnter += (_, __) => card.BackColor = AppTheme.Primary90;
            child.MouseLeave += (_, __) => card.BackColor = AppTheme.Surface;
        }
    }

    private void MakePanelClickable(Panel panel, string pageKey, string label)
    {
        panel.Cursor = Cursors.Hand;
        panel.Click += (_, __) => RaiseNavigation(pageKey, label);

        panel.MouseEnter += (_, __) => panel.Invalidate();
        panel.MouseLeave += (_, __) => panel.Invalidate();
    }

    private void RaiseNavigation(string pageKey, string label)
    {
        if (NavigateRequested is null) return;
        NavigateRequested.Invoke(pageKey);
    }

    // ============================================================
    //  THEME
    // ============================================================

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlInsights.BackColor = AppTheme.Surface;
        pnlRecentOrders.BackColor = AppTheme.Surface;
        pnlTopCustomers.BackColor = AppTheme.Surface;
        pnlAtRiskList.BackColor = AppTheme.Surface;

        lblHeaderTitle.Font = AppTheme.FontHeading;
        lblHeaderTitle.ForeColor = AppTheme.TextPrimary;

        lblHeaderSubtitle.ForeColor = AppTheme.TextSecondary;

        AppTheme.StyleLabel(lblDateRange);
        AppTheme.StyleInput(cmbDateRange);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleLabel(lblStatus);

        lblInsightsTitle.Font = AppTheme.FontSubheading;
        lblInsightsTitle.ForeColor = AppTheme.TextPrimary;

        lblRecentOrders.Font = AppTheme.FontSubheading;
        lblRecentOrders.ForeColor = AppTheme.TextPrimary;

        lblTopCustomers.Font = AppTheme.FontSubheading;
        lblTopCustomers.ForeColor = AppTheme.TextPrimary;

        lblAtRiskTitle.Font = AppTheme.FontSubheading;
        lblAtRiskTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleGrid(gridRecentOrders);
        AppTheme.StyleGrid(gridTopCustomers);
        AppTheme.StyleGrid(gridAtRisk);

        foreach (Control c in tblKpiRow1.Controls)
            StyleKpiCard(c);
        foreach (Control c in tblKpiRow2.Controls)
            StyleKpiCard(c);
        foreach (Control c in tblKpiRow3.Controls)
            StyleKpiCard(c);

        foreach (var pnl in new[]
        {
            pnlSalesChart, pnlTopProductsChart, pnlRatingChart, pnlPaymentChart,
            pnlRetentionSplitChart, pnlRetentionRecencyChart
        })
        {
            pnl.BackColor = AppTheme.Surface;
            pnl.Paint += ChartPanel_Paint;
        }
    }

    private void StyleKpiCard(Control card)
    {
        card.BackColor = AppTheme.Surface;

        var lblValue = card.Controls["lblValue"];
        if (lblValue is not null)
            lblValue.ForeColor = AppTheme.Primary20;

        var lblSub = card.Controls["lblSub"];
        if (lblSub is not null)
            lblSub.ForeColor = AppTheme.TextSecondary;
    }

    private void FrmBusinessIntelligence_Load(object? sender, EventArgs e)
    {
        cmbDateRange.Items.Clear();
        cmbDateRange.Items.AddRange(new object[] { "All Time", "Today", "Last 7 Days", "Last 30 Days" });
        cmbDateRange.SelectedIndex = 0;
    }

    // ============================================================
    //  LOAD ALL
    // ============================================================

    private void LoadAll()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var now = DateTime.UtcNow;
            var range = cmbDateRange.SelectedItem?.ToString() ?? "All Time";

            DateTime? from = range switch
            {
                "Today" => now.Date,
                "Last 7 Days" => now.AddDays(-7),
                "Last 30 Days" => now.AddDays(-30),
                _ => null
            };

            // ---- KPI Row 1 ----
            decimal totalRevenue = db.Transactions
                .Where(t => from == null || t.PaidAt >= from)
                .Select(t => (decimal?)t.AmountPaid)
                .Sum() ?? 0m;
            int totalTxns = db.Transactions.Count();
            SetCardValue(cardRevenue, $"₱{totalRevenue:N0}", $"{totalTxns} total transactions");

            decimal todaySales = db.Transactions
                .Where(t => t.PaidAt.Date == now.Date)
                .Select(t => (decimal?)t.AmountPaid)
                .Sum() ?? 0m;
            int todayOrders = db.Orders.Count(o => o.OrderDate.Date == now.Date);
            SetCardValue(cardTodaySales, $"₱{todaySales:N0}", $"{todayOrders} orders today");

            int totalOrders = db.Orders.Count(o => from == null || o.OrderDate >= from);
            SetCardValue(cardOrders, totalOrders.ToString("N0"), "all statuses");

            decimal totalOrderValue = db.Orders
                .Where(o => from == null || o.OrderDate >= from)
                .Select(o => (decimal?)o.TotalAmount)
                .Sum() ?? 0m;
            decimal avgOrder = totalOrders > 0 ? totalOrderValue / totalOrders : 0m;
            SetCardValue(cardAvgOrder, $"₱{avgOrder:N0}", "per order");

            int activeCustomers = db.Customers.Count(c => c.IsActive);
            int newThisMonth = db.Customers.Count(c => c.CreatedAt >= now.AddDays(-30));
            SetCardValue(cardCustomers, activeCustomers.ToString("N0"), $"+{newThisMonth} new (30d)");

            int totalPoints = db.Customers.Where(c => c.IsActive).Sum(c => (int?)c.CurrentPoints) ?? 0;
            SetCardValue(cardPoints, totalPoints.ToString("N0"), $"≈ ₱{totalPoints / 100m:N0} value");

            // ---- KPI Row 2 ----
            int lowStock = db.Inventories.Count(i => i.QuantityOnHand <= i.ReorderLevel);
            SetCardValue(cardLowStock, lowStock.ToString("N0"), "need restock");

            int openFeedback = db.CustomerFeedbacks.Count(f => f.Status == "New");
            SetCardValue(cardOpenFeedback, openFeedback.ToString("N0"), "unreviewed");

            double avgRating = db.CustomerFeedbacks.Any()
                ? db.CustomerFeedbacks.Average(f => (double)f.Rating)
                : 0;
            int totalFeedback = db.CustomerFeedbacks.Count();
            SetCardValue(cardAvgRating, avgRating.ToString("0.0") + " ★", $"{totalFeedback} ratings");

            int activePromos = db.Promotions.Count(p =>
                p.IsActive && p.StartDate <= now && p.EndDate >= now);
            SetCardValue(cardPromos, activePromos.ToString("N0"), "running now");

            // ---- KPI Row 3 (retention) ----
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

            int cntActive = 0;
            int cntAtRisk = 0;
            int cntDormant = 0;
            int cntNever = 0;

            var atRiskList = new List<(string Name, int Days, int Points, string Status)>();

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
                    atRiskList.Add((c.CustomerName, days, c.CurrentPoints, status));
            }

            SetCardValue(cardAtRisk, cntAtRisk.ToString("N0"), "missed 30-60d");
            SetCardValue(cardDormant, cntDormant.ToString("N0"), "no order in 60+d");
            SetCardValue(cardNeverOrdered, cntNever.ToString("N0"), "signed up only");

            decimal pointsLiability = totalPoints / 100m;
            SetCardValue(cardPointsLiability, $"₱{pointsLiability:N0}",
                $"{totalPoints:N0} pts outstanding");

            _retentionSplit = new List<(string Status, int Count)>
            {
                ("Active", cntActive),
                ("At Risk", cntAtRisk),
                ("Dormant", cntDormant),
                ("Never", cntNever)
            };

            _retentionRecency = new int[6];
            foreach (var c in customerStats)
            {
                if (c.LastOrderAt == null) continue;
                int d = (int)(now - c.LastOrderAt.Value).TotalDays;
                if (d <= 7) _retentionRecency[0]++;
                else if (d <= 30) _retentionRecency[1]++;
                else if (d <= 60) _retentionRecency[2]++;
                else if (d <= 90) _retentionRecency[3]++;
                else if (d <= 180) _retentionRecency[4]++;
                else _retentionRecency[5]++;
            }

            gridAtRisk.DataSource = atRiskList
                .OrderByDescending(x => x.Days)
                .Take(15)
                .Select(x => new
                {
                    Customer = x.Name,
                    DaysSince = x.Days,
                    Points = x.Points,
                    Status = x.Status
                })
                .ToList();

            // ---- Charts ----
            _salesLast7 = Enumerable.Range(0, 7)
                .Select(i => now.Date.AddDays(-6 + i))
                .Select(d => (
                    Day: d,
                    Total: db.Transactions.Where(t => t.PaidAt.Date == d).Sum(t => (decimal?)t.AmountPaid) ?? 0m
                ))
                .ToList();

            _topProducts = db.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => from == null || oi.Order!.OrderDate >= from)
                .GroupBy(oi => oi.Product!.ProductName)
                .Select(g => new { Name = g.Key, Qty = g.Sum(oi => oi.Quantity) })
                .OrderByDescending(x => x.Qty)
                .Take(5)
                .ToList()
                .Select(x => (x.Name, x.Qty))
                .ToList();

            _ratingDist = new int[5];
            foreach (var fb in db.CustomerFeedbacks.Where(f => from == null || f.SubmittedAt >= from))
            {
                int r = Math.Clamp(fb.Rating, 1, 5);
                _ratingDist[r - 1]++;
            }

            _paymentSplit = db.Transactions
                .Where(t => from == null || t.PaidAt >= from)
                .GroupBy(t => t.PaymentMethod)
                .Select(g => new { Method = g.Key, Count = g.Count() })
                .ToList()
                .Select(x => (x.Method, x.Count))
                .ToList();

            // ---- Tables ----
            gridRecentOrders.DataSource = db.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .ToList()
                .Select(o => new
                {
                    o.OrderCode,
                    Customer = o.Customer?.CustomerName ?? "(deleted)",
                    o.TotalAmount,
                    o.Status,
                    o.OrderDate
                })
                .ToList();

            gridTopCustomers.DataSource = db.Orders
                .Include(o => o.Customer)
                .Where(o => from == null || o.OrderDate >= from)
                .ToList()
                .GroupBy(o => new
                {
                    o.CustomerId,
                    Name = o.Customer?.CustomerName ?? "(deleted)"
                })
                .Select(g => new
                {
                    Customer = g.Key.Name,
                    Orders = g.Count(),
                    TotalSpent = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(10)
                .ToList();

            BuildInsights(db, now, from, totalOrders, avgOrder,
                activeCustomers, lowStock, openFeedback, avgRating,
                activePromos, totalPoints);

            BuildRetentionInsights(cntActive, cntAtRisk, cntDormant, cntNever, totalPoints);

            pnlSalesChart.Invalidate();
            pnlTopProductsChart.Invalidate();
            pnlRatingChart.Invalidate();
            pnlPaymentChart.Invalidate();
            pnlRetentionSplitChart.Invalidate();
            pnlRetentionRecencyChart.Invalidate();

            lblStatus.Text = $"Updated {DateTime.Now:HH:mm:ss}  —  Range: {range}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ============================================================
    //  RETENTION INSIGHTS (appended after the standard ones)
    // ============================================================

    private void BuildRetentionInsights(int active, int atRisk, int dormant, int never, int totalPoints)
    {
        int total = active + atRisk + dormant + never;
        if (total == 0) return;

        var insights = new List<(string Icon, Color Color, string Text)>();

        int atRiskPct = atRisk * 100 / total;
        int dormantPct = dormant * 100 / total;

        if (dormant > 0)
            insights.Add(("🚨", AppTheme.Danger,
                $"{dormant} customer(s) are DORMANT ({dormantPct}% of base). Reach out with a win-back promotion."));

        if (atRisk > 0)
            insights.Add(("⚠️", AppTheme.WarningAmber,
                $"{atRisk} customer(s) are AT RISK ({atRiskPct}% of base). They ordered 30–60 days ago — a reminder could bring them back."));

        if (never > 0)
            insights.Add(("👤", AppTheme.TextSecondary,
                $"{never} customer(s) signed up but NEVER ordered. Consider a first-purchase incentive."));

        if (active > 0 && dormant + atRisk > active)
            insights.Add(("📊", AppTheme.Danger,
                $"Retention concern: {atRisk + dormant} customers are slipping away vs {active} still active."));

        decimal liability = totalPoints / 100m;
        if (liability > 500)
            insights.Add(("💰", AppTheme.WarningAmber,
                $"Outstanding loyalty liability: ₱{liability:N0} ({totalPoints:N0} points). Consider a redemption campaign to reduce liability."));

        foreach (var (icon, color, text) in insights)
        {
            var row = BuildInsightRow(icon, color, text);
            flowInsights.Controls.Add(row);
        }
    }

    // ============================================================
    //  INSIGHTS
    // ============================================================

    private void BuildInsights(
        TenantCrmDbContext db, DateTime now, DateTime? from,
        int totalOrders, decimal avgOrder, int activeCustomers,
        int lowStock, int openFeedback, double avgRating,
        int activePromos, int totalPoints)
    {
        flowInsights.SuspendLayout();
        flowInsights.Controls.Clear();

        var insights = new List<(string Icon, Color Color, string Text)>();

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
                insights.Add(("📈", AppTheme.SuccessGreen,
                    $"Sales are trending UP — ₱{thisWeek:N0} this week vs ₱{lastWeek:N0} last week ({pct:N0}% increase)."));
            else if (pct <= -5)
                insights.Add(("📉", AppTheme.Danger,
                    $"Sales are DOWN — ₱{thisWeek:N0} this week vs ₱{lastWeek:N0} last week ({Math.Abs(pct):N0}% decrease). Consider a promotion."));
            else
                insights.Add(("➖", AppTheme.TextSecondary,
                    $"Sales are steady — ₱{thisWeek:N0} this week vs ₱{lastWeek:N0} last week."));
        }
        else if (thisWeek > 0)
        {
            insights.Add(("📈", AppTheme.SuccessGreen,
                $"Sales this week: ₱{thisWeek:N0}. No prior week to compare."));
        }

        if (_topProducts.Count > 0)
        {
            var top = _topProducts[0];
            insights.Add(("🏆", AppTheme.WarningAmber,
                $"Best performer: '{top.Name}' — {top.Qty:N0} sold."));
        }

        if (avgRating > 0)
        {
            int fiveStar = _ratingDist.Length > 4 ? _ratingDist[4] : 0;
            int totalRatings = _ratingDist.Sum();
            double fiveStarPct = totalRatings > 0 ? fiveStar * 100.0 / totalRatings : 0;

            if (avgRating >= 4.5)
                insights.Add(("⭐", AppTheme.SuccessGreen,
                    $"Customer satisfaction is excellent — average {avgRating:0.0}★ ({fiveStarPct:N0}% are 5-star)."));
            else if (avgRating >= 3.5)
                insights.Add(("⭐", AppTheme.WarningAmber,
                    $"Customer satisfaction is decent — average {avgRating:0.0}★. {openFeedback} reviews still unreviewed."));
            else
                insights.Add(("⚠️", AppTheme.Danger,
                    $"Customer satisfaction is LOW — average {avgRating:0.0}★. Investigate recent complaints."));
        }

        if (lowStock > 0)
        {
            var lowStockItems = db.Inventories
                .Include(i => i.Product)
                .Where(i => i.QuantityOnHand <= i.ReorderLevel)
                .Take(3)
                .ToList()
                .Select(i => $"{i.Product?.ProductName ?? "?"} ({i.QuantityOnHand:N0})")
                .ToList();

            insights.Add(("📦", AppTheme.Danger,
                $"{lowStock} product(s) need restocking: {string.Join(", ", lowStockItems)}."));
        }

        var customerOrderCounts = db.Orders
            .Where(o => from == null || o.OrderDate >= from)
            .GroupBy(o => o.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                Count = g.Count(),
                Spent = g.Sum(o => o.TotalAmount)
            })
            .ToList();

        int repeat = customerOrderCounts.Count(x => x.Count > 1);
        int once = customerOrderCounts.Count(x => x.Count == 1);

        if (repeat > 0 && once > 0)
        {
            decimal avgRepeat = customerOrderCounts.Where(x => x.Count > 1).Average(x => x.Spent);
            decimal avgOnce = customerOrderCounts.Where(x => x.Count == 1).Average(x => x.Spent);
            double ratio = avgOnce > 0 ? (double)(avgRepeat / avgOnce) : 0;

            insights.Add(("🔄", AppTheme.Primary20,
                $"{repeat} repeat customers ({repeat * 100 / (repeat + once)}% of base). Their average spend is ₱{avgRepeat:N0} — {ratio:0.0}x higher than one-time buyers."));
        }

        var promoStats = db.PromotionRedemptions
            .GroupBy(r => r.PromotionId)
            .Select(g => new
            {
                PromoId = g.Key,
                Uses = g.Count(),
                Total = g.Sum(r => r.DiscountApplied)
            })
            .OrderByDescending(x => x.Uses)
            .Take(1)
            .ToList();

        if (promoStats.Any())
        {
            var top = promoStats[0];
            var promo = db.Promotions.FirstOrDefault(p => p.PromotionId == top.PromoId);
            if (promo is not null)
            {
                insights.Add(("🎁", AppTheme.Primary20,
                    $"Top promo: '{promo.PromotionCode}' used {top.Uses} times, giving ₱{top.Total:N0} in discounts."));
            }
        }

        int lowRatingCount = db.CustomerFeedbacks.Count(f => f.Rating <= 2);
        if (lowRatingCount > 0)
        {
            insights.Add(("⚠️", AppTheme.WarningAmber,
                $"{lowRatingCount} customer(s) gave low ratings (≤2★). Consider creating retention offers to win them back."));
        }

        if (_paymentSplit.Count >= 2)
        {
            var top = _paymentSplit.OrderByDescending(x => x.Count).First();
            int total = _paymentSplit.Sum(x => x.Count);
            int pct = total > 0 ? top.Count * 100 / total : 0;
            insights.Add(("💳", AppTheme.TextSecondary,
                $"Payment preference: '{top.Method}' is dominant at {pct}% of transactions."));
        }

        if (totalPoints > 0)
        {
            int customersWithPoints = db.Customers.Count(c => c.CurrentPoints > 0);
            insights.Add(("🎯", AppTheme.SuccessGreen,
                $"{customersWithPoints} customer(s) currently hold {totalPoints:N0} loyalty points (≈ ₱{totalPoints / 100m:N0} liability)."));
        }

        var lastSaleDate = db.Transactions
            .OrderByDescending(t => t.PaidAt)
            .Select(t => (DateTime?)t.PaidAt)
            .FirstOrDefault();

        if (lastSaleDate.HasValue)
        {
            int daysSinceLastSale = (int)(now - lastSaleDate.Value).TotalDays;
            if (daysSinceLastSale >= 3)
                insights.Add(("💤", AppTheme.Danger,
                    $"No sales recorded in {daysSinceLastSale} days. Check system or staff activity."));
        }

        if (insights.Count == 0)
        {
            insights.Add(("ℹ️", AppTheme.TextSecondary,
                "Not enough data yet to generate insights. Start recording orders to see recommendations."));
        }

        foreach (var (icon, color, text) in insights)
        {
            var row = BuildInsightRow(icon, color, text);
            flowInsights.Controls.Add(row);
        }

        flowInsights.ResumeLayout();
    }

    private Panel BuildInsightRow(string icon, Color accentColor, string text)
    {
        int availableWidth = flowInsights.ClientSize.Width
                             - flowInsights.Padding.Left
                             - flowInsights.Padding.Right
                             - 20;

        if (availableWidth < 300) availableWidth = 300;

        var textFont = new Font("Segoe UI", 9.5F);
        var textSize = TextRenderer.MeasureText(
            text,
            textFont,
            new Size(availableWidth - 100, int.MaxValue),
            TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);

        int rowHeight = Math.Max(54, textSize.Height + 24);

        var row = new Panel
        {
            Width = availableWidth,
            Height = rowHeight,
            Margin = new Padding(0, 3, 0, 3),
            BackColor = Color.FromArgb(248, 250, 252)
        };

        var bar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 5,
            BackColor = accentColor
        };

        var lblIcon = new Label
        {
            Text = icon,
            Font = new Font("Segoe UI", 15F),
            ForeColor = AppTheme.TextPrimary,
            Location = new Point(18, (rowHeight - 28) / 2),
            AutoSize = false,
            Size = new Size(34, 28),
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = Color.Transparent
        };

        var lblText = new Label
        {
            Text = text,
            Font = textFont,
            ForeColor = AppTheme.TextPrimary,
            Location = new Point(60, 8),
            AutoSize = false,
            Size = new Size(availableWidth - 80, rowHeight - 16),
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft
        };

        row.Controls.Add(lblText);
        row.Controls.Add(lblIcon);
        row.Controls.Add(bar);

        return row;
    }

    private void ResizeInsightRows()
    {
        if (flowInsights.Controls.Count == 0) return;

        int availableWidth = flowInsights.ClientSize.Width
                             - flowInsights.Padding.Left
                             - flowInsights.Padding.Right
                             - 20;

        if (availableWidth < 300) return;

        flowInsights.SuspendLayout();

        foreach (Control c in flowInsights.Controls)
        {
            if (c is not Panel row) continue;

            row.Width = availableWidth;

            var lblText = row.Controls.OfType<Label>()
                .FirstOrDefault(l => l.AutoSize == false && l.Location.X > 30);

            if (lblText is not null)
                lblText.Width = availableWidth - 80;
        }

        flowInsights.ResumeLayout();
    }

    private void SetCardValue(Panel card, string value, string subtitle)
    {
        var lblValue = card.Controls["lblValue"] as Label;
        var lblSub = card.Controls["lblSub"] as Label;

        if (lblValue is not null) lblValue.Text = value;
        if (lblSub is not null) lblSub.Text = subtitle;
    }

    // ============================================================
    //  CHART DATA
    // ============================================================
    private List<(DateTime Day, decimal Total)> _salesLast7 = new();
    private List<(string Name, int Qty)> _topProducts = new();
    private int[] _ratingDist = new int[5];
    private List<(string Method, int Count)> _paymentSplit = new();

    private List<(string Status, int Count)> _retentionSplit = new();
    private int[] _retentionRecency = new int[6];

    // ============================================================
    //  CHART PAINTER
    // ============================================================
    private void ChartPanel_Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not Panel pnl) return;

        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        var titleFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        var textFont = new Font("Segoe UI", 8.5F);
        var smallFont = new Font("Segoe UI", 7.5F);

        using var titleBrush = new SolidBrush(AppTheme.TextPrimary);
        using var textBrush = new SolidBrush(AppTheme.TextSecondary);
        using var barBrush = new SolidBrush(AppTheme.Primary20);
        using var accentBrush = new SolidBrush(AppTheme.SuccessGreen);
        using var gridPen = new Pen(Color.FromArgb(230, 234, 240), 1);

        var area = pnl.ClientRectangle;

        using var hintFont = new Font("Segoe UI", 7.5F, FontStyle.Italic);
        using var hintBrush = new SolidBrush(AppTheme.TextMuted);
        g.DrawString("click to view", hintFont, hintBrush, area.Width - 90, 14);

        if (pnl == pnlSalesChart)
            DrawSalesChart(g, area, titleFont, textFont, smallFont, titleBrush, textBrush, barBrush, gridPen);
        else if (pnl == pnlTopProductsChart)
            DrawTopProducts(g, area, titleFont, textFont, smallFont, titleBrush, textBrush, barBrush);
        else if (pnl == pnlRatingChart)
            DrawRatingDist(g, area, titleFont, textFont, smallFont, titleBrush, textBrush, accentBrush);
        else if (pnl == pnlPaymentChart)
            DrawPaymentSplit(g, area, titleFont, textFont, smallFont, titleBrush, textBrush);
        else if (pnl == pnlRetentionSplitChart)
            DrawRetentionSplit(g, area, titleFont, textFont, smallFont, titleBrush, textBrush);
        else if (pnl == pnlRetentionRecencyChart)
            DrawRetentionRecency(g, area, titleFont, textFont, smallFont, titleBrush, textBrush, barBrush, gridPen);
    }

    // ============================================================
    //  EXISTING CHARTS (restored — these were missing from your file)
    // ============================================================

    private void DrawSalesChart(Graphics g, Rectangle area, Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush, Brush barBrush, Pen gridPen)
    {
        g.DrawString("Sales — Last 7 Days", titleFont, titleBrush, 16, 14);
        if (_salesLast7.Count == 0) return;

        decimal max = _salesLast7.Max(x => x.Total);
        if (max <= 0) max = 1;

        int chartTop = 60;
        int chartBottom = area.Height - 40;
        int chartLeft = 50;
        int chartRight = area.Width - 20;
        int chartHeight = chartBottom - chartTop;
        int chartWidth = chartRight - chartLeft;

        for (int i = 0; i <= 4; i++)
        {
            int y = chartTop + (chartHeight * i / 4);
            g.DrawLine(gridPen, chartLeft, y, chartRight, y);
        }

        int n = _salesLast7.Count;
        int barSpace = chartWidth / n;
        int barWidth = (int)(barSpace * 0.6);
        int gap = (barSpace - barWidth) / 2;

        for (int i = 0; i < n; i++)
        {
            var (day, total) = _salesLast7[i];
            int barH = (int)(chartHeight * (double)(total / max));
            int x = chartLeft + i * barSpace + gap;
            int y = chartBottom - barH;

            g.FillRectangle(barBrush, x, y, barWidth, barH);

            string val = total > 0 ? $"₱{total:N0}" : "";
            if (val.Length > 0)
                g.DrawString(val, smallFont, textBrush, x + barWidth / 2 - 20, y - 16);

            string label = day.ToString("ddd");
            g.DrawString(label, textFont, textBrush, x + barWidth / 2 - 14, chartBottom + 6);
        }
    }

    private void DrawTopProducts(Graphics g, Rectangle area, Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush, Brush barBrush)
    {
        g.DrawString("Top 5 Best-Selling Products", titleFont, titleBrush, 16, 14);

        if (_topProducts.Count == 0)
        {
            g.DrawString("No sales data yet.", textFont, textBrush, 16, 60);
            return;
        }

        int maxQty = _topProducts.Max(x => x.Qty);
        if (maxQty == 0) maxQty = 1;

        int startY = 60;
        int rowH = 42;
        int labelW = 180;
        int barMax = area.Width - labelW - 60;

        for (int i = 0; i < _topProducts.Count; i++)
        {
            var (name, qty) = _topProducts[i];
            int y = startY + i * rowH;

            g.DrawString(name, textFont, textBrush, 16, y + 6);

            int barW = (int)(barMax * ((double)qty / maxQty));
            if (barW < 4 && qty > 0) barW = 4;

            g.FillRectangle(barBrush, labelW, y + 8, barW, 20);
            g.DrawString($"{qty:N0} sold", smallFont, textBrush, labelW + barW + 8, y + 12);
        }
    }

    private void DrawRatingDist(Graphics g, Rectangle area, Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush, Brush barBrush)
    {
        g.DrawString("Feedback Rating Distribution", titleFont, titleBrush, 16, 14);

        int max = _ratingDist.Length > 0 ? _ratingDist.Max() : 0;
        if (max == 0) max = 1;

        int chartTop = 60;
        int chartBottom = area.Height - 40;
        int chartLeft = 60;
        int chartRight = area.Width - 20;
        int chartHeight = chartBottom - chartTop;
        int chartWidth = chartRight - chartLeft;

        int barSpace = chartWidth / 5;
        int barWidth = (int)(barSpace * 0.55);
        int gap = (barSpace - barWidth) / 2;

        for (int i = 0; i < 5; i++)
        {
            int count = _ratingDist[i];
            int barH = (int)(chartHeight * ((double)count / max));
            int x = chartLeft + i * barSpace + gap;
            int y = chartBottom - barH;

            g.FillRectangle(barBrush, x, y, barWidth, barH);

            if (count > 0)
                g.DrawString($"{count}", smallFont, textBrush, x + barWidth / 2 - 6, y - 15);

            string label = $"{i + 1}★";
            g.DrawString(label, textFont, textBrush, x + barWidth / 2 - 10, chartBottom + 6);
        }
    }

    private void DrawPaymentSplit(Graphics g, Rectangle area, Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush)
    {
        g.DrawString("Payment Method Split", titleFont, titleBrush, 16, 14);

        if (_paymentSplit.Count == 0)
        {
            g.DrawString("No payment data yet.", textFont, textBrush, 16, 60);
            return;
        }

        int total = _paymentSplit.Sum(x => x.Count);
        if (total == 0) return;

        int cx = area.Width / 3;
        int cy = area.Height / 2 + 20;
        int radius = Math.Min(area.Width, area.Height) / 3 - 30;

        Color[] palette = { AppTheme.SuccessGreen, AppTheme.Primary20, AppTheme.WarningAmber, AppTheme.Danger };

        float startAngle = -90f;
        for (int i = 0; i < _paymentSplit.Count; i++)
        {
            var (_, count) = _paymentSplit[i];
            float sweep = 360f * count / total;

            using var sliceBrush = new SolidBrush(palette[i % palette.Length]);
            g.FillPie(sliceBrush, cx - radius, cy - radius, radius * 2, radius * 2, startAngle, sweep);

            startAngle += sweep;
        }

        int ly = 70;
        for (int i = 0; i < _paymentSplit.Count; i++)
        {
            var (method, count) = _paymentSplit[i];
            using var sliceBrush = new SolidBrush(palette[i % palette.Length]);

            g.FillRectangle(sliceBrush, cx + radius + 30, ly, 16, 16);
            g.DrawString($"{method}: {count} ({count * 100 / total}%)",
                textFont, textBrush, cx + radius + 54, ly);
            ly += 28;
        }
    }

    // ============================================================
    //  NEW: retention charts
    // ============================================================

    private void DrawRetentionSplit(Graphics g, Rectangle area,
        Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush)
    {
        g.DrawString("Customer Retention Split", titleFont, titleBrush, 16, 14);

        if (_retentionSplit.Count == 0)
        {
            g.DrawString("No customer data yet.", textFont, textBrush, 16, 60);
            return;
        }

        int total = _retentionSplit.Sum(x => x.Count);
        if (total == 0) return;

        int cx = area.Width / 3;
        int cy = area.Height / 2 + 20;
        int radius = Math.Min(area.Width, area.Height) / 3 - 30;

        Color[] palette =
        {
            Color.FromArgb(22, 130, 60),
            Color.FromArgb(200, 130, 0),
            Color.FromArgb(190, 40, 40),
            Color.FromArgb(120, 120, 120)
        };

        float startAngle = -90f;
        for (int i = 0; i < _retentionSplit.Count; i++)
        {
            var (_, count) = _retentionSplit[i];
            if (count == 0) continue;

            float sweep = 360f * count / total;

            using var sliceBrush = new SolidBrush(palette[i % palette.Length]);
            g.FillPie(sliceBrush, cx - radius, cy - radius, radius * 2, radius * 2, startAngle, sweep);

            startAngle += sweep;
        }

        int ly = 70;
        for (int i = 0; i < _retentionSplit.Count; i++)
        {
            var (status, count) = _retentionSplit[i];
            using var sliceBrush = new SolidBrush(palette[i % palette.Length]);

            g.FillRectangle(sliceBrush, cx + radius + 30, ly, 16, 16);
            g.DrawString($"{status}: {count} ({count * 100 / total}%)",
                textFont, textBrush, cx + radius + 54, ly);
            ly += 28;
        }
    }

    private void DrawRetentionRecency(Graphics g, Rectangle area,
        Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush, Brush barBrush, Pen gridPen)
    {
        g.DrawString("Recency — Days Since Last Order", titleFont, titleBrush, 16, 14);

        int max = _retentionRecency.Length > 0 ? _retentionRecency.Max() : 0;
        if (max == 0) max = 1;

        int chartTop = 60;
        int chartBottom = area.Height - 40;
        int chartLeft = 50;
        int chartRight = area.Width - 20;
        int chartHeight = chartBottom - chartTop;
        int chartWidth = chartRight - chartLeft;

        for (int i = 0; i <= 4; i++)
        {
            int y = chartTop + (chartHeight * i / 4);
            g.DrawLine(gridPen, chartLeft, y, chartRight, y);
        }

        string[] labels = { "0-7d", "8-30d", "31-60d", "61-90d", "91-180d", "180+d" };
        Color[] colors =
        {
            Color.FromArgb(22, 130, 60),
            Color.FromArgb(60, 160, 80),
            Color.FromArgb(200, 130, 0),
            Color.FromArgb(220, 100, 40),
            Color.FromArgb(200, 60, 40),
            Color.FromArgb(140, 40, 40)
        };

        int n = _retentionRecency.Length;
        int barSpace = chartWidth / n;
        int barWidth = (int)(barSpace * 0.6);
        int gap = (barSpace - barWidth) / 2;

        for (int i = 0; i < n; i++)
        {
            int count = _retentionRecency[i];
            int barH = (int)(chartHeight * ((double)count / max));
            int x = chartLeft + i * barSpace + gap;
            int y = chartBottom - barH;

            using var brush = new SolidBrush(colors[i]);
            g.FillRectangle(brush, x, y, barWidth, barH);

            if (count > 0)
                g.DrawString($"{count}", smallFont, textBrush,
                    x + barWidth / 2 - 6, y - 15);

            g.DrawString(labels[i], smallFont, textBrush,
                x + barWidth / 2 - 14, chartBottom + 6);
        }
    }
}