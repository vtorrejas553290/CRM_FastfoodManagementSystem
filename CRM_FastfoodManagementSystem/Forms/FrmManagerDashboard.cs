using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmManagerDashboard : Form
{
    public FrmManagerDashboard()
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
        pnlRecentOrders.BackColor = AppTheme.Surface;

        lblHeaderTitle.Font = AppTheme.FontHeading;
        lblHeaderTitle.ForeColor = AppTheme.TextPrimary;
        lblHeaderSubtitle.ForeColor = AppTheme.TextSecondary;

        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridRecentOrders);

        lblRecentOrders.Font = AppTheme.FontSubheading;
        lblRecentOrders.ForeColor = AppTheme.TextPrimary;

        foreach (Control c in flowCards.Controls)
            StyleKpiCard(c);
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

    private void LoadAll()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var now = DateTime.UtcNow;

            // Today's Sales
            decimal todaySales = db.Transactions
                .Where(t => t.PaidAt.Date == now.Date)
                .Select(t => (decimal?)t.AmountPaid)
                .Sum() ?? 0m;

            int todayTxns = db.Transactions.Count(t => t.PaidAt.Date == now.Date);
            SetCardValue(cardTodaySales, $"₱{todaySales:N0}", $"{todayTxns} transactions");

            // Today's Orders
            int todayOrders = db.Orders.Count(o => o.OrderDate.Date == now.Date);
            int totalOrders = db.Orders.Count();
            SetCardValue(cardTodayOrders, todayOrders.ToString("N0"), $"{totalOrders} all-time");

            // Low Stock
            int lowStock = db.Inventories.Count(i => i.QuantityOnHand <= i.ReorderLevel);
            SetCardValue(cardLowStock, lowStock.ToString("N0"), "need restock");

            // Open Feedback
            int openFeedback = db.CustomerFeedbacks.Count(f => f.Status == "New");
            int totalFeedback = db.CustomerFeedbacks.Count();
            SetCardValue(cardOpenFeedback, openFeedback.ToString("N0"), $"{totalFeedback} total");

            // Active Promotions
            int activePromos = db.Promotions.Count(p =>
                p.IsActive && p.StartDate <= now && p.EndDate >= now);
            SetCardValue(cardActivePromos, activePromos.ToString("N0"), "running now");

            // Pending Retention
            int pending = 0;
            SetCardValue(cardPendingRetention, pending.ToString("N0"), "planned");

            // Recent Orders (with safe null handling for Customer)
            gridRecentOrders.DataSource = db.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Take(15)
                .ToList()
                .Select(o => new
                {
                    o.OrderCode,
                    Customer = o.Customer?.CustomerName ?? "(deleted)",
                    o.OrderType,
                    o.TotalAmount,
                    o.Status,
                    o.OrderDate
                })
                .ToList();

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
}