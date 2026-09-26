using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmProductManagement : Form
{
    private readonly IProductController _controller = new ProductController();

    private List<ProductRow> _allRows = new();
    private int _currentPage = 1;

    public FrmProductManagement()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridProducts,
            ("colEdit", "warning"),
            ("colArchive", "danger"));

        Load += (_, __) =>
        {
            InitPager();
            LoadProducts();
        };

        btnAdd.Click += BtnAdd_Click;
        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadProducts();
        };
        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadProducts();
        };

        cmbPageSize.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            RenderPage();
        };
        btnFirstPage.Click += (_, __) => GoToPage(1);
        btnPrevPage.Click += (_, __) => GoToPage(_currentPage - 1);
        btnNextPage.Click += (_, __) => GoToPage(_currentPage + 1);
        btnLastPage.Click += (_, __) => GoToPage(TotalPages);
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlTop.BackColor = AppTheme.Surface;
        pnlGridWrap.BackColor = AppTheme.ContentSurface;
        pnlPager.BackColor = AppTheme.Surface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSuccessButton(btnAdd);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridProducts);

        chkShowArchived.ForeColor = AppTheme.TextPrimary;
        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.BackColor = Color.Transparent;

        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    private void LoadProducts()
    {
        try
        {
            var filter = new ProductFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                ShowArchived = chkShowArchived.Checked
            };

            _allRows = _controller.GetProducts(filter);

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            int count = _allRows.Count;
            if (filter.ShowArchived)
            {
                lblStatus.ForeColor = AppTheme.TextSecondary;
                lblStatus.Text = $"{count} archived product(s).";
            }
            else
            {
                lblStatus.ForeColor = AppTheme.TextSecondary;
                lblStatus.Text = $"{count} active product(s).";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ============================================================
    //  PAGINATION
    // ============================================================

    private int PageSize
    {
        get
        {
            var s = cmbPageSize.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(s) || s == "All") return int.MaxValue;
            return int.TryParse(s, out int n) && n > 0 ? n : 25;
        }
    }

    private int TotalPages
    {
        get
        {
            if (PageSize == int.MaxValue) return 1;
            return Math.Max(1, (int)Math.Ceiling(_allRows.Count / (double)PageSize));
        }
    }

    private void GoToPage(int page)
    {
        int target = Math.Clamp(page, 1, TotalPages);
        if (target == _currentPage) return;
        _currentPage = target;
        RenderPage();
    }

    private void RenderPage()
    {
        int size = PageSize;
        List<ProductRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridProducts.DataSource = pageRows;
        BuildActionColumns();
        StyleColumns();

        int total = TotalPages;
        lblPageInfo.Text = $"Page {_currentPage} of {total}";

        int first = _allRows.Count == 0
            ? 0
            : ((_currentPage - 1) * (size == int.MaxValue ? _allRows.Count : size)) + 1;
        int last = size == int.MaxValue
            ? _allRows.Count
            : Math.Min(_currentPage * size, _allRows.Count);
        lblShowing.Text = $"Showing {first}–{last} of {_allRows.Count}";

        bool multiPage = total > 1;
        btnFirstPage.Enabled = multiPage && _currentPage > 1;
        btnPrevPage.Enabled = multiPage && _currentPage > 1;
        btnNextPage.Enabled = multiPage && _currentPage < total;
        btnLastPage.Enabled = multiPage && _currentPage < total;
    }

    // ============================================================
    //  ACTION COLUMNS
    // ============================================================

    private void BuildActionColumns()
    {
        for (int i = gridProducts.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridProducts.Columns[i];
            if (col.Name == "colEdit" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridProducts.Columns.RemoveAt(i);
        }

        bool showArchived = chkShowArchived.Checked;

        gridProducts.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEdit", "Edit", "Edit", "warning", 90));

        if (showArchived)
        {
            gridProducts.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 110));

            AppTheme.ApplyStyleToButtonColumn(gridProducts, "colUnarchive", "success");
        }
        else
        {
            gridProducts.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 100));

            AppTheme.ApplyStyleToButtonColumn(gridProducts, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridProducts, "colEdit", "warning");

        gridProducts.CellContentClick -= GridProducts_CellContentClick;
        gridProducts.CellContentClick += GridProducts_CellContentClick;
    }

    private void StyleColumns()
    {
        var grid = gridProducts;

        if (grid.Columns.Contains("ProductId")) grid.Columns["ProductId"].Visible = false;

        SetColumnFixed("ProductCode", "Code", 120, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Category", "Category", 150, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("UnitPrice", "Unit Price", 120, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("Qty", "Qty", 90, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("Status", "Status", 110, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("ProductName", "Product Name", 100, 200, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("UnitPrice")) grid.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
        if (grid.Columns.Contains("Qty")) grid.Columns["Qty"].DefaultCellStyle.Format = "N2";

        foreach (var name in new[] { "colEdit", "colArchive", "colUnarchive" })
        {
            if (!grid.Columns.Contains(name)) continue;
            var c = grid.Columns[name];
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Resizable = DataGridViewTriState.False;
            c.DefaultCellStyle.Padding = new Padding(0);
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            c.DefaultCellStyle.ForeColor = AppTheme.TextOnDark;
            c.DefaultCellStyle.SelectionForeColor = AppTheme.TextOnDark;
            c.HeaderCell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c.MinimumWidth = c.Width;
        }

        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.RowTemplate.Height = 32;

        grid.CellFormatting -= Grid_CellFormatting;
        grid.CellFormatting += Grid_CellFormatting;
    }

    private void SetColumnFixed(string name, string header, int width, DataGridViewContentAlignment align)
    {
        if (!gridProducts.Columns.Contains(name)) return;
        var c = gridProducts.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridProducts.Columns.Contains(name)) return;
        var c = gridProducts.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridProducts.Rows.Count) return;
        var row = gridProducts.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridProducts.Columns[e.ColumnIndex].Name != "Status") return;

        var status = row.Cells["Status"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = status switch
        {
            "Active" => Color.FromArgb(22, 130, 60),
            "Archived" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    private void GridProducts_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridProducts.Columns[e.ColumnIndex];

        if (column.Name == "colEdit")
            HandleEdit(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
        else if (column.Name == "colUnarchive")
            HandleUnarchive(e.RowIndex);
    }

    private int? GetProductIdAtRow(int rowIndex)
    {
        var cell = gridProducts.Rows[rowIndex].Cells["ProductId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void HandleEdit(int rowIndex)
    {
        var id = GetProductIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmProductEditor(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Update", "Product", id.Value,
                $"Product #{id.Value} updated");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "Product updated.";
            LoadProducts();
        }
    }

    private void HandleArchive(int rowIndex)
    {
        var id = GetProductIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Archive this product? It will no longer appear in active lists.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var product = db.Products.FirstOrDefault(x => x.ProductId == id.Value);
            if (product is null) return;

            product.IsActive = false;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "Product", id.Value,
                $"Product '{product.ProductName}' archived");

            lblStatus.ForeColor = AppTheme.Danger;
            lblStatus.Text = "Product archived.";
            LoadProducts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void HandleUnarchive(int rowIndex)
    {
        var id = GetProductIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Restore this product to active status?",
            "Confirm Unarchive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var product = db.Products.FirstOrDefault(x => x.ProductId == id.Value);
            if (product is null) return;

            product.IsActive = true;
            db.SaveChanges();

            ActivityLogger.Log("Unarchive", "Product", id.Value,
                $"Product '{product.ProductName}' restored");

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Product restored.";
            LoadProducts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var dlg = new FrmProductEditor();
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Create", "Product", null,
                "New product added");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "Product added.";
            LoadProducts();
        }
    }
}