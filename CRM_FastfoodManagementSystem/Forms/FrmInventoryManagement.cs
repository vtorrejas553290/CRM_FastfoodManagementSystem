using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmInventoryManagement : Form
{
    private readonly IInventoryController _controller = new InventoryController();

    private List<InventoryRow> _allRows = new();
    private int _currentPage = 1;

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

       

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadInventory();
        };

        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadInventory();
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
        cmbPageSize.SelectedIndex = 1;
    }

    private void LoadInventory()
    {
        try
        {
            var filter = new InventoryFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                ShowArchived = chkShowArchived.Checked
            };

            _allRows = _controller.GetInventory(filter);

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            if (filter.ShowArchived)
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
        StyleColumns();

        int total = TotalPages;
        lblPageInfo.Text = $"Page {_currentPage} of {total}";

        int first = _allRows.Count == 0 ? 0 : ((_currentPage - 1) * (size == int.MaxValue ? _allRows.Count : size)) + 1;
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

    private void StyleColumns()
    {
        var grid = gridInventory;

        if (grid.Columns.Contains("InventoryId")) grid.Columns["InventoryId"].Visible = false;

        SetColumnFixed("ProductCode", "Code", 120, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("QuantityOnHand", "Qty On Hand", 130, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("ReorderLevel", "Reorder Level", 130, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("Alert", "Alert", 120, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("LastUpdatedAt", "Last Updated", 160, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("Product", "Product", 100, 180, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("QuantityOnHand")) grid.Columns["QuantityOnHand"].DefaultCellStyle.Format = "N2";
        if (grid.Columns.Contains("ReorderLevel")) grid.Columns["ReorderLevel"].DefaultCellStyle.Format = "N2";
        if (grid.Columns.Contains("LastUpdatedAt")) grid.Columns["LastUpdatedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

        foreach (var name in new[] { "colRestock", "colArchive", "colUnarchive" })
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
        grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        grid.CellFormatting -= Grid_CellFormatting;
        grid.CellFormatting += Grid_CellFormatting;
    }

    private void SetColumnFixed(string name, string header, int width, DataGridViewContentAlignment align)
    {
        if (!gridInventory.Columns.Contains(name)) return;
        var c = gridInventory.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridInventory.Columns.Contains(name)) return;
        var c = gridInventory.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridInventory.Rows.Count) return;
        var row = gridInventory.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridInventory.Columns[e.ColumnIndex].Name != "Alert") return;

        var alert = row.Cells["Alert"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = alert.StartsWith("⚠")
            ? Color.FromArgb(190, 40, 40)
            : Color.FromArgb(22, 130, 60);
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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