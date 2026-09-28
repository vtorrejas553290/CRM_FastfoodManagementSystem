using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmAdminDashboard : Form
{
    public FrmAdminDashboard()
    {
        InitializeComponent();
        ApplyTheme();

        Load += (_, __) => LoadAll();
        btnRefresh.Click += (_, __) => LoadAll();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlRecentOrders.BackColor = AppTheme.Surface;

        AppTheme.StyleGrid(gridRecentOrders);

        lblRecentOrders.Font = AppTheme.FontSubheading;
        lblRecentOrders.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleLabel(lblStatus);

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

            // 1. Total Customers
            int totalCustomers = db.Customers.Count();
            SetCardValue(cardTotalCustomers, totalCustomers.ToString("N0"), "in this tenant");

            // 2. Total Revenue (all-time, sum of AmountPaid)
            decimal totalRevenue = db.Transactions
                .Select(t => (decimal?)t.AmountPaid)
                .Sum() ?? 0m;
            SetCardValue(cardTotalRevenue, $"₱{totalRevenue:N0}", "all-time");

            // 3. Active Users
            int activeUsers = db.Users.Count(u => u.IsActive);
            int totalUsers = db.Users.Count();
            SetCardValue(cardActiveUsers, activeUsers.ToString("N0"), $"of {totalUsers} total users");

            // 4. Total Products (active)
            int totalProducts = db.Products.Count(p => p.IsActive);
            SetCardValue(cardTotalProducts, totalProducts.ToString("N0"), "in catalog");

            // 5. Average Order Value (guard against divide-by-zero)
            int ordersCount = db.Orders.Count();
            decimal avgOrderValue = ordersCount > 0 ? totalRevenue / ordersCount : 0m;
            SetCardValue(cardAvgOrderValue, $"₱{avgOrderValue:N2}", "per order");

            // 6. New Customers This Month
            int newCustomersMonth = db.Customers.Count(c =>
                c.CreatedAt.Year == now.Year &&
                c.CreatedAt.Month == now.Month);
            SetCardValue(cardNewCustomersMonth, newCustomersMonth.ToString("N0"), "this month");

            // 7. New Orders This Week (last 7 days)
            var weekAgo = now.AddDays(-7);
            int newOrdersWeek = db.Orders.Count(o => o.OrderDate >= weekAgo);
            SetCardValue(cardNewOrdersWeek, newOrdersWeek.ToString("N0"), "last 7 days");

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