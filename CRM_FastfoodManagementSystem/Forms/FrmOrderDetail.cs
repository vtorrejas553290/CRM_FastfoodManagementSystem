using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmOrderDetail : Form
{
    private readonly int _orderId;

    // UI controls (created in code, no designer file needed)
    private Label lblOrderCode = null!;
    private Label lblCustomer = null!;
    private Label lblOrderType = null!;
    private Label lblStatus = null!;
    private Label lblOrderDate = null!;

    private DataGridView gridItems = null!;

    private Label lblSubTotal = null!;
    private Label lblDiscount = null!;
    private Label lblTotal = null!;

    private Label lblPayMethod = null!;
    private Label lblPayAmount = null!;
    private Label lblPayChange = null!;
    private Label lblPayReference = null!;
    private Label lblPayPaidAt = null!;
    private Label lblUnpaid = null!;

    private Panel pnlPayment = null!;

    public FrmOrderDetail(int orderId)
    {
        _orderId = orderId;
        BuildUi();
        Load += FrmOrderDetail_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = "Order Details";
        ClientSize = new Size(720, 720);
        MinimumSize = new Size(640, 560);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = false;
        MinimizeBox = false;
        Padding = new Padding(16);

        // ============================================================
        // ROOT: three rows — Header | Items (fills) | Totals+Payment
        // ============================================================
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            BackColor = Color.Transparent
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // header card
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // "Items" title
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // grid (fills)
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // totals + payment
        Controls.Add(root);

        // ============================================================
        // 1) HEADER CARD — order code + metadata in a 2-col table
        // ============================================================
        var headerCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            Padding = new Padding(16, 12, 16, 12),
            Margin = new Padding(0, 0, 0, 12),
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };

        var headerGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            RowCount = 5,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = Color.Transparent
        };
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 5; i++)
            headerGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // Order code spans both columns, bigger font
        lblOrderCode = new Label
        {
            Text = "Order Code:",
            AutoSize = true,
            Font = AppTheme.FontHeading,
            ForeColor = AppTheme.TextPrimary,
            Margin = new Padding(0, 0, 0, 8)
        };
        headerGrid.Controls.Add(lblOrderCode, 0, 0);
        headerGrid.SetColumnSpan(lblOrderCode, 2);

        lblCustomer = MakeMetaLabel();
        lblOrderType = MakeMetaLabel();
        lblStatus = MakeMetaLabel();
        lblOrderDate = MakeMetaLabel();

        AddMetaRow(headerGrid, 1, "Customer", lblCustomer);
        AddMetaRow(headerGrid, 2, "Order Type", lblOrderType);
        AddMetaRow(headerGrid, 3, "Status", lblStatus);
        AddMetaRow(headerGrid, 4, "Order Date", lblOrderDate);

        headerCard.Controls.Add(headerGrid);
        root.Controls.Add(headerCard, 0, 0);

        // ============================================================
        // 2) ITEMS TITLE
        // ============================================================
        var lblItemsTitle = new Label
        {
            Text = "Items",
            AutoSize = true,
            Font = AppTheme.FontSubheading,
            ForeColor = AppTheme.TextPrimary,
            Margin = new Padding(0, 0, 0, 6)
        };
        root.Controls.Add(lblItemsTitle, 0, 1);

        // ============================================================
        // 3) ITEMS GRID — fills remaining vertical space
        // ============================================================
        gridItems = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = AppTheme.Surface,
            BorderStyle = BorderStyle.None,
            Margin = new Padding(0, 0, 0, 12)
        };
        AppTheme.StyleGrid(gridItems);

        // Make Quantity / UnitPrice / LineTotal right-aligned and narrower,
        // and Product fill the rest.
        gridItems.DataBindingComplete += (_, __) =>
        {
            if (gridItems.Columns.Contains("Quantity"))
            {
                gridItems.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                gridItems.Columns["Quantity"].Width = 70;
                gridItems.Columns["Quantity"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }
            if (gridItems.Columns.Contains("UnitPrice"))
            {
                gridItems.Columns["UnitPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                gridItems.Columns["UnitPrice"].Width = 110;
                gridItems.Columns["UnitPrice"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                gridItems.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            }
            if (gridItems.Columns.Contains("LineTotal"))
            {
                gridItems.Columns["LineTotal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                gridItems.Columns["LineTotal"].Width = 120;
                gridItems.Columns["LineTotal"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                gridItems.Columns["LineTotal"].DefaultCellStyle.Format = "N2";
            }
            if (gridItems.Columns.Contains("Product"))
            {
                gridItems.Columns["Product"].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
            }
        };

        root.Controls.Add(gridItems, 0, 2);

        // ============================================================
        // 4) BOTTOM: Totals (left) + Payment (right) side-by-side
        // ============================================================
        var bottom = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(0, 4, 0, 12),
            BackColor = Color.Transparent
        };
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        // ---- Totals card ----
        var totalsCard = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            Padding = new Padding(16, 12, 16, 12),
            Margin = new Padding(0, 0, 6, 0),
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };

        var totalsGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            RowCount = 3,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = Color.Transparent
        };
        totalsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        totalsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        for (int i = 0; i < 3; i++)
            totalsGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        totalsGrid.Controls.Add(MakeFieldLabel("SubTotal"), 0, 0);
        lblSubTotal = MakeValueLabel(rightAligned: true);
        totalsGrid.Controls.Add(lblSubTotal, 1, 0);

        totalsGrid.Controls.Add(MakeFieldLabel("Discount"), 0, 1);
        lblDiscount = MakeValueLabel(rightAligned: true);
        totalsGrid.Controls.Add(lblDiscount, 1, 1);

        totalsGrid.Controls.Add(MakeFieldLabel("Total", bold: true), 0, 2);
        lblTotal = MakeValueLabel(rightAligned: true, bold: true, color: AppTheme.SuccessGreen);
        totalsGrid.Controls.Add(lblTotal, 1, 2);

        totalsCard.Controls.Add(totalsGrid);
        bottom.Controls.Add(totalsCard, 0, 0);

        // ---- Payment card ----
        pnlPayment = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            Padding = new Padding(16, 12, 16, 12),
            Margin = new Padding(6, 0, 0, 0),
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };

        var payGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            RowCount = 6,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = Color.Transparent
        };
        payGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        payGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        for (int i = 0; i < 6; i++)
            payGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        payGrid.Controls.Add(MakeFieldLabel("Method"), 0, 0);
        lblPayMethod = MakeValueLabel();
        payGrid.Controls.Add(lblPayMethod, 1, 0);

        payGrid.Controls.Add(MakeFieldLabel("Amount Paid"), 0, 1);
        lblPayAmount = MakeValueLabel(rightAligned: true);
        payGrid.Controls.Add(lblPayAmount, 1, 1);

        payGrid.Controls.Add(MakeFieldLabel("Change"), 0, 2);
        lblPayChange = MakeValueLabel(rightAligned: true);
        payGrid.Controls.Add(lblPayChange, 1, 2);

        payGrid.Controls.Add(MakeFieldLabel("Reference"), 0, 3);
        lblPayReference = MakeValueLabel();
        payGrid.Controls.Add(lblPayReference, 1, 3);

        payGrid.Controls.Add(MakeFieldLabel("Paid At"), 0, 4);
        lblPayPaidAt = MakeValueLabel();
        payGrid.Controls.Add(lblPayPaidAt, 1, 4);

        lblUnpaid = new Label
        {
            Text = "(unpaid)",
            AutoSize = true,
            Font = AppTheme.FontBody,
            ForeColor = AppTheme.Danger,
            Margin = new Padding(0)
        };
        payGrid.Controls.Add(lblUnpaid, 0, 5);
        payGrid.SetColumnSpan(lblUnpaid, 2);

        pnlPayment.Controls.Add(payGrid);
        bottom.Controls.Add(pnlPayment, 1, 0);

        root.Controls.Add(bottom, 0, 3);

        // ============================================================
        // CLOSE BUTTON — docked bottom of the form (outside root)
        // ============================================================
        var pnlButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 56,
            Padding = new Padding(0, 8, 0, 0),
            BackColor = Color.Transparent
        };

        var btnClose = new Button
        {
            Text = "Close",
            Width = 120,
            Height = 36,
            FlatStyle = FlatStyle.Flat
        };
        AppTheme.StyleNeutralButton(btnClose);
        btnClose.Click += (_, __) => Close();
        pnlButtons.Controls.Add(btnClose);

        // Add button panel after root (Dock.Bottom fills below the fill panel)
        Controls.Add(pnlButtons);
        pnlButtons.BringToFront();
    }

    private void FrmOrderDetail_Load(object? sender, EventArgs e)
    {
        using var db = AppServices.CreateTenantContext();

        var order = db.Orders
            .Include(x => x.Items).ThenInclude(i => i.Product)
            .Include(x => x.Customer)
            .Include(x => x.Transaction)
            .AsNoTracking()
            .FirstOrDefault(x => x.OrderId == _orderId);

        if (order is null)
        {
            MessageBox.Show("Order not found.", "Error");
            Close();
            return;
        }

        // ---- Header ----
        lblOrderCode.Text = $"Order {order.OrderCode}";
        lblCustomer.Text = order.Customer?.CustomerName ?? "(deleted)";
        lblOrderType.Text = order.OrderType;
        lblStatus.Text = order.Status;
        lblOrderDate.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm");

        // ---- Items ----
        gridItems.DataSource = order.Items.Select(i => new
        {
            Product = i.Product?.ProductName ?? "(unknown)",
            i.Quantity,
            UnitPrice = i.UnitPrice,
            LineTotal = i.LineTotal
        }).ToList();

        // ---- Totals ----
        lblSubTotal.Text = $"₱{order.SubTotal:N2}";
        decimal discount = order.SubTotal - order.TotalAmount;
        lblDiscount.Text = discount > 0 ? $"- ₱{discount:N2}" : "₱0.00";
        lblTotal.Text = $"₱{order.TotalAmount:N2}";

        // ---- Payment ----
        if (order.Transaction is not null)
        {
            lblPayMethod.Text = order.Transaction.PaymentMethod;
            lblPayAmount.Text = $"₱{order.Transaction.AmountPaid:N2}";
            lblPayChange.Text = $"₱{order.Transaction.ChangeDue:N2}";
            lblPayReference.Text = string.IsNullOrEmpty(order.Transaction.ReferenceNumber)
                ? "-"
                : order.Transaction.ReferenceNumber;
            lblPayPaidAt.Text = order.Transaction.PaidAt.ToString("yyyy-MM-dd HH:mm");

            lblUnpaid.Visible = false;
        }
        else
        {
            lblPayMethod.Text = "—";
            lblPayAmount.Text = "—";
            lblPayChange.Text = "—";
            lblPayReference.Text = "—";
            lblPayPaidAt.Text = "—";

            lblUnpaid.Visible = true;
        }
    }

    // ============================================================
    // HELPERS
    // ============================================================

    private Label MakeMetaLabel() => new Label
    {
        Text = "—",
        AutoSize = true,
        Font = AppTheme.FontBody,
        ForeColor = AppTheme.TextPrimary,
        Margin = new Padding(0, 2, 0, 2)
    };

    private Label MakeFieldLabel(string text, bool bold = false) => new Label
    {
        Text = text,
        AutoSize = true,
        Font = bold ? AppTheme.FontSubheading : AppTheme.FontBody,
        ForeColor = AppTheme.TextSecondary,
        Margin = new Padding(0, 2, 12, 2),
        TextAlign = ContentAlignment.MiddleLeft
    };

    private Label MakeValueLabel(bool rightAligned = false, bool bold = false, Color? color = null) =>
        new Label
        {
            Text = "—",
            AutoSize = true,
            Font = bold ? AppTheme.FontSubheading : AppTheme.FontBody,
            ForeColor = color ?? AppTheme.TextPrimary,
            Margin = new Padding(0, 2, 0, 2),
            TextAlign = rightAligned
                ? ContentAlignment.MiddleRight
                : ContentAlignment.MiddleLeft
        };

    private void AddMetaRow(TableLayoutPanel grid, int row, string label, Label valueLabel)
    {
        var lbl = MakeFieldLabel(label);
        grid.Controls.Add(lbl, 0, row);
        grid.Controls.Add(valueLabel, 1, row);
    }
}