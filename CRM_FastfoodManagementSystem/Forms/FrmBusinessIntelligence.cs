using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;
using System.Drawing.Drawing2D;

namespace CRM.winForms.Forms;

public partial class FrmBusinessIntelligence : Form
{
    public event Action<string>? NavigateRequested;

    private readonly IBiController _controller = new BiController();

    private bool _syncingDates = false;

    // ---- Chart data (populated from BiSnapshot) ----
    private List<(DateTime Day, decimal Total)> _salesLast7 = new();
    private List<(string Name, int Qty)> _topProducts = new();
    private int[] _ratingDist = new int[5];
    private List<(string Method, int Count)> _paymentSplit = new();
    private List<(string Status, int Count)> _retentionSplit = new();
    private int[] _retentionRecency = new int[6];

    public FrmBusinessIntelligence()
    {
        InitializeComponent();
        ApplyTheme();

        Load += FrmBusinessIntelligence_Load;

        cmbDateRange.SelectedIndexChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            SyncDatePickersFromPeriod();
            _syncingDates = false;

            LoadAll();
        };

        dtpFromDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateRange.SelectedItem?.ToString() != "Custom Range")
                cmbDateRange.SelectedItem = "Custom Range";
            _syncingDates = false;

            LoadAll();
        };

        dtpToDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateRange.SelectedItem?.ToString() != "Custom Range")
                cmbDateRange.SelectedItem = "Custom Range";
            _syncingDates = false;

            LoadAll();
        };

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

        lblHeaderSubtitle.ForeColor = AppTheme.TextSecondary;

        AppTheme.StyleLabel(lblDateRange);
        AppTheme.StyleLabel(lblFromDate);
        AppTheme.StyleLabel(lblToDate);
        AppTheme.StyleInput(cmbDateRange);
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

        foreach (Control c in tblKpiRow1.Controls) StyleKpiCard(c);
        foreach (Control c in tblKpiRow2.Controls) StyleKpiCard(c);
        foreach (Control c in tblKpiRow3.Controls) StyleKpiCard(c);

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
        if (lblValue != null) lblValue.ForeColor = AppTheme.Primary20;

        var lblSub = card.Controls["lblSub"];
        if (lblSub != null) lblSub.ForeColor = AppTheme.TextSecondary;
    }

    private void FrmBusinessIntelligence_Load(object? sender, EventArgs e)
    {
        cmbDateRange.Items.Clear();
        cmbDateRange.Items.AddRange(new object[]
        {
            "Today",
            "Last 7 Days",
            "Last 30 Days",
            "This Month",
            "Last Month",
            "This Year",
            "Last Year",
            "All Time",
            "Custom Range"
        });

        _syncingDates = true;
        cmbDateRange.SelectedItem = "This Month";
        SyncDatePickersFromPeriod();
        _syncingDates = false;

        LoadAll();

        BeginInvoke(new Action(() =>
        {
            pnlBody.AutoScrollPosition = new Point(0, 0);
        }));
    }

    /// <summary>
    /// Fills From/To pickers from the selected period.
    /// "Custom Range" leaves them alone.
    /// </summary>
    private void SyncDatePickersFromPeriod()
    {
        var now = DateTime.UtcNow;
        var period = cmbDateRange.SelectedItem?.ToString() ?? "This Month";

        DateTime from;
        DateTime to = now.Date;

        switch (period)
        {
            case "Today":
                from = now.Date;
                to = now.Date;
                break;
            case "Last 7 Days":
                from = now.Date.AddDays(-7);
                break;
            case "Last 30 Days":
                from = now.Date.AddDays(-30);
                break;
            case "This Month":
                from = new DateTime(now.Year, now.Month, 1);
                break;
            case "Last Month":
                var lastMonthEnd = new DateTime(now.Year, now.Month, 1).AddDays(-1);
                from = new DateTime(lastMonthEnd.Year, lastMonthEnd.Month, 1);
                to = lastMonthEnd;
                break;
            case "This Year":
                from = new DateTime(now.Year, 1, 1);
                break;
            case "Last Year":
                from = new DateTime(now.Year - 1, 1, 1);
                to = new DateTime(now.Year - 1, 12, 31);
                break;
            case "All Time":
                from = new DateTime(2000, 1, 1);
                break;
            case "Custom Range":
                return;
            default:
                from = now.Date.AddDays(-30);
                break;
        }

        dtpFromDate.Value = from;
        dtpToDate.Value = to;
    }

    private (DateTime? from, DateTime? to) GetFromTo()
    {
        var period = cmbDateRange.SelectedItem?.ToString() ?? "This Month";

        if (period == "All Time")
            return (null, null);

        DateTime from = dtpFromDate.Value.Date;
        DateTime to = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
        return (from, to);
    }

    // ============================================================
    //  LOAD ALL
    // ============================================================

    private void LoadAll()
    {
        try
        {
            var (from, to) = GetFromTo();

            var filter = new BiFilter
            {
                Period = cmbDateRange.SelectedItem?.ToString() ?? "This Month",
                FromDate = from,
                ToDate = to,
                Now = DateTime.UtcNow
            };

            var s = _controller.GetSnapshot(filter);

            // ---- KPI Row 1 ----
            SetCardValue(cardRevenue, $"₱{s.TotalRevenue:N0}",
                $"{s.TotalTransactions} total transactions");
            SetCardValue(cardTodaySales, $"₱{s.TodaySales:N0}",
                $"{s.TodayOrders} orders today");
            SetCardValue(cardOrders, s.TotalOrders.ToString("N0"), "in period");
            SetCardValue(cardAvgOrder, $"₱{s.AverageOrderValue:N0}", "per order");
            SetCardValue(cardCustomers, s.ActiveCustomers.ToString("N0"),
                $"+{s.NewCustomersLast30Days} new (30d)");
            SetCardValue(cardPoints, s.TotalPoints.ToString("N0"),
                $"≈ ₱{s.TotalPoints / 100m:N0} value");

            // ---- KPI Row 2 ----
            SetCardValue(cardLowStock, s.LowStockCount.ToString("N0"), "need restock");
            SetCardValue(cardOpenFeedback, s.OpenFeedbackCount.ToString("N0"), "unreviewed");
            SetCardValue(cardAvgRating, s.AverageRating.ToString("0.0") + " ★",
                $"{s.TotalFeedbackCount} ratings");
            SetCardValue(cardPromos, s.ActivePromotionsCount.ToString("N0"), "running now");

            // ---- KPI Row 3 ----
            SetCardValue(cardAtRisk, s.AtRiskCount.ToString("N0"), "missed 30-60d");
            SetCardValue(cardDormant, s.DormantCount.ToString("N0"), "no order in 60+d");
            SetCardValue(cardNeverOrdered, s.NeverOrderedCount.ToString("N0"), "signed up only");
            SetCardValue(cardPointsLiability, $"₱{s.PointsLiability:N0}",
                $"{s.TotalPoints:N0} pts outstanding");

            // ---- Tables ----
            gridAtRisk.DataSource = s.AtRiskList
                .Select(r => new
                {
                    Customer = r.Cells.GetValueOrDefault("Customer"),
                    DaysSince = r.Cells.GetValueOrDefault("DaysSince"),
                    Points = r.Cells.GetValueOrDefault("Points"),
                    Status = r.Cells.GetValueOrDefault("Status")
                })
                .ToList();

            gridRecentOrders.DataSource = s.RecentOrders
                .Select(r => new
                {
                    OrderCode = r.Cells.GetValueOrDefault("OrderCode"),
                    Customer = r.Cells.GetValueOrDefault("Customer"),
                    TotalAmount = r.Cells.GetValueOrDefault("TotalAmount"),
                    Status = r.Cells.GetValueOrDefault("Status"),
                    OrderDate = r.Cells.GetValueOrDefault("OrderDate")
                })
                .ToList();

            gridTopCustomers.DataSource = s.TopCustomers
                .Select(r => new
                {
                    Customer = r.Cells.GetValueOrDefault("Customer"),
                    Orders = r.Cells.GetValueOrDefault("Orders"),
                    TotalSpent = r.Cells.GetValueOrDefault("TotalSpent")
                })
                .ToList();

            // ---- Chart data ----
            _salesLast7 = s.SalesLast7
                .Select(x => (x.Day, x.Total))
                .ToList();
            _topProducts = s.TopProducts
                .Select(x => (x.Name, x.Qty))
                .ToList();
            _ratingDist = s.RatingDistribution;
            _paymentSplit = s.PaymentSplit
                .Select(x => (x.Method, x.Count))
                .ToList();
            _retentionSplit = s.RetentionSplit
                .Select(x => (x.Status, x.Count))
                .ToList();
            _retentionRecency = s.RetentionRecency;

            // ---- Insights ----
            BuildInsights(s);

            pnlSalesChart.Invalidate();
            pnlTopProductsChart.Invalidate();
            pnlRatingChart.Invalidate();
            pnlPaymentChart.Invalidate();
            pnlRetentionSplitChart.Invalidate();
            pnlRetentionRecencyChart.Invalidate();

            lblStatus.Text =
                $"Updated {DateTime.Now:HH:mm:ss}  —  Period: {filter.Period}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BuildInsights(BiSnapshot s)
    {
        flowInsights.SuspendLayout();
        flowInsights.Controls.Clear();

        foreach (var insight in s.Insights)
        {
            Color accent = insight.Semantic switch
            {
                "success" => AppTheme.SuccessGreen,
                "warning" => AppTheme.WarningAmber,
                "danger" => AppTheme.Danger,
                "info" => AppTheme.Primary20,
                _ => AppTheme.TextSecondary
            };

            var row = BuildInsightRow(insight.Icon, accent, insight.Text);
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

            if (lblText != null)
                lblText.Width = availableWidth - 80;
        }

        flowInsights.ResumeLayout();
    }

    private void SetCardValue(Panel card, string value, string subtitle)
    {
        var lblValue = card.Controls["lblValue"] as Label;
        var lblSub = card.Controls["lblSub"] as Label;

        if (lblValue != null) lblValue.Text = value;
        if (lblSub != null) lblSub.Text = subtitle;
    }

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