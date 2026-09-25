using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmPaymentDialog : Form
{
    private readonly int _orderId;
    private decimal _total;

    private Label _lblOrder = new();
    private Label _lblTotal = new();
    private Label _lblMethod = new();
    private ComboBox _cmbMethod = new();
    private Label _lblReference = new();
    private TextBox _txtReference = new();
    private Label _lblPaid = new();
    private NumericUpDown _numPaid = new();
    private Label _lblChangeLabel = new();
    private Label _lblChange = new();
    private Button _btnConfirm = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

    public FrmPaymentDialog(int orderId)
    {
        _orderId = orderId;
        BuildUi();
        Load += FrmPaymentDialog_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = "Payment";
        ClientSize = new Size(520, 420);

        _lblOrder = new Label { Location = new Point(20, 20), AutoSize = true, Font = AppTheme.FontBody };
        _lblOrder.ForeColor = AppTheme.TextPrimary;

        _lblTotal = new Label { Location = new Point(20, 55), AutoSize = true, Font = AppTheme.FontTitle };
        _lblTotal.ForeColor = AppTheme.SuccessGreen;

        _lblMethod = new Label { Text = "Payment Method:", Location = new Point(20, 110), AutoSize = true };
        AppTheme.StyleLabel(_lblMethod);
        _cmbMethod = new ComboBox
        {
            Location = new Point(160, 108),
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        AppTheme.StyleInput(_cmbMethod);
        _cmbMethod.SelectedIndexChanged += (_, __) => UpdateUi();

        _lblReference = new Label { Text = "GCash Ref No:", Location = new Point(20, 150), AutoSize = true };
        AppTheme.StyleLabel(_lblReference);
        _txtReference = new TextBox { Location = new Point(160, 148), Width = 300 };
        AppTheme.StyleInput(_txtReference);

        _lblPaid = new Label { Text = "Amount Paid:", Location = new Point(20, 190), AutoSize = true };
        AppTheme.StyleLabel(_lblPaid);
        _numPaid = new NumericUpDown
        {
            Location = new Point(160, 188),
            Width = 200,
            DecimalPlaces = 2,
            Maximum = 1_000_000
        };
        AppTheme.StyleInput(_numPaid);
        _numPaid.ValueChanged += (_, __) => UpdateChange();

        _lblChangeLabel = new Label { Text = "Change:", Location = new Point(20, 230), AutoSize = true };
        AppTheme.StyleLabel(_lblChangeLabel);
        _lblChange = new Label
        {
            Location = new Point(160, 230),
            AutoSize = true,
            Font = AppTheme.FontSubheading
        };
        _lblChange.ForeColor = AppTheme.SuccessGreen;

        _btnConfirm = new Button { Text = "Confirm Payment", Location = new Point(160, 290), Width = 200 };
        AppTheme.StyleSuccessButton(_btnConfirm);
        _btnConfirm.Click += BtnConfirm_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(370, 290), Width = 100 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label { Location = new Point(20, 340), AutoSize = true, MaximumSize = new Size(480, 40) };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[]
        {
            _lblOrder, _lblTotal,
            _lblMethod, _cmbMethod,
            _lblReference, _txtReference,
            _lblPaid, _numPaid,
            _lblChangeLabel, _lblChange,
            _btnConfirm, _btnCancel, _lblStatus
        });
    }

    private void FrmPaymentDialog_Load(object? sender, EventArgs e)
    {
        _cmbMethod.Items.Clear();
        _cmbMethod.Items.AddRange(new object[] { "Cash", "GCash" });
        _cmbMethod.SelectedIndex = 0;

        using var db = AppServices.CreateTenantContext();
        var order = db.Orders.AsNoTracking().FirstOrDefault(x => x.OrderId == _orderId);

        if (order is null)
        {
            MessageBox.Show("Order not found.", "Error");
            Close();
            return;
        }

        _total = order.TotalAmount;
        _lblOrder.Text = $"Order: {order.OrderCode}";
        _lblTotal.Text = $"Total: ₱{_total:N2}";

        _numPaid.Value = _total;
        UpdateUi();
        UpdateChange();
    }

    private void UpdateUi()
    {
        var method = _cmbMethod.SelectedItem?.ToString() ?? "Cash";
        bool isGcash = method == "GCash";

        _lblReference.Visible = isGcash;
        _txtReference.Visible = isGcash;
    }

    private void UpdateChange()
    {
        decimal paid = _numPaid.Value;
        decimal change = paid - _total;

        if (change < 0)
        {
            _lblChange.Text = $"Short by ₱{Math.Abs(change):N2}";
            _lblChange.ForeColor = AppTheme.Danger;
        }
        else
        {
            _lblChange.Text = $"₱{change:N2}";
            _lblChange.ForeColor = AppTheme.SuccessGreen;
        }
    }

    private void BtnConfirm_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        var method = _cmbMethod.SelectedItem?.ToString() ?? "Cash";
        decimal paid = _numPaid.Value;

        if (paid < _total)
        {
            _lblStatus.Text = "Amount paid cannot be less than the total.";
            return;
        }

        if (method == "GCash" && string.IsNullOrWhiteSpace(_txtReference.Text))
        {
            _lblStatus.Text = "GCash Reference Number is required.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            if (db.Transactions.Any(x => x.OrderId == _orderId))
            {
                _lblStatus.Text = "This order has already been paid.";
                return;
            }

            var order = db.Orders.FirstOrDefault(x => x.OrderId == _orderId);
            if (order is null)
            {
                _lblStatus.Text = "Order no longer exists.";
                return;
            }

            decimal change = paid - _total;

            db.Transactions.Add(new Transaction
            {
                OrderId = _orderId,
                PaymentMethod = method,
                AmountPaid = paid,
                ChangeDue = change,
                ReferenceNumber = method == "GCash" ? _txtReference.Text.Trim() : null,
                PaidAt = DateTime.UtcNow
            });

            order.Status = "Paid";
            db.SaveChanges();

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = ex.Message;
        }
    }
}