using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmProductEditor : Form
{
    private readonly int? _productId;

    private TextBox _txtCode = new();
    private TextBox _txtName = new();
    private ComboBox _cmbCategory = new();
    private NumericUpDown _numPrice = new();
    private CheckBox _chkActive = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Button _btnNewCategory = new();
    private Label _lblStatus = new();

    public FrmProductEditor() : this(null) { }

    public FrmProductEditor(int? productId)
    {
        _productId = productId;
        BuildUi();
        Load += FrmProductEditor_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = _productId is null ? "Add Product" : "Edit Product";
        ClientSize = new Size(500, 360);

        var lblCode = new Label { Text = "Product Code:", Location = new Point(20, 20), AutoSize = true };
        AppTheme.StyleLabel(lblCode);
        _txtCode = new TextBox { Location = new Point(150, 18), Width = 300 };
        AppTheme.StyleInput(_txtCode);

        var lblName = new Label { Text = "Product Name:", Location = new Point(20, 60), AutoSize = true };
        AppTheme.StyleLabel(lblName);
        _txtName = new TextBox { Location = new Point(150, 58), Width = 300 };
        AppTheme.StyleInput(_txtName);

        var lblCategory = new Label { Text = "Category:", Location = new Point(20, 100), AutoSize = true };
        AppTheme.StyleLabel(lblCategory);
        _cmbCategory = new ComboBox
        {
            Location = new Point(150, 98),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        AppTheme.StyleInput(_cmbCategory);

        _btnNewCategory = new Button { Text = "New", Location = new Point(360, 97), Width = 90 };
        AppTheme.StylePrimaryButton(_btnNewCategory);
        _btnNewCategory.Click += BtnNewCategory_Click;

        var lblPrice = new Label { Text = "Unit Price:", Location = new Point(20, 140), AutoSize = true };
        AppTheme.StyleLabel(lblPrice);
        _numPrice = new NumericUpDown
        {
            Location = new Point(150, 138),
            Width = 150,
            DecimalPlaces = 2,
            Maximum = 1_000_000
        };
        AppTheme.StyleInput(_numPrice);

        _chkActive = new CheckBox
        {
            Text = "Is Active",
            Location = new Point(150, 175),
            Checked = true,
            AutoSize = true
        };
        _chkActive.Font = AppTheme.FontBody;
        _chkActive.ForeColor = AppTheme.TextPrimary;

        _btnSave = new Button { Text = "Save", Location = new Point(150, 220), Width = 140 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(300, 220), Width = 140 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label { Location = new Point(20, 275), AutoSize = true, MaximumSize = new Size(460, 40) };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[]
        {
            lblCode, _txtCode, lblName, _txtName,
            lblCategory, _cmbCategory, _btnNewCategory,
            lblPrice, _numPrice, _chkActive,
            _btnSave, _btnCancel, _lblStatus
        });
    }

    private void FrmProductEditor_Load(object? sender, EventArgs e)
    {
        LoadCategories();

        if (_productId is null) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var p = db.Products.AsNoTracking().FirstOrDefault(x => x.ProductId == _productId.Value);
            if (p is null) { Close(); return; }

            _txtCode.Text = p.ProductCode;
            _txtCode.Enabled = false;   // code is immutable after creation
            _txtName.Text = p.ProductName;
            _cmbCategory.SelectedValue = p.CategoryId;
            _numPrice.Value = p.UnitPrice;
            _chkActive.Checked = p.IsActive;
        }
        catch (Exception ex)
        {
            _lblStatus.ForeColor = AppTheme.Error;
            _lblStatus.Text = ex.Message;
        }
    }

    private void LoadCategories()
    {
        using var db = AppServices.CreateTenantContext();
        var cats = db.Categories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.CategoryName)
            .Select(x => new { x.CategoryId, x.CategoryName })
            .ToList();

        _cmbCategory.DataSource = cats;
        _cmbCategory.DisplayMember = "CategoryName";
        _cmbCategory.ValueMember = "CategoryId";
    }

    private void BtnNewCategory_Click(object? sender, EventArgs e)
    {
        using var dlg = new FrmCategoryEditor();
        if (dlg.ShowDialog(this) == DialogResult.OK)
            LoadCategories();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        // ============ VALIDATION ============
        if (string.IsNullOrWhiteSpace(_txtCode.Text) || _txtCode.Text.Trim().Length < 2)
        {
            _lblStatus.Text = "Product Code must be at least 2 characters.";
            return;
        }

        if (string.IsNullOrWhiteSpace(_txtName.Text) || _txtName.Text.Trim().Length < 2)
        {
            _lblStatus.Text = "Product Name must be at least 2 characters.";
            return;
        }

        if (_cmbCategory.SelectedValue is null)
        {
            _lblStatus.Text = "Please select a category.";
            return;
        }

        if (_numPrice.Value <= 0)
        {
            _lblStatus.Text = "Price must be greater than 0.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            if (_productId is null)
            {
                // ============ ADD ============
                if (db.Products.Any(x => x.ProductCode == _txtCode.Text.Trim()))
                {
                    _lblStatus.Text = "Product code already exists.";
                    return;
                }

                var product = new Product
                {
                    ProductCode = _txtCode.Text.Trim(),
                    ProductName = _txtName.Text.Trim(),
                    CategoryId = (int)_cmbCategory.SelectedValue,
                    UnitPrice = _numPrice.Value,
                    IsActive = _chkActive.Checked,
                    CreatedAt = DateTime.UtcNow
                };

                db.Products.Add(product);
                db.SaveChanges();

                // Auto-create inventory record
                db.Inventories.Add(new Inventory
                {
                    ProductId = product.ProductId,
                    QuantityOnHand = 0,
                    ReorderLevel = 10,
                    LastUpdatedAt = DateTime.UtcNow
                });
                db.SaveChanges();

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // ============ EDIT ============
                var product = db.Products.FirstOrDefault(x => x.ProductId == _productId.Value);
                if (product is null)
                {
                    _lblStatus.Text = "Product no longer exists.";
                    return;
                }

                product.ProductName = _txtName.Text.Trim();
                product.CategoryId = (int)_cmbCategory.SelectedValue;
                product.UnitPrice = _numPrice.Value;
                product.IsActive = _chkActive.Checked;

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