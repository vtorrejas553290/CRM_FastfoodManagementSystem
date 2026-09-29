using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace CRM.winForms.Forms;

public partial class FrmSuperAdminBI : Form
{
    // ---- Chart data (populated in LoadAll) ----
    private List<(string PlanCode, int Count)> _subsByPlan = new();
    private List<(string PlanCode, int FeatureCount)> _planFeatures = new();

    public FrmSuperAdminBI()
    {
        InitializeComponent();
        ApplyTheme();

        Load += (_, __) => LoadAll();
        btnRefresh.Click += (_, __) => LoadAll();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);

        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;

        pnlSubscriptions.BackColor = AppTheme.Surface;
        pnlPlans.BackColor = AppTheme.Surface;
        pnlRouting.BackColor = AppTheme.Surface;

        lblHeaderTitle.Font = AppTheme.FontHeading;
        lblHeaderTitle.ForeColor = AppTheme.TextPrimary;
        lblHeaderSubtitle.ForeColor = AppTheme.TextSecondary;

        lblSubscriptionsTitle.Font = AppTheme.FontSubheading;
        lblSubscriptionsTitle.ForeColor = AppTheme.TextPrimary;
        lblPlansTitle.Font = AppTheme.FontSubheading;
        lblPlansTitle.ForeColor = AppTheme.TextPrimary;
        lblRoutingTitle.Font = AppTheme.FontSubheading;
        lblRoutingTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleLabel(lblStatus);

        AppTheme.StyleGrid(gridSubscriptions);
        AppTheme.StyleGrid(gridPlans);
        AppTheme.StyleGrid(gridRouting);

        foreach (Control c in tblKpis.Controls)
            StyleKpiCard(c);

        // Wire up chart painting
        pnlSubsByPlanChart.BackColor = AppTheme.Surface;
        pnlSubsByPlanChart.Paint += ChartPanel_Paint;

