using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmCustomerRetention : Form
{
    private const int AtRiskDays = 30;
    private const int DormantDays = 60;

    private List<RetentionRow> _allRows = new();
    private int _currentPage = 1;

    private class RetentionRow
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public int CurrentPoints { get; set; }
        public decimal PointsValue { get; set; }
        public DateTime? LastOrderAt { get; set; }
        public int? DaysSince { get; set; }
        public int TotalOrders { get; set; }
        public string Retention { get; set; } = "Active";
    }

    public FrmCustomerRetention()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridCustomers,
            ("colHistory", "primary"),
            ("colAdjust", "warning"),
            ("colContact", "success"));

        Load += (_, __) =>
        {
            InitFilters();
            InitPager();
            LoadCustomers();
        };

        btnRefresh.Click += (_, __) => LoadCustomers();

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadCustomers();
        };

        cmbStatusFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadCustomers();
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
        AppTheme.StyleInput(cmbStatusFilter);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridCustomers);
        AppTheme.StyleLabel(lblStatusFilter);

        lblPointsInfo.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
        lblPointsInfo.ForeColor = AppTheme.TextSecondary;

        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);
    }

    private void InitFilters()
    {
        cmbStatusFilter.Items.Clear();
        cmbStatusFilter.Items.AddRange(new object[]
        {
            "All", "Active", "At Risk", "Dormant", "Never"
        });
        cmbStatusFilter.SelectedIndex = 0;
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    // ============================================================
    //  DATA LOAD
    // ============================================================

    private void LoadCustomers()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var search = txtSearch.Text.Trim().ToLower();
            var statusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All";
            var now = DateTime.UtcNow;
            var cutoffAtRisk = now.AddDays(-AtRiskDays);
            var cutoffDormant = now.AddDays(-DormantDays);

            var raw = db.Customers
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Where(c => string.IsNullOrWhiteSpace(search) ||
                            c.CustomerCode.ToLower().Contains(search) ||
                            c.CustomerName.ToLower().Contains(search))
                .Select(c => new
                {
                    c.CustomerId,
                    c.CustomerCode,
                    c.CustomerName,
                    c.ContactNumber,
                    c.CurrentPoints,
                    LastOrderAt = c.Orders
                        .Where(o => o.Status != "Cancelled")
                        .Max(o => (DateTime?)o.OrderDate),
                    TotalOrders = c.Orders.Count(o => o.Status != "Cancelled")
                })
                .ToList();

            _allRows = raw.Select(x =>
            {
                int? days = x.LastOrderAt.HasValue
                    ? (int)(now - x.LastOrderAt.Value).TotalDays
                    : (int?)null;

                string status;
                if (x.LastOrderAt == null) status = "Never";
                else if (x.LastOrderAt < cutoffDormant) status = "Dormant";
                else if (x.LastOrderAt < cutoffAtRisk) status = "At Risk";
                else status = "Active";

                return new RetentionRow
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,
                    ContactNumber = x.ContactNumber ?? "-",
                    CurrentPoints = x.CurrentPoints,
                    PointsValue = x.CurrentPoints / 100m,
                    LastOrderAt = x.LastOrderAt,
                    DaysSince = days,
                    TotalOrders = x.TotalOrders,
                    Retention = status
                };
            }).ToList();

            if (statusFilter != "All")
                _allRows = _allRows.Where(r => r.Retention == statusFilter).ToList();

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            int active = _allRows.Count(r => r.Retention == "Active");
            int atRisk = _allRows.Count(r => r.Retention == "At Risk");
            int dormant = _allRows.Count(r => r.Retention == "Dormant");
            int never = _allRows.Count(r => r.Retention == "Never");
            int totalPoints = _allRows.Sum(r => r.CurrentPoints);

            lblStatus.ForeColor = AppTheme.TextSecondary;
            lblStatus.Text =
                $"{_allRows.Count} customer(s) — " +
                $"✅ {active} active · ⚠️ {atRisk} at risk · 🚨 {dormant} dormant · ⚪ {never} never — " +
                $"{totalPoints:N0} points in circulation (₱{totalPoints / 100m:N2})";
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
        List<RetentionRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridCustomers.DataSource = pageRows.Select(x => new
        {
            x.CustomerId,
            x.CustomerCode,
            x.CustomerName,
            x.ContactNumber,
            x.CurrentPoints,
            x.PointsValue,
            x.LastOrderAt,
            x.DaysSince,
            x.TotalOrders,
            x.Retention
        }).ToList();

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
    //  ACTION COLUMNS — History + Adjust + Contact
    // ============================================================

    private void BuildActionColumns()
    {
        for (int i = gridCustomers.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridCustomers.Columns[i];
            if (col.Name == "colHistory" ||
                col.Name == "colAdjust" ||
                col.Name == "colContact" ||
                col.Name == "colFollowUp" ||      // legacy cleanup
                col.Name == "colSendOffer")       // legacy cleanup
            {
                gridCustomers.Columns.RemoveAt(i);
            }
        }

        gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colHistory", "History", "View History", "primary", 130));

        gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colAdjust", "Adjust", "Adjust Points", "warning", 130));

        gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colContact", "Contact", "Contact Customer", "success", 180));

        AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colHistory", "primary");
        AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colAdjust", "warning");
        AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colContact", "success");

        gridCustomers.CellContentClick -= GridCustomers_CellContentClick;
        gridCustomers.CellContentClick += GridCustomers_CellContentClick;
    }

    // ============================================================
    //  COLUMN LAYOUT — edge-to-edge grid, no clipped buttons
    // ============================================================

    private void StyleColumns()
    {
        var grid = gridCustomers;

        // Hide the raw ID
        if (grid.Columns.Contains("CustomerId"))
            grid.Columns["CustomerId"].Visible = false;

        // ---- Code ----
        if (grid.Columns.Contains("CustomerCode"))
        {
            var c = grid.Columns["CustomerCode"];
            c.HeaderText = "Code";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 110;
            c.MinimumWidth = 110;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        // ---- Customer name (Fill — absorbs leftover space, not greedy) ----
        if (grid.Columns.Contains("CustomerName"))
        {
            var c = grid.Columns["CustomerName"];
            c.HeaderText = "Customer";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c.FillWeight = 100;
            c.MinimumWidth = 100;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        // ---- Contact number ----
        if (grid.Columns.Contains("ContactNumber"))
        {
            var c = grid.Columns["ContactNumber"];
            c.HeaderText = "Contact";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 130;
            c.MinimumWidth = 130;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        // ---- Points ----
        if (grid.Columns.Contains("CurrentPoints"))
        {
            var c = grid.Columns["CurrentPoints"];
            c.HeaderText = "Points";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 80;
            c.MinimumWidth = 80;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            c.DefaultCellStyle.Format = "N0";
        }

        // ---- Peso value ----
        if (grid.Columns.Contains("PointsValue"))
        {
            var c = grid.Columns["PointsValue"];
            c.HeaderText = "Value";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 90;
            c.MinimumWidth = 90;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            c.DefaultCellStyle.Format = "N2";
        }

        // ---- Last order date ----
        if (grid.Columns.Contains("LastOrderAt"))
        {
            var c = grid.Columns["LastOrderAt"];
            c.HeaderText = "Last Order";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 160;
            c.MinimumWidth = 160;
            c.DefaultCellStyle.Format = "yyyy-MM-dd";
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // ---- Days since ----
        if (grid.Columns.Contains("DaysSince"))
        {
            var c = grid.Columns["DaysSince"];
            c.HeaderText = "Days";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 70;
            c.MinimumWidth = 70;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // ---- Total orders ----
        if (grid.Columns.Contains("TotalOrders"))
        {
            var c = grid.Columns["TotalOrders"];
            c.HeaderText = "Orders";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 75;
            c.MinimumWidth = 75;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // ---- Retention status ----
        if (grid.Columns.Contains("Retention"))
        {
            var c = grid.Columns["Retention"];
            c.HeaderText = "Status";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 100;
            c.MinimumWidth = 100;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        // ---- Action columns: fixed widths, tight padding, clear font ----
        ApplyActionColumnLayout(grid, "colHistory", 130);
        ApplyActionColumnLayout(grid, "colAdjust", 130);
        ApplyActionColumnLayout(grid, "colContact", 180);

        grid.CellFormatting -= Grid_CellFormatting;
        grid.CellFormatting += Grid_CellFormatting;
    }

    /// <summary>
    /// Ensures an action column is wide enough, has no excess internal padding,
    /// and uses a font that renders its caption clearly.
    /// </summary>
    private static void ApplyActionColumnLayout(DataGridView grid, string columnName, int width)
    {
        if (!grid.Columns.Contains(columnName)) return;

        var c = grid.Columns[columnName];
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Resizable = DataGridViewTriState.False;
        c.Width = width;
        c.MinimumWidth = width;

        // Kill the default cell padding that squeezes the text
        c.DefaultCellStyle.Padding = new Padding(0);

        // Center the text horizontally and vertically
        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        // Use a slightly larger, medium-weight font for readability
        c.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

        // Keep the button text white on colored background
        c.DefaultCellStyle.ForeColor = AppTheme.TextOnDark;
        c.DefaultCellStyle.SelectionForeColor = AppTheme.TextOnDark;

        // Header font — bold for clarity
        c.HeaderCell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridCustomers.Rows.Count) return;

        var row = gridCustomers.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;

        var status = row.Cells["Retention"]?.Value?.ToString() ?? "";

        Color color = status switch
        {
            "Active" => Color.FromArgb(22, 130, 60),
            "At Risk" => Color.FromArgb(200, 130, 0),
            "Dormant" => Color.FromArgb(190, 40, 40),
            "Never" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };

        var colName = gridCustomers.Columns[e.ColumnIndex].Name;
        if (colName == "Retention" || colName == "DaysSince")
            e.CellStyle.ForeColor = color;
    }

    // ============================================================
    //  CLICK DISPATCH
    // ============================================================

    private void GridCustomers_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridCustomers.Columns[e.ColumnIndex];

        if (column.Name == "colHistory")
            ShowHistory(e.RowIndex);
        else if (column.Name == "colAdjust")
            AdjustPointsForRow(e.RowIndex);
        else if (column.Name == "colContact")
            OpenContact(e.RowIndex);
    }

    private int? GetCustomerIdAtRow(int rowIndex)
    {
        var cell = gridCustomers.Rows[rowIndex].Cells["CustomerId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    // ============================================================
    //  ACTIONS
    // ============================================================

    private void ShowHistory(int rowIndex)
    {
        var id = GetCustomerIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmPointsHistory(id.Value);
        dlg.ShowDialog(this);
    }

    private void AdjustPointsForRow(int rowIndex)
    {
        var id = GetCustomerIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmAdjustPoints(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Points", "Customer", id.Value, "Points adjusted");
            LoadCustomers();
        }
    }

    private void OpenContact(int rowIndex)
    {
        var id = GetCustomerIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmContactCustomer(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            // Contact dialog logs everything internally.
            // No grid reload needed unless customer data changed.
        }
    }
}