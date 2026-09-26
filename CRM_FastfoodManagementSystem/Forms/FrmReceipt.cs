using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmReceipt : Form
{
    private readonly string _orderCode;
    private readonly string _customerName;
    private readonly string _orderType;
    private readonly string _paymentMethod;
    private readonly string _reference;
    private readonly decimal _subTotal;
    private readonly decimal _discount;
    private readonly decimal _total;
    private readonly decimal _paid;
    private readonly decimal _change;
    private readonly int _pointsRedeemed;
    private readonly int _pointsEarned;
    private readonly int _newPointsBalance;
    private readonly List<OrderItem> _items;
    private readonly DateTime _paidAt;

    public FrmReceipt(
        string orderCode,
        string customerName,
        string orderType,
        string paymentMethod,
        string reference,
        decimal subTotal,
        decimal discount,
        decimal total,
        decimal paid,
        decimal change,
        int pointsRedeemed,
        int pointsEarned,
        int newPointsBalance,
        List<OrderItem> items,
        DateTime paidAt)
    {
        _orderCode = orderCode;
        _customerName = customerName;
        _orderType = orderType;
        _paymentMethod = paymentMethod;
        _reference = reference;
        _subTotal = subTotal;
        _discount = discount;
        _total = total;
        _paid = paid;
        _change = change;
        _pointsRedeemed = pointsRedeemed;
        _pointsEarned = pointsEarned;
        _newPointsBalance = newPointsBalance;
        _items = items;
        _paidAt = paidAt;

        InitializeComponent();
        ApplyTheme();
        RenderReceipt();

        btnClose.Click += (_, __) => { DialogResult = DialogResult.OK; Close(); };
        btnPrint.Click += (_, __) => PrintReceipt();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlFooter.BackColor = AppTheme.Surface;
        pnlScroll.BackColor = AppTheme.ContentSurface;

        AppTheme.StyleSuccessButton(btnPrint);
        AppTheme.StyleSecondaryButton(btnClose);

        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.ForeColor = AppTheme.TextPrimary;
        lblSubtitle.Font = new Font("Segoe UI", 8.5F);
        lblSubtitle.ForeColor = AppTheme.TextSecondary;
    }

    private void RenderReceipt()
    {
        // Build the whole receipt as one string so the label can auto-size.
        // Fixed-width: 42 characters wide, monospaced font keeps it aligned.
        const int LineWidth = 42;

        var sb = new System.Text.StringBuilder();

        // ---- Header ----
        sb.AppendLine(Center("CRM FastFood", LineWidth));
        sb.AppendLine(Center("— — — — — — — — — —", LineWidth));
        sb.AppendLine($"Order    : {_orderCode}");
        sb.AppendLine($"Customer : {_customerName}");
        sb.AppendLine($"Type     : {_orderType}");
        sb.AppendLine($"Date     : {_paidAt:yyyy-MM-dd HH:mm}");
        sb.AppendLine();

        // ---- Column headers ----
        sb.AppendLine("QTY  ITEM                  PRICE      TOTAL");
        sb.AppendLine(new string('-', LineWidth));

        // ---- Item lines ----
        foreach (var item in _items)
        {
            string name = item.Product != null
                ? item.Product.ProductName
                : ProductNameLookup(item.ProductId);

            string nameCol = FitRight(name, 18);
            string qtyCol = item.Quantity.ToString().PadLeft(3);
            string priceCol = item.UnitPrice.ToString("N2").PadLeft(10);
            string totalCol = item.LineTotal.ToString("N2").PadLeft(10);

            sb.AppendLine($"{qtyCol}  {nameCol}  {priceCol} {totalCol}");
        }

        sb.AppendLine(new string('-', LineWidth));

        // ---- Totals ----
        sb.AppendLine($"{"SubTotal:",-30}{_subTotal,12:N2}");

        if (_discount > 0)
            sb.AppendLine($"{"Discount:",-30}{-_discount,12:N2}");

        sb.AppendLine($"{"TOTAL:",-30}{_total,12:N2}");
        sb.AppendLine($"{"Paid (" + _paymentMethod + "):",-30}{_paid,12:N2}");
        sb.AppendLine($"{"Change:",-30}{_change,12:N2}");

        if (!string.IsNullOrWhiteSpace(_reference))
            sb.AppendLine($"Reference: {_reference}");

        // ---- Loyalty ----
        if (_pointsRedeemed > 0 || _pointsEarned > 0)
        {
            sb.AppendLine();
            sb.AppendLine("Loyalty");
            if (_pointsRedeemed > 0)
                sb.AppendLine($"  Redeemed: -{_pointsRedeemed:N0} pts");
            if (_pointsEarned > 0)
                sb.AppendLine($"  Earned:   +{_pointsEarned:N0} pts");
            sb.AppendLine($"  Balance:   {_newPointsBalance:N0} pts");
        }

        // ---- Footer ----
        sb.AppendLine();
        sb.AppendLine(new string('-', LineWidth));
        sb.AppendLine(Center("Thank you! Please come again.", LineWidth));

        lblReceiptText.Text = sb.ToString();
    }

    private static string Center(string s, int width)
    {
        if (s.Length >= width) return s;
        int pad = (width - s.Length) / 2;
        return new string(' ', pad) + s;
    }

    private static string FitRight(string s, int width)
    {
        if (string.IsNullOrEmpty(s)) return new string(' ', width);
        if (s.Length > width) return s.Substring(0, width - 1) + "…";
        return s.PadRight(width);
    }

    private static string ProductNameLookup(int productId)
    {
        try
        {
            using var db = CRM.infrastructure.AppServices.CreateTenantContext();
            var p = db.Products
                .AsNoTracking()
                .FirstOrDefault(x => x.ProductId == productId);
            return p?.ProductName ?? $"Item #{productId}";
        }
        catch
        {
            return $"Item #{productId}";
        }
    }

    private void PrintReceipt()
    {
        var pd = new System.Drawing.Printing.PrintDocument();
        pd.DocumentName = $"Receipt {_orderCode}";

        pd.PrintPage += (s, e) =>
        {
            var g = e.Graphics;
            if (g is null) return;

            using var font = new Font("Consolas", 10F);
            using var brush = new SolidBrush(Color.Black);

            g.DrawString(lblReceiptText.Text, font, brush, 60, 60);
        };

        using var preview = new PrintPreviewDialog
        {
            Document = pd,
            Width = 900,
            Height = 700,
            StartPosition = FormStartPosition.CenterParent
        };
        preview.ShowDialog(this);
    }
}