        pnlPlanFeaturesChart.BackColor = AppTheme.Surface;
        pnlPlanFeaturesChart.Paint += ChartPanel_Paint;
    }

    private void StyleKpiCard(Control card)
    {
        card.BackColor = AppTheme.Surface;

        var lblValue = card.Controls["lblValue"];
        if (lblValue is not null) lblValue.ForeColor = AppTheme.Primary20;

        var lblSub = card.Controls["lblSub"];
        if (lblSub is not null) lblSub.ForeColor = AppTheme.TextSecondary;
    }

    private void LoadAll()
    {
        try
        {
            using var db = AppServices.CreateMasterContext();

            // ---- KPI cards ----
            int tenantCount = db.Companies.Count(c => c.IsActive);
            int activeSubs = db.Subscriptions.Count(s => s.IsActive);
            int planCount = db.Plans.Count();
            int routeCount = db.CompanyDatabases.Count(cd => cd.IsActive);

            SetCardValue(cardTenants, tenantCount.ToString("N0"), "active companies");
            SetCardValue(cardSubs, activeSubs.ToString("N0"), "active subscriptions");
            SetCardValue(cardPlans, planCount.ToString("N0"), "plan tiers");
            SetCardValue(cardRoutes, routeCount.ToString("N0"), "database routes");

            // ---- Subscriptions table ----
            var subRows = (
                from s in db.Subscriptions.AsNoTracking()
                join c in db.Companies.AsNoTracking() on s.CompanyId equals c.CompanyId
                join p in db.Plans.AsNoTracking() on s.PlanId equals p.PlanId
                orderby c.CompanyCode
                select new
                {
                    CompanyCode = c.CompanyCode,
                    CompanyName = c.CompanyName,
                    Plan = p.PlanCode,
                    Started = s.StartedAt,
                    Ends = s.EndsAt,
                    Active = s.IsActive
                }
            ).ToList();

            gridSubscriptions.DataSource = subRows;

            // ---- Plan feature matrix ----
            var plans = db.Plans.AsNoTracking()
                .OrderBy(p => p.PlanCode)
                .ToList();

            var planRows = plans
                .Select(p => new
                {
                    Plan = p.PlanCode,
                    MainTransaction = p.HasMainTransaction ? "✓" : "—",
                    DataCollection = p.HasDataCollection ? "✓" : "—",
                    BusinessIntel = p.HasBusinessIntelligence ? "✓" : "—",
                    Actions = p.HasActions ? "✓" : "—",
                    Branching = p.HasBranching ? "✓" : "—"
                })
                .ToList();

            gridPlans.DataSource = planRows;

            // ---- Database routing table ----
            var routeRows = (
                from cd in db.CompanyDatabases.AsNoTracking()
                join c in db.Companies.AsNoTracking() on cd.CompanyId equals c.CompanyId
                orderby c.CompanyCode
                select new
                {
                    Company = c.CompanyCode,
                    Server = cd.ServerName,
                    Database = cd.DatabaseName,
                    CredentialKey = cd.CredentialKey,
                    Active = cd.IsActive
                }
            ).ToList();

            gridRouting.DataSource = routeRows;

            // ---- Chart data ----

            // Subscriptions by Plan
            _subsByPlan = (
                from s in db.Subscriptions.AsNoTracking()
                join p in db.Plans.AsNoTracking() on s.PlanId equals p.PlanId
                where s.IsActive
                group s by p.PlanCode into g
                orderby g.Key
                select new
                {
                    PlanCode = g.Key,
                    Count = g.Count()
                }
            ).ToList()
            .Select(x => (x.PlanCode, x.Count))
            .ToList();

            // Feature count per plan
            _planFeatures = plans
                .Select(p => (
                    PlanCode: p.PlanCode,
                    FeatureCount:
                        (p.HasMainTransaction ? 1 : 0) +
                        (p.HasDataCollection ? 1 : 0) +
                        (p.HasBusinessIntelligence ? 1 : 0) +
                        (p.HasActions ? 1 : 0) +
                        (p.HasBranching ? 1 : 0)
                ))
                .ToList();

            pnlSubsByPlanChart.Invalidate();
            pnlPlanFeaturesChart.Invalidate();

            lblStatus.Text = $"Updated {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetCardValue(Panel card, string value, string subtitle)
    {
        var lblValue = card.Controls["lblValue"] as Label;
        var lblSub = card.Controls["lblSub"] as Label;

        if (lblValue is not null) lblValue.Text = value;
        if (lblSub is not null) lblSub.Text = subtitle;
    }

    // ============================================================
    //  CHART PAINTING
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

        var area = pnl.ClientRectangle;

        if (pnl == pnlSubsByPlanChart)
            DrawSubsByPlan(g, area, titleFont, textFont, smallFont, titleBrush, textBrush);
        else if (pnl == pnlPlanFeaturesChart)
            DrawPlanFeatures(g, area, titleFont, textFont, smallFont, titleBrush, textBrush);
    }

    private void DrawSubsByPlan(Graphics g, Rectangle area,
        Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush)
    {
        g.DrawString("Active Subscriptions by Plan", titleFont, titleBrush, 16, 14);

        if (_subsByPlan.Count == 0)
        {
            g.DrawString("No active subscriptions.", textFont, textBrush, 16, 60);
            return;
        }

        int total = _subsByPlan.Sum(x => x.Count);
        if (total == 0) return;

        int cx = area.Width / 3;
        int cy = area.Height / 2 + 20;
        int radius = Math.Min(area.Width, area.Height) / 3 - 30;

        Color[] palette =
        {
            Color.FromArgb(120, 170, 210),
            Color.FromArgb(60, 130, 200),
            Color.FromArgb(20, 60, 120)
        };

        float startAngle = -90f;
        for (int i = 0; i < _subsByPlan.Count; i++)
        {
            var (_, count) = _subsByPlan[i];
            float sweep = 360f * count / total;

            using var sliceBrush = new SolidBrush(palette[i % palette.Length]);
            g.FillPie(sliceBrush, cx - radius, cy - radius, radius * 2, radius * 2,
                startAngle, sweep);

            startAngle += sweep;
        }

        int ly = 70;
        for (int i = 0; i < _subsByPlan.Count; i++)
        {
            var (plan, count) = _subsByPlan[i];
            using var sliceBrush = new SolidBrush(palette[i % palette.Length]);

            g.FillRectangle(sliceBrush, cx + radius + 30, ly, 16, 16);
            g.DrawString($"{plan}: {count} ({count * 100 / total}%)",
                textFont, textBrush, cx + radius + 54, ly);
            ly += 28;
        }
    }

    private void DrawPlanFeatures(Graphics g, Rectangle area,
        Font titleFont, Font textFont, Font smallFont,
        Brush titleBrush, Brush textBrush)
    {
        g.DrawString("Feature Count per Plan", titleFont, titleBrush, 16, 14);

        if (_planFeatures.Count == 0)
        {
            g.DrawString("No plans defined.", textFont, textBrush, 16, 60);
            return;
        }

        int maxFeatures = 5;

        int chartTop = 60;
        int chartBottom = area.Height - 40;
        int chartLeft = 60;
        int chartRight = area.Width - 30;
        int chartHeight = chartBottom - chartTop;
        int chartWidth = chartRight - chartLeft;

        int n = _planFeatures.Count;
        int barSpace = chartWidth / Math.Max(n, 1);
        int barWidth = (int)(barSpace * 0.55);
        int gap = (barSpace - barWidth) / 2;

        using var gridPen = new Pen(Color.FromArgb(230, 234, 240), 1);
        for (int i = 0; i <= maxFeatures; i++)
        {
            int y = chartBottom - (chartHeight * i / maxFeatures);
            g.DrawLine(gridPen, chartLeft, y, chartRight, y);
            g.DrawString(i.ToString(), smallFont, textBrush, chartLeft - 24, y - 8);
        }

        Color[] barColors =
        {
            Color.FromArgb(120, 170, 210),
            Color.FromArgb(60, 130, 200),
            Color.FromArgb(20, 60, 120)
        };

        for (int i = 0; i < n; i++)
        {
            var (plan, count) = _planFeatures[i];
            int barH = (int)(chartHeight * ((double)count / maxFeatures));
            int x = chartLeft + i * barSpace + gap;
            int y = chartBottom - barH;

            using var barBrush = new SolidBrush(barColors[i % barColors.Length]);
            g.FillRectangle(barBrush, x, y, barWidth, barH);

            if (count > 0)
                g.DrawString($"{count}/5", smallFont, textBrush, x + barWidth / 2 - 16, y - 15);

            g.DrawString(plan, textFont, textBrush, x + barWidth / 2 - 22, chartBottom + 6);
        }
    }
}