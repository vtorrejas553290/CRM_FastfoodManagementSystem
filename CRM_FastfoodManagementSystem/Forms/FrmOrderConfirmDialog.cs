using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmOrderConfirmDialog : Form
{
    private readonly string _customerName;
    private readonly string _orderType;
    private readonly List<OrderItem> _items;
    private readonly decimal _total;
    private readonly string _method;
    private readonly decimal _paid;
    private readonly decimal _change;
    private readonly string _reference;

    private Button _btnConfirm = new();
    private Button _btnCancel = new();

    public FrmOrderConfirmDialog(
        string customerName,
        string orderType,
        List<OrderItem> items,
        decimal total,
        string method,
        decimal paid,
        decimal change,
        string reference)
    {
        _customerName = customerName;
        _orderType = orderType;
        _items = items;
        _total = total;
        _method = method;
        _paid = paid;
        _change = change;
        _reference = reference;

        BuildUi();
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = "Confirm Order";
        ClientSize = new Size(560, 620);

        int y = 20;

        // Title
        var lblTitle = new Label
        {
            Text = "Please confirm the order details",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = AppTheme.TextPrimary,
            Location = new Point(20, y),
            AutoSize = true
        };
        Controls.Add(lblTitle);
        y += 45;

        // Customer + Order Type
        AddRow("Customer:", _customerName, ref y);
        AddRow("Order Type:", _orderType, ref y);
        y += 10;

        // Items grid
        var lblItems = new Label
        {
            Text = "Items:",
            Font = AppTheme.FontSubheading,
            ForeColor = AppTheme.TextPrimary,
            Location = new Point(20, y),
            AutoSize = true
        };
        Controls.Add(lblItems);
        y += 28;

        var grid = new DataGridView
        {
            Location = new Point(20, y),
            Size = new Size(520, 200),
            ReadOnly = true,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = AppTheme.Surface,
            BorderStyle = BorderStyle.None
        };
        AppTheme.StyleGrid(grid);

        using (var db = AppServices.CreateTenantContext())
        {
            grid.DataSource = _items.Select(i => new
            {
                Item = db.Products.AsNoTracking()
                    .FirstOrDefault(p => p.ProductId == i.ProductId)?.ProductName ?? "(unknown)",
                Qty = i.Quantity,
                Price = $"₱{i.UnitPrice:N2}",
                Total = $"₱{i.LineTotal:N2}"
            }).ToList();
        }

        Controls.Add(grid);
        y += 210;

        // Totals
        AddRow("Total:", $"₱{_total:N2}", ref y, AppTheme.FontHeading, AppTheme.SuccessGreen);
        AddRow("Payment:", _method, ref y);
        if (_method == "GCash" && !string.IsNullOrEmpty(_reference))
        {
            AddRow("GCash Ref:", _reference, ref y);
        }
        AddRow("Amount Paid:", $"₱{_paid:N2}", ref y);
        AddRow("Change:", $"₱{_change:N2}", ref y, AppTheme.FontSubheading, AppTheme.SuccessGreen);
        y += 10;

        // Buttons
        _btnConfirm = new Button
        {
            Text = "Confirm Payment",
            Location = new Point(20, ClientSize.Height - 60),
            Size = new Size(260, 42)
        };
        AppTheme.StyleSuccessButton(_btnConfirm);
        _btnConfirm.Click += (_, __) => { DialogResult = DialogResult.OK; Close(); };

        _btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(290, ClientSize.Height - 60),
            Size = new Size(250, 42)
        };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        Controls.Add(_btnConfirm);
        Controls.Add(_btnCancel);
    }

    private void AddRow(string label, string value, ref int y,
        Font? valueFont = null, Color? valueColor = null)
    {
        var lbl = new Label
        {
            Text = label,
            Font = AppTheme.FontLabel,
            ForeColor = AppTheme.TextSecondary,
            Location = new Point(20, y),
            AutoSize = true
        };

        var val = new Label
        {
            Text = value,
            Font = valueFont ?? AppTheme.FontBody,
            ForeColor = valueColor ?? AppTheme.TextPrimary,
            Location = new Point(160, y),
            AutoSize = true
        };

        Controls.Add(lbl);
        Controls.Add(val);

        y += (valueFont == AppTheme.FontHeading ? 32 : 25);
    }
}