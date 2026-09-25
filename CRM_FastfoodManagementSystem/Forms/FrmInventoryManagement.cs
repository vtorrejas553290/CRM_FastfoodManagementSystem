using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmInventoryManagement : Form
{
    // ---- Pagination state ----
    private List<InventoryRow> _allRows = new();
    private int _currentPage = 1;

    /// <summary>
    /// Snapshot row type. Needed because we paginate client-side after
    /// running the EF query, so we can't use an anonymous type in a field.
    /// </summary>
    private class InventoryRow
    {
        public int InventoryId { get; set; }
        public string ProductCode { get; set; } = "";
        public string Product { get; set; } = "";
        public decimal QuantityOnHand { get; set; }
        public decimal ReorderLevel { get; set; }
        public string Alert { get; set; } = "";
        public DateTime LastUpdatedAt { get; set; }
    }

    public FrmInventoryManagement()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridInventory,
            ("colRestock", "success"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) =>
        {
            InitPager();
            LoadInventory();
        };

        btnRefresh.Click += (_, __) => LoadInventory();

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;     // reset page on new search
            LoadInventory();
        };

        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;     // reset page on archive toggle
            LoadInventory();
        };

        // Pagination events
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
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridInventory);

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
        cmbPageSize.SelectedIndex = 1;   // default 25
    }

    // ============================================================
    //  DATA LOAD
    // ============================================================

    private void LoadInventory()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var search = txtSearch.Text.Trim().ToLower();
            bool showArchived = chkShowArchived.Checked;

            var query = db.Inventories
                .Include(x => x.Product)
                .AsNoTracking()
                .AsQueryable();

            if (showArchived)
                query = query.Where(x => x.Product != null && !x.Product.IsActive);
            else
                query = query.Where(x => x.Product != null && x.Product.IsActive);

            _allRows = query
                .Where(x => string.IsNullOrWhiteSpace(search) ||
                            x.Product!.ProductName.ToLower().Contains(search) ||
                            x.Product.ProductCode.ToLower().Contains(search))
                .OrderBy(x => x.Product!.ProductName)
                .Select(x => new InventoryRow
                {
                    InventoryId = x.InventoryId,
                    ProductCode = x.Product!.ProductCode,
                    Product = x.Product.ProductName,
                    QuantityOnHand = x.QuantityOnHand,
                    ReorderLevel = x.ReorderLevel,
                    Alert = x.QuantityOnHand <= x.ReorderLevel ? "⚠ REORDER" : "OK",
                    LastUpdatedAt = x.LastUpdatedAt
                })
                .ToList();

            // If current page exceeds new total pages, clamp
            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            // Status label
            if (showArchived)
            {
                lblStatus.ForeColor = AppTheme.TextSecondary;
                lblStatus.Text = $"{_allRows.Count} archived inventory item(s).";
            }
            else
            {
                int low = _allRows.Count(x => x.Alert.StartsWith("⚠"));
                lblStatus.ForeColor = low > 0 ? AppTheme.Danger : AppTheme.TextSecondary;
                lblStatus.Text = $"{_allRows.Count} active item(s) — {low} need reorder.";
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
        List<InventoryRow> pageRows;

        if (size == int.MaxValue)
        {
            pageRows = _allRows;
        }
        else
        {
            int skip = (_currentPage - 1) * size;
            pageRows = _allRows.Skip(skip).Take(size).ToList();
        }

        gridInventory.DataSource = pageRows.Select(x => new
        {
            x.InventoryId,
            x.ProductCode,
            x.Product,
            x.QuantityOnHand,
            x.ReorderLevel,
            x.Alert,
            x.LastUpdatedAt
        }).ToList();

        BuildActionColumns();

        // Update page-info label
        int total = TotalPages;
        lblPageInfo.Text = $"Page {_currentPage} of {total}";

        // Update showing label
        int first = _allRows.Count == 0 ? 0 : ((_currentPage - 1) * (size == int.MaxValue ? _allRows.Count : size)) + 1;
        int last = size == int.MaxValue
            ? _allRows.Count
            : Math.Min(_currentPage * size, _allRows.Count);
        lblShowing.Text = $"Showing {first}–{last} of {_allRows.Count}";

        // Enable/disable nav buttons
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
        for (int i = gridInventory.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridInventory.Columns[i];
            if (col.Name == "colRestock" ||
                col.Name == "colArchive" ||
                col.Name == "colUnarchive")
            {
                gridInventory.Columns.RemoveAt(i);
            }
        }

        bool showArchived = chkShowArchived.Checked;

        if (showArchived)
        {
            gridInventory.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 120));

            AppTheme.ApplyStyleToButtonColumn(gridInventory, "colUnarchive", "success");
        }
        else
        {
            gridInventory.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colRestock", "Restock", "Restock", "success", 110));

            gridInventory.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 110));

            AppTheme.ApplyStyleToButtonColumn(gridInventory, "colRestock", "success");
            AppTheme.ApplyStyleToButtonColumn(gridInventory, "colArchive", "danger");
        }

        gridInventory.CellContentClick -= GridInventory_CellContentClick;
        gridInventory.CellContentClick += GridInventory_CellContentClick;
    }

    private void GridInventory_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridInventory.Columns[e.ColumnIndex];

        if (column.Name == "colRestock")
            HandleRestock(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
        else if (column.Name == "colUnarchive")
            HandleUnarchive(e.RowIndex);
    }

    private int? GetInventoryIdAtRow(int rowIndex)
    {
        var cell = gridInventory.Rows[rowIndex].Cells["InventoryId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    // ============================================================
    //  HANDLERS
    // ============================================================

    private void HandleRestock(int rowIndex)
    {
        var id = GetInventoryIdAtRow(rowIndex);
        if (id is null) return;

        var input = Microsoft.VisualBasic.Interaction.InputBox(
            "Enter quantity to add to stock:",
            "Restock",
            "10");

        if (!decimal.TryParse(input, out decimal amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid positive number.", "Invalid input");
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();
            var inv = db.Inventories.FirstOrDefault(x => x.InventoryId == id.Value);
            if (inv is null) return;

            inv.QuantityOnHand += amount;
            inv.LastUpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            ActivityLogger.Log("Restock", "Inventory", id.Value,
                $"Restocked +{amount} units");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = $"Restocked +{amount} units.";
            LoadInventory();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void HandleArchive(int rowIndex)
    {
        var id = GetInventoryIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Archive this product from inventory? The product will be marked inactive.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var inv = db.Inventories
                .Include(x => x.Product)
                .FirstOrDefault(x => x.InventoryId == id.Value);

            if (inv?.Product is null) return;

            inv.Product.IsActive = false;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "Product", inv.ProductId,
                $"Product '{inv.Product.ProductName}' archived from inventory");

            lblStatus.ForeColor = AppTheme.Danger;
            lblStatus.Text = "Product archived from inventory.";
            LoadInventory();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void HandleUnarchive(int rowIndex)
    {
        var id = GetInventoryIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Restore this product to active inventory?",
            "Confirm Unarchive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var inv = db.Inventories
                .Include(x => x.Product)
                .FirstOrDefault(x => x.InventoryId == id.Value);

            if (inv?.Product is null) return;

            inv.Product.IsActive = true;
            db.SaveChanges();

            ActivityLogger.Log("Unarchive", "Product", inv.ProductId,
                $"Product '{inv.Product.ProductName}' restored to inventory");

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Product restored to inventory.";
            LoadInventory();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}