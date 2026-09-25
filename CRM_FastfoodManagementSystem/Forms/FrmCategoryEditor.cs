using CRM.domain.Entities;

namespace CRM.winForms.Forms;

public partial class FrmCategoryEditor : Form
{
    private TextBox _txtCode = new();
    private TextBox _txtName = new();
    private TextBox _txtDesc = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

    public FrmCategoryEditor()
    {
        BuildUi();
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = "New Category";
        ClientSize = new Size(440, 260);

        var lblCode = new Label { Text = "Code:", Location = new Point(20, 20), AutoSize = true };
        AppTheme.StyleLabel(lblCode);
        _txtCode = new TextBox { Location = new Point(140, 18), Width = 260 };
        AppTheme.StyleInput(_txtCode);

        var lblName = new Label { Text = "Name:", Location = new Point(20, 60), AutoSize = true };
        AppTheme.StyleLabel(lblName);
        _txtName = new TextBox { Location = new Point(140, 58), Width = 260 };
        AppTheme.StyleInput(_txtName);

        var lblDesc = new Label { Text = "Description:", Location = new Point(20, 100), AutoSize = true };
        AppTheme.StyleLabel(lblDesc);
        _txtDesc = new TextBox { Location = new Point(140, 98), Width = 260, Height = 60, Multiline = true };
        AppTheme.StyleInput(_txtDesc);

        _btnSave = new Button { Text = "Save", Location = new Point(140, 180), Width = 120 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(280, 180), Width = 120 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label { Location = new Point(20, 225), AutoSize = true };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[] { lblCode, _txtCode, lblName, _txtName, lblDesc, _txtDesc, _btnSave, _btnCancel, _lblStatus });
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(_txtCode.Text) || _txtCode.Text.Trim().Length < 2)
        {
            _lblStatus.Text = "Code must be at least 2 characters.";
            return;
        }

        if (string.IsNullOrWhiteSpace(_txtName.Text) || _txtName.Text.Trim().Length < 2)
        {
            _lblStatus.Text = "Name must be at least 2 characters.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            if (db.Categories.Any(x => x.CategoryCode == _txtCode.Text.Trim()))
            {
                _lblStatus.Text = "Category code already exists.";
                return;
            }

            db.Categories.Add(new Category
            {
                CategoryCode = _txtCode.Text.Trim(),
                CategoryName = _txtName.Text.Trim(),
                Description = _txtDesc.Text.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
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