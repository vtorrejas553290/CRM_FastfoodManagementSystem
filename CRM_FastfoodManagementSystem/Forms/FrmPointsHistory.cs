using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmPointsHistory : Form
{
    private readonly int _customerId;

    public FrmPointsHistory(int customerId)
    {
        _customerId = customerId;
        BuildUi();
        Load += FrmPointsHistory_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: false);
        Text = "Points History";
        ClientSize = new Size(800, 520);
    }

    private void FrmPointsHistory_Load(object? sender, EventArgs e)
    {
        using var db = AppServices.CreateTenantContext();

        var customer = db.Customers.AsNoTracking().FirstOrDefault(x => x.CustomerId == _customerId);
        if (customer is null) { Close(); return; }

        Text = $"Points History — {customer.CustomerName}";

        var lblTitle = new Label
        {
            Text = $"{customer.CustomerName} ({customer.CustomerCode})",
            Font = AppTheme.FontHeading,
            ForeColor = AppTheme.TextPrimary,
            Location = new Point(20, 20),
            AutoSize = true
        };
        Controls.Add(lblTitle);

        var lblBalance = new Label
        {
            Text = $"Current Balance: {customer.CurrentPoints:N0} points  =  ₱{customer.CurrentPoints / 100m:N2}",
            Font = AppTheme.FontSubheading,
            ForeColor = AppTheme.SuccessGreen,
            Location = new Point(20, 55),
            AutoSize = true
        };
        Controls.Add(lblBalance);

        var grid = new DataGridView
        {
            Location = new Point(20, 95),
            Size = new Size(760, 380),
            ReadOnly = true
        };
        AppTheme.StyleGrid(grid);

        var rows = db.CustomerPoints
            .Include(x => x.Order)
            .AsNoTracking()
            .Where(x => x.CustomerId == _customerId)
            .OrderByDescending(x => x.PerformedAt)
            .Select(x => new
            {
                x.PerformedAt,
                x.TransactionType,
                x.Points,
                Order = x.Order != null ? x.Order.OrderCode : "-",
                x.Notes
            })
            .ToList();

        grid.DataSource = rows;
        Controls.Add(grid);

        var btnClose = new Button
        {
            Text = "Close",
            Location = new Point(660, 480),
            Size = new Size(120, 34)
        };
        AppTheme.StyleNeutralButton(btnClose);
        btnClose.Click += (_, __) => Close();
        Controls.Add(btnClose);
    }
}