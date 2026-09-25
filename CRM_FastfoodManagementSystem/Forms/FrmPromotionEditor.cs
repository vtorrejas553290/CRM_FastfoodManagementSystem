using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmPromotionEditor : Form
{
    private readonly int? _promotionId;

    private TextBox _txtCode = new();
    private TextBox _txtName = new();
    private TextBox _txtDesc = new();
    private ComboBox _cmbType = new();
    private NumericUpDown _numValue = new();
    private NumericUpDown _numMin = new();
    private CheckBox _chkMin = new();
    private DateTimePicker _dtpStart = new();
    private DateTimePicker _dtpEnd = new();
    private CheckBox _chkActive = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

    public FrmPromotionEditor(int? promotionId)
    {
        _promotionId = promotionId;
        BuildUi();
        Load += FrmPromotionEditor_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = _promotionId is null ? "Add Promotion" : "Edit Promotion";
        ClientSize = new Size(560, 560);

        int y = 20;
        AddLabeledTextBox("Promotion Code:", ref y, out _txtCode);
        AddLabeledTextBox("Promotion Name:", ref y, out _txtName);

        // Description
        var lblDesc = new Label { Text = "Description:", Location = new Point(20, y), AutoSize = true };
        AppTheme.StyleLabel(lblDesc);
        _txtDesc = new TextBox { Location = new Point(170, y - 2), Width = 360, Height = 60, Multiline = true };
        AppTheme.StyleInput(_txtDesc);
        Controls.Add(lblDesc); Controls.Add(_txtDesc);
        y += 75;

        // Discount Type + Value
        var lblType = new Label { Text = "Discount Type:", Location = new Point(20, y), AutoSize = true };
        AppTheme.StyleLabel(lblType);
        _cmbType = new ComboBox { Location = new Point(170, y - 2), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
        AppTheme.StyleInput(_cmbType);
        _cmbType.Items.AddRange(new object[] { "Percent", "Fixed" });
        _cmbType.SelectedIndex = 0;
        Controls.Add(lblType); Controls.Add(_cmbType);

        var lblValue = new Label { Text = "Value:", Location = new Point(340, y), AutoSize = true };
        AppTheme.StyleLabel(lblValue);
        _numValue = new NumericUpDown { Location = new Point(400, y - 2), Width = 130, DecimalPlaces = 2, Maximum = 100000 };
        AppTheme.StyleInput(_numValue);
        Controls.Add(lblValue); Controls.Add(_numValue);
        y += 40;

        // Minimum Purchase
        _chkMin = new CheckBox { Text = "Minimum Purchase", Location = new Point(170, y), AutoSize = true };
        _chkMin.Font = AppTheme.FontBody;
        _chkMin.ForeColor = AppTheme.TextPrimary;
        _chkMin.CheckedChanged += (_, __) => _numMin.Enabled = _chkMin.Checked;
        _numMin = new NumericUpDown { Location = new Point(340, y - 2), Width = 190, DecimalPlaces = 2, Maximum = 1000000, Enabled = false };
        AppTheme.StyleInput(_numMin);
        Controls.Add(_chkMin); Controls.Add(_numMin);
        y += 40;

        // Start Date
        var lblStart = new Label { Text = "Start Date:", Location = new Point(20, y), AutoSize = true };
        AppTheme.StyleLabel(lblStart);
        _dtpStart = new DateTimePicker { Location = new Point(170, y - 2), Width = 200, Format = DateTimePickerFormat.Short };
        AppTheme.StyleInput(_dtpStart);
        Controls.Add(lblStart); Controls.Add(_dtpStart);
        y += 40;

        // End Date
        var lblEnd = new Label { Text = "End Date:", Location = new Point(20, y), AutoSize = true };
        AppTheme.StyleLabel(lblEnd);
        _dtpEnd = new DateTimePicker { Location = new Point(170, y - 2), Width = 200, Format = DateTimePickerFormat.Short };
        AppTheme.StyleInput(_dtpEnd);
        Controls.Add(lblEnd); Controls.Add(_dtpEnd);
        y += 45;

        // Active
        _chkActive = new CheckBox { Text = "Is Active", Location = new Point(170, y), AutoSize = true, Checked = true };
        _chkActive.Font = AppTheme.FontBody;
        _chkActive.ForeColor = AppTheme.TextPrimary;
        Controls.Add(_chkActive);
        y += 40;

        // Buttons
        _btnSave = new Button { Text = "Save", Location = new Point(170, y), Width = 140 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(330, y), Width = 200 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        Controls.Add(_btnSave);
        Controls.Add(_btnCancel);
        y += 55;

        _lblStatus = new Label { Location = new Point(20, y), AutoSize = true, MaximumSize = new Size(520, 40) };
        AppTheme.StyleLabel(_lblStatus);
        Controls.Add(_lblStatus);
    }

    private void AddLabeledTextBox(string label, ref int y, out TextBox box)
    {
        var lbl = new Label { Text = label, Location = new Point(20, y), AutoSize = true };
        AppTheme.StyleLabel(lbl);
        box = new TextBox { Location = new Point(170, y - 2), Width = 360 };
        AppTheme.StyleInput(box);
        Controls.Add(lbl);
        Controls.Add(box);
        y += 40;
    }

    private void FrmPromotionEditor_Load(object? sender, EventArgs e)
    {
        _dtpStart.Value = DateTime.Today;
        _dtpEnd.Value = DateTime.Today.AddDays(30);

        if (_promotionId is null) return;

        using var db = AppServices.CreateTenantContext();
        var promo = db.Promotions.AsNoTracking().FirstOrDefault(x => x.PromotionId == _promotionId.Value);
        if (promo is null) { Close(); return; }

        _txtCode.Text = promo.PromotionCode;
        _txtCode.Enabled = false;   // immutable
        _txtName.Text = promo.PromotionName;
        _txtDesc.Text = promo.Description ?? "";
        _cmbType.SelectedItem = promo.DiscountType;
        _numValue.Value = promo.DiscountValue;

        if (promo.MinimumPurchase.HasValue)
        {
            _chkMin.Checked = true;
            _numMin.Enabled = true;
            _numMin.Value = promo.MinimumPurchase.Value;
        }

        _dtpStart.Value = promo.StartDate;
        _dtpEnd.Value = promo.EndDate;
        _chkActive.Checked = promo.IsActive;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(_txtCode.Text) || _txtCode.Text.Trim().Length < 2)
        { _lblStatus.Text = "Promotion Code must be at least 2 characters."; return; }

        if (string.IsNullOrWhiteSpace(_txtName.Text) || _txtName.Text.Trim().Length < 3)
        { _lblStatus.Text = "Promotion Name must be at least 3 characters."; return; }

        if (_numValue.Value <= 0)
        { _lblStatus.Text = "Discount Value must be greater than 0."; return; }

        var type = _cmbType.SelectedItem?.ToString() ?? "Percent";
        if (type == "Percent" && _numValue.Value > 100)
        { _lblStatus.Text = "Percent discount cannot exceed 100."; return; }

        if (_dtpEnd.Value.Date < _dtpStart.Value.Date)
        { _lblStatus.Text = "End Date must be on or after Start Date."; return; }

        try
        {
            using var db = AppServices.CreateTenantContext();

            if (_promotionId is null)
            {
                // ADD
                if (db.Promotions.Any(x => x.PromotionCode == _txtCode.Text.Trim()))
                { _lblStatus.Text = "Promotion Code already exists."; return; }

                db.Promotions.Add(new Promotion
                {
                    PromotionCode = _txtCode.Text.Trim(),
                    PromotionName = _txtName.Text.Trim(),
                    Description = string.IsNullOrWhiteSpace(_txtDesc.Text) ? null : _txtDesc.Text.Trim(),
                    DiscountType = type,
                    DiscountValue = _numValue.Value,
                    MinimumPurchase = _chkMin.Checked ? _numMin.Value : null,
                    StartDate = _dtpStart.Value.Date,
                    EndDate = _dtpEnd.Value.Date,
                    IsActive = _chkActive.Checked,
                    CreatedAt = DateTime.UtcNow
                });

                db.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // EDIT
                var promo = db.Promotions.FirstOrDefault(x => x.PromotionId == _promotionId.Value);
                if (promo is null) { _lblStatus.Text = "Promotion no longer exists."; return; }

                promo.PromotionName = _txtName.Text.Trim();
                promo.Description = string.IsNullOrWhiteSpace(_txtDesc.Text) ? null : _txtDesc.Text.Trim();
                promo.DiscountType = type;
                promo.DiscountValue = _numValue.Value;
                promo.MinimumPurchase = _chkMin.Checked ? _numMin.Value : null;
                promo.StartDate = _dtpStart.Value.Date;
                promo.EndDate = _dtpEnd.Value.Date;
                promo.IsActive = _chkActive.Checked;

                db.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        catch (Exception ex)
        {
            _lblStatus.Text = ex.Message;
        }
    }
}