using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CRM.domain.Models;
using CRM.infrastructure;
using CRM.infrastructure.Data;

namespace CRM.winForms.Forms;

public partial class FrmContactCustomer : Form
{
    private readonly int _customerId;
    private string _phone = "";
    private string _email = "";
    private string _address = "";
    private string _customerName = "";
    private string _customerCode = "";

    public FrmContactCustomer(int customerId)
    {
        _customerId = customerId;
        InitializeComponent();
        ApplyTheme();

        Load += FrmContactCustomer_Load;
        Load += (_, __) => FitToScreen();

        btnSms.Click += (_, __) => OpenSmsCompose();
        btnEmail.Click += (_, __) => OpenEmailCompose();
        btnCopy.Click += (_, __) => CopyToClipboard();
        btnClose.Click += (_, __) => Close();
    }

    private void FitToScreen()
    {
        var wa = Screen.FromControl(this).WorkingArea;

        int maxW = wa.Width - 60;
        int maxH = wa.Height - 60;

        int minW = Math.Min(MinimumSize.Width, maxW);
        int minH = Math.Min(MinimumSize.Height, maxH);

        int w = Math.Clamp(Width, minW, maxW);
        int h = Math.Clamp(Height, minH, maxH);

        Size = new Size(w, h);

        StartPosition = FormStartPosition.Manual;
        Location = new Point(
            wa.Left + (wa.Width - Width) / 2,
            wa.Top + (wa.Height - Height) / 2);
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblCustomerName.ForeColor = AppTheme.TextPrimary;
        lblSubtitle.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[] { lblContactTitle, lblContextTitle })
            lbl.ForeColor = AppTheme.TextPrimary;

        foreach (var lbl in new[]
        {
            lblPhoneLabel, lblEmailLabel, lblAddressLabel
        })
            AppTheme.StyleLabel(lbl);

        foreach (var lbl in new[] { lblPhone, lblEmail, lblAddress, lblLastOrder, lblPoints })
            lbl.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleSuccessButton(btnSms);
        AppTheme.StyleSecondaryButton(btnEmail);
        AppTheme.StyleSecondaryButton(btnCopy);
        AppTheme.StyleNeutralButton(btnClose);
    }

    private void FrmContactCustomer_Load(object? sender, EventArgs e)
    {
        using var db = AppServices.CreateTenantContext();

        var customer = db.Customers
            .AsNoTracking()
            .FirstOrDefault(c => c.CustomerId == _customerId);

        if (customer is null)
        {
            MessageBox.Show("Customer not found.", "Error");
            Close();
            return;
        }

        _customerName = customer.CustomerName;
        _customerCode = customer.CustomerCode ?? "";
        _phone = customer.ContactNumber ?? "";
        _email = customer.EmailAddress ?? "";
        _address = customer.Address ?? "";

        lblCustomerName.Text = _customerName;
        lblPhone.Text = string.IsNullOrWhiteSpace(_phone) ? "—" : _phone;
        lblEmail.Text = string.IsNullOrWhiteSpace(_email) ? "—" : _email;
        lblAddress.Text = string.IsNullOrWhiteSpace(_address) ? "—" : _address;

        var lastOrder = db.Orders
            .Where(o => o.CustomerId == _customerId && o.Status != "Cancelled")
            .OrderByDescending(o => o.OrderDate)
            .Select(o => (DateTime?)o.OrderDate)
            .FirstOrDefault();

        if (lastOrder.HasValue)
        {
            int days = (int)(DateTime.UtcNow - lastOrder.Value).TotalDays;
            lblLastOrder.Text = $"Last order: {lastOrder.Value:yyyy-MM-dd} ({days} days ago)";
        }
        else
        {
            lblLastOrder.Text = "Last order: Never";
        }

        lblPoints.Text = $"Loyalty points: {customer.CurrentPoints:N0} " +
                        $"(worth ₱{LoyaltyConfig.PesosForPoints(customer.CurrentPoints):N2})";

        string retention;
        if (!lastOrder.HasValue) retention = "Never ordered";
        else
        {
            int d = (int)(DateTime.UtcNow - lastOrder.Value).TotalDays;
            retention = d <= 30 ? "Active"
                : d <= 60 ? "At Risk"
                : "Dormant";
        }
        lblSubtitle.Text = $"Retention: {retention}";

        btnSms.Enabled = !string.IsNullOrWhiteSpace(_phone);
        btnEmail.Enabled = !string.IsNullOrWhiteSpace(_email);
    }

    // ============================================================
    //  ACTIONS
    // ============================================================

    private void OpenUri(string uri)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = uri,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not open:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OpenSmsCompose()
    {
        if (string.IsNullOrWhiteSpace(_phone))
        {
            MessageBox.Show("This customer has no phone number on file.",
                "No phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dlg = new FrmSendSms(_customerId, _customerName, _customerCode, _phone);
        dlg.ShowDialog(this);
    }

    private void OpenEmailCompose()
    {
        if (string.IsNullOrWhiteSpace(_email))
        {
            MessageBox.Show("This customer has no email address on file.",
                "No email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dlg = new FrmSendEmail(_customerId, _customerName, _customerCode, _email);
        dlg.ShowDialog(this);
    }

    private void CopyToClipboard()
    {
        var lines = new List<string> { _customerName };
        if (!string.IsNullOrWhiteSpace(_phone)) lines.Add($"Phone: {_phone}");
        if (!string.IsNullOrWhiteSpace(_email)) lines.Add($"Email: {_email}");
        if (!string.IsNullOrWhiteSpace(_address)) lines.Add($"Address: {_address}");

        Clipboard.SetText(string.Join(Environment.NewLine, lines));
        MessageBox.Show("Contact info copied to clipboard.",
            "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}