using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmProductManagement : Form
{
    public FrmProductManagement()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridProducts,
            ("colEdit", "warning"),
            ("colArchive", "danger"));

        Load += (_, __) => LoadProducts();
        btnRefresh.Click += (_, __) => LoadProducts();
        btnAdd.Click += BtnAdd_Click;
        txtSearch.TextChanged += (_, __) => LoadProducts();
        chkShowArchived.CheckedChanged += (_, __) => LoadProducts();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlTop.BackColor = AppTheme.Surface;
        pnlGridWrap.BackColor = AppTheme.ContentSurface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleSuccessButton(btnAdd);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridProducts);

        chkShowArchived.ForeColor = AppTheme.TextPrimary;
        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.BackColor = Color.Transparent;
    }

    private void LoadProducts()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var search = txtSearch.Text.Trim().ToLower();
            bool showArchived = chkShowArchived.Checked;

            var query = db.Products
                .Include(x => x.Category)
                .Include(x => x.Inventory)
                .AsNoTracking()
                .AsQueryable();

            // Filter by archive state
            if (showArchived)
                query = query.Where(x => !x.IsActive);
            else
                query = query.Where(x => x.IsActive);

            var products = query
                .Where(x => string.IsNullOrWhiteSpace(search) ||
                            x.ProductName.ToLower().Contains(search) ||
                            x.ProductCode.ToLower().Contains(search))
                .OrderBy(x => x.ProductId)
                .Select(x => new
                {
                    x.ProductId,
                    x.ProductCode,
                    x.ProductName,
                    Category = x.Category != null ? x.Category.CategoryName : "(none)",
                    x.UnitPrice,
                    Qty = x.Inventory != null ? x.Inventory.QuantityOnHand : 0,
                    Status = x.IsActive ? "Active" : "Archived"
                })
                .ToList();

            gridProducts.DataSource = products;
            BuildActionColumns();

            // Update status bar
            int count = products.Count;
            if (showArchived)
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

    private void BuildActionColumns()
    {
        // Remember which one was removed to rebuild cleanly
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
            // Unarchive button for archived rows
            gridProducts.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 110));

            AppTheme.ApplyStyleToButtonColumn(gridProducts, "colUnarchive", "success");
        }
        else
        {
            // Archive button for active rows
            gridProducts.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 100));

            AppTheme.ApplyStyleToButtonColumn(gridProducts, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridProducts, "colEdit", "warning");

        gridProducts.CellContentClick -= GridProducts_CellContentClick;
        gridProducts.CellContentClick += GridProducts_CellContentClick;
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