using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmContactCustomer : Form
{
    private readonly int _customerId;
    private string _phone = "";
    private string _email = "";
    private string _address = "";
    private string _customerName = "";

    private static readonly string[] Outcomes =
    {
        "(no outcome)",
        "Answered",
        "No answer",
        "Voicemail",
        "Wrong number",
        "Callback requested",
        "Email sent",
        "SMS sent"
    };

    public FrmContactCustomer(int customerId)
    {
        _customerId = customerId;
        InitializeComponent();
        ApplyTheme();
        Load += FrmContactCustomer_Load;

        btnCall.Click += (_, __) => OpenUri($"tel:{_phone}");
        btnSms.Click += (_, __) => OpenUri($"sms:{_phone}");
        btnEmail.Click += (_, __) => OpenEmailCompose();
        btnCopy.Click += (_, __) => CopyToClipboard();
        btnSaveAndClose.Click += (_, __) => SaveAndClose();
        btnClose.Click += (_, __) => Close();

        chkFollowUp.CheckedChanged += (_, __) =>
            dtpFollowUp.Enabled = chkFollowUp.Checked;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblCustomerName.ForeColor = AppTheme.TextPrimary;
        lblSubtitle.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[]
        {
            lblContactTitle, lblContextTitle, lblLogTitle, lblHistoryTitle
        })
            lbl.ForeColor = AppTheme.TextPrimary;

        foreach (var lbl in new[]
        {
            lblPhoneLabel, lblEmailLabel, lblAddressLabel,
            lblOutcome, lblNotes, lblFollowUp
        })
            lbl.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[]
        {
            lblPhone, lblEmail, lblAddress, lblLastOrder, lblPoints
        })
            lbl.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleSuccessButton(btnCall);
        AppTheme.StyleSecondaryButton(btnSms);
        AppTheme.StyleSecondaryButton(btnEmail);
        AppTheme.StyleSecondaryButton(btnCopy);
        AppTheme.StyleSuccessButton(btnSaveAndClose);
        AppTheme.StyleNeutralButton(btnClose);

        AppTheme.StyleInput(cmbOutcome);
        AppTheme.StyleInput(txtNotes);
        AppTheme.StyleInput(dtpFollowUp);

        chkFollowUp.ForeColor = AppTheme.TextPrimary;
        chkFollowUp.BackColor = System.Drawing.Color.Transparent;

        // Contact history grid — use the shared theme style
        AppTheme.StyleGrid(gridContactHistory);
    }

    private void FrmContactCustomer_Load(object? sender, EventArgs e)
    {
        // Populate outcome dropdown
        cmbOutcome.Items.Clear();
        cmbOutcome.Items.AddRange(Outcomes);
        cmbOutcome.SelectedIndex = 0;

        // Default follow-up date = one week out
        dtpFollowUp.Value = DateTime.Today.AddDays(7);

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
        _phone = customer.ContactNumber ?? "";
        _email = customer.EmailAddress ?? "";
        _address = customer.Address ?? "";

        lblCustomerName.Text = _customerName;
        lblPhone.Text = string.IsNullOrWhiteSpace(_phone) ? "—" : _phone;
        lblEmail.Text = string.IsNullOrWhiteSpace(_email) ? "—" : _email;
        lblAddress.Text = string.IsNullOrWhiteSpace(_address) ? "—" : _address;

        // Retention context
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
                        $"(worth ₱{customer.CurrentPoints / 100m:N2})";

        // Subtitle reflects retention status
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

        // Enable/disable buttons based on availability
        btnCall.Enabled = !string.IsNullOrWhiteSpace(_phone);
        btnSms.Enabled = !string.IsNullOrWhiteSpace(_phone);
        btnEmail.Enabled = !string.IsNullOrWhiteSpace(_email);

        // Load contact history
        LoadContactHistory(db);
    }

    private void LoadContactHistory(DbContext db)
    {
        // ActivityLog entries for this customer with ActionType = "Contact"
        // Skip entries with no real outcome — only show meaningful contact attempts.
        var raw = db.Set<CRM.domain.Entities.ActivityLog>()
            .AsNoTracking()
            .Where(a => a.EntityName == "Customer"
                     && a.EntityId == _customerId
                     && a.ActionType == "Contact"
                     && !a.Description.Contains("(no outcome)"))
            .OrderByDescending(a => a.PerformedAt)
            .Take(20)
            .ToList();

        var rows = raw.Select(a => new
        {
            Date = a.PerformedAt,
            Outcome = ExtractOutcome(a.Description),
            Notes = ExtractNotes(a.Description),
            By = string.IsNullOrWhiteSpace(a.Username) ? "—" : a.Username
        }).ToList();

        gridContactHistory.DataSource = rows;

        // Style columns
        if (gridContactHistory.Columns.Contains("Date"))
        {
            var c = gridContactHistory.Columns["Date"];
            c.HeaderText = "Date";
            c.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 150;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        if (gridContactHistory.Columns.Contains("Outcome"))
        {
            var c = gridContactHistory.Columns["Outcome"];
            c.HeaderText = "Outcome";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 130;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        if (gridContactHistory.Columns.Contains("Notes"))
        {
            var c = gridContactHistory.Columns["Notes"];
            c.HeaderText = "Notes";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c.MinimumWidth = 180;
            c.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        if (gridContactHistory.Columns.Contains("By"))
        {
            var c = gridContactHistory.Columns["By"];
            c.HeaderText = "By";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 130;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        // Row heights auto-fit wrapped notes
        gridContactHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        // Color the outcome cell
        gridContactHistory.CellFormatting -= GridHistory_CellFormatting;
        gridContactHistory.CellFormatting += GridHistory_CellFormatting;
    }

    private void GridHistory_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridContactHistory.Rows.Count) return;
        if (gridContactHistory.Columns[e.ColumnIndex].Name != "Outcome") return;

        var row = gridContactHistory.Rows[e.RowIndex];
        var outcome = row.Cells["Outcome"]?.Value?.ToString() ?? "";

        e.CellStyle.ForeColor = outcome switch
        {
            "Answered" => Color.FromArgb(22, 130, 60),
            "No answer" => Color.FromArgb(190, 40, 40),
            "Voicemail" => Color.FromArgb(200, 130, 0),
            "Wrong number" => Color.FromArgb(190, 40, 40),
            "Callback requested" => Color.FromArgb(30, 100, 200),
            "Email sent" => Color.FromArgb(30, 100, 200),
            "SMS sent" => Color.FromArgb(30, 100, 200),
            _ => AppTheme.TextSecondary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    // ============================================================
    //  PARSERS for ActivityLog.Description
    //  Format: "Contact with 'Juan Cruz': Answered — Interested in the promo [Follow-up: 2026-10-15]"
    // ============================================================

    private static string ExtractOutcome(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return "";

        var idx = description.IndexOf(": ", StringComparison.Ordinal);
        var tail = idx >= 0 ? description.Substring(idx + 2) : description;

        var dash = tail.IndexOf(" — ", StringComparison.Ordinal);
        if (dash > 0) return tail.Substring(0, dash).Trim();

        var bracket = tail.IndexOf(" [", StringComparison.Ordinal);
        if (bracket > 0) return tail.Substring(0, bracket).Trim();

        return tail.Trim();
    }

    private static string ExtractNotes(string? description)
    {
        if (string.IsNullOrWhiteSpace(description)) return "";

        var idx = description.IndexOf(": ", StringComparison.Ordinal);
        var tail = idx >= 0 ? description.Substring(idx + 2) : description;

        var dash = tail.IndexOf(" — ", StringComparison.Ordinal);
        if (dash >= 0) tail = tail.Substring(dash + 3);

        var bracket = tail.IndexOf(" [", StringComparison.Ordinal);
        if (bracket > 0) tail = tail.Substring(0, bracket);

        return tail.Trim();
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

    private void OpenEmailCompose()
    {
        if (string.IsNullOrWhiteSpace(_email))
        {
            MessageBox.Show("This customer has no email address on file.",
                "No email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dlg = new FrmSendEmail(_customerId, _customerName, _email);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            int idx = Array.IndexOf(Outcomes, "Email sent");
            if (idx >= 0) cmbOutcome.SelectedIndex = idx;
        }
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

    private void SaveAndClose()
    {
        var outcome = cmbOutcome.SelectedItem?.ToString() ?? "(no outcome)";
        var notes = txtNotes.Text.Trim();

        // Nothing to log — close without writing anything
        if (outcome == "(no outcome)" && string.IsNullOrWhiteSpace(notes))
        {
            var confirm = MessageBox.Show(
                "No outcome or notes entered. Close anyway?",
                "Nothing to log",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            // Just close — do NOT write anything to the ActivityLog.
            Close();
            return;
        }

        string description = $"{outcome}";
        if (!string.IsNullOrWhiteSpace(notes))
            description += $" — {notes}";
        if (chkFollowUp.Checked)
            description += $" [Follow-up: {dtpFollowUp.Value:yyyy-MM-dd}]";

        try
        {
            ActivityLogger.Log("Contact", "Customer", _customerId,
                $"Contact with '{_customerName}': {description}");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}