using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmAdjustPoints : Form
{
    private readonly int _customerId;

    private Label _lblCustomer = new();
    private Label _lblBalance = new();
    private ComboBox _cmbType = new();
    private NumericUpDown _numPoints = new();
    private TextBox _txtNotes = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

    public FrmAdjustPoints(int customerId)
    {
        _customerId = customerId;
        BuildUi();
        Load += FrmAdjustPoints_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = "Adjust Points";
        ClientSize = new Size(520, 400);

        _lblCustomer = new Label { Location = new Point(20, 20), AutoSize = true, Font = AppTheme.FontHeading };
        _lblCustomer.ForeColor = AppTheme.TextPrimary;

        _lblBalance = new Label { Location = new Point(20, 55), AutoSize = true, Font = AppTheme.FontSubheading };
        _lblBalance.ForeColor = AppTheme.SuccessGreen;

        var lblType = new Label { Text = "Type:", Location = new Point(20, 100), AutoSize = true };
        AppTheme.StyleLabel(lblType);
        _cmbType = new ComboBox { Location = new Point(140, 98), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
        AppTheme.StyleInput(_cmbType);
        _cmbType.Items.AddRange(new object[] { "Bonus", "Correction" });
        _cmbType.SelectedIndex = 0;

        var lblPoints = new Label { Text = "Points:", Location = new Point(20, 145), AutoSize = true };
        AppTheme.StyleLabel(lblPoints);
        _numPoints = new NumericUpDown
        {
            Location = new Point(140, 143),
            Width = 200,
            Minimum = -1_000_000,
            Maximum = 1_000_000,
            DecimalPlaces = 0
        };
        AppTheme.StyleInput(_numPoints);

        var lblHint = new Label
        {
            Text = "(use negative to deduct)",
            Location = new Point(350, 147),
            AutoSize = true,
            Font = AppTheme.FontSmall,
            ForeColor = AppTheme.TextMuted
        };

        var lblNotes = new Label { Text = "Reason:", Location = new Point(20, 190), AutoSize = true };
        AppTheme.StyleLabel(lblNotes);
        _txtNotes = new TextBox { Location = new Point(140, 188), Width = 340, Height = 80, Multiline = true };
        AppTheme.StyleInput(_txtNotes);

        _btnSave = new Button { Text = "Save", Location = new Point(140, 290), Width = 160 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(320, 290), Width = 160 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label { Location = new Point(20, 340), AutoSize = true, MaximumSize = new Size(460, 40) };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[]
        {
            _lblCustomer, _lblBalance,
            lblType, _cmbType,
            lblPoints, _numPoints, lblHint,
            lblNotes, _txtNotes,
            _btnSave, _btnCancel, _lblStatus
        });
    }

    private void FrmAdjustPoints_Load(object? sender, EventArgs e)
    {
        using var db = AppServices.CreateTenantContext();
        var customer = db.Customers.AsNoTracking().FirstOrDefault(x => x.CustomerId == _customerId);
        if (customer is null) { Close(); return; }

        _lblCustomer.Text = customer.CustomerName;
        _lblBalance.Text = $"Current Balance: {customer.CurrentPoints:N0} points";
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        if (_numPoints.Value == 0)
        {
            _lblStatus.Text = "Points adjustment cannot be zero.";
            return;
        }

        if (string.IsNullOrWhiteSpace(_txtNotes.Text) || _txtNotes.Text.Trim().Length < 3)
        {
            _lblStatus.Text = "Please provide a reason (min 3 characters).";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == _customerId);
            if (customer is null) { _lblStatus.Text = "Customer not found."; return; }

            int delta = (int)_numPoints.Value;

            if (customer.CurrentPoints + delta < 0)
            {
                _lblStatus.Text = "Adjustment would make balance negative.";
                return;
            }

            customer.CurrentPoints += delta;

            db.CustomerPoints.Add(new CustomerPoint
            {
                CustomerId = _customerId,
                TransactionType = _cmbType.SelectedItem?.ToString() ?? "Correction",
                Points = delta,
                Notes = _txtNotes.Text.Trim(),
                PerformedByUserId = UserSession.UserId,
                PerformedAt = DateTime.UtcNow
            });

            db.SaveChanges();

            ActivityLogger.Log("Points", "Customer", _customerId,
                $"{(delta >= 0 ? "+" : "")}{delta} points — {_txtNotes.Text.Trim()}");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = ex.Message;
        }
    }
}