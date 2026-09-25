using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmActivityLogs : Form
{
    // ---- Pagination state ----
    private List<LogRow> _allRows = new();
    private int _currentPage = 1;

    /// <summary>
    /// Concrete row type — anonymous types can't be stored in a field.
    /// </summary>
    private class LogRow
    {
        public int ActivityLogId { get; set; }
        public DateTime PerformedAt { get; set; }
        public string Username { get; set; } = "";
        public string RoleCode { get; set; } = "";
        public string ActionType { get; set; } = "";
        public string EntityName { get; set; } = "";
        public int? EntityId { get; set; }
        public string Description { get; set; } = "";
    }

    public FrmActivityLogs()
    {
        InitializeComponent();
        ApplyTheme();

        Load += (_, __) =>
        {
            InitFilters();
            InitPager();
            LoadLogs();
        };

        btnRefresh.Click += (_, __) => LoadLogs();

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadLogs();
        };

        cmbActionFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadLogs();
        };

        cmbDateFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadLogs();
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
        AppTheme.StyleInput(cmbActionFilter);
        AppTheme.StyleInput(cmbDateFilter);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleGrid(gridLogs);

        AppTheme.StyleLabel(lblActionFilter);
        AppTheme.StyleLabel(lblDateFilter);

        lblTotal.Font = AppTheme.FontSubheading;
        lblTotal.ForeColor = AppTheme.Primary20;

        // Pager styling
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
        cmbActionFilter.Items.Clear();
        cmbActionFilter.Items.AddRange(new object[]
        {
            "All",
            "Login",
            "Logout",
            "Create",
            "Update",
            "Archive",
            "Restock",
            "Order",
            "Payment"
        });
        cmbActionFilter.SelectedIndex = 0;

        cmbDateFilter.Items.Clear();
        cmbDateFilter.Items.AddRange(new object[] { "All", "Today", "Last 7 Days", "Last 30 Days" });
        cmbDateFilter.SelectedIndex = 0;
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

    private void LoadLogs()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var search = txtSearch.Text.Trim().ToLower();
            var action = cmbActionFilter.SelectedItem?.ToString() ?? "All";
            var dateRange = cmbDateFilter.SelectedItem?.ToString() ?? "All";

            var query = db.ActivityLogs.AsNoTracking().AsQueryable();

            if (action != "All")
                query = query.Where(x => x.ActionType == action);

            var now = DateTime.UtcNow;

            if (dateRange == "Today")
                query = query.Where(x => x.PerformedAt.Date == now.Date);
            else if (dateRange == "Last 7 Days")
                query = query.Where(x => x.PerformedAt >= now.AddDays(-7));
            else if (dateRange == "Last 30 Days")
                query = query.Where(x => x.PerformedAt >= now.AddDays(-30));

            _allRows = query
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    (x.Username != null && x.Username.ToLower().Contains(search)) ||
                    x.EntityName.ToLower().Contains(search) ||
                    (x.Description != null && x.Description.ToLower().Contains(search)))
                .OrderByDescending(x => x.PerformedAt)
                .Take(1000)   // cap at 1000 rows for performance
                .Select(x => new LogRow
                {
                    ActivityLogId = x.ActivityLogId,
                    PerformedAt = x.PerformedAt,
                    Username = x.Username ?? "",
                    RoleCode = x.RoleCode ?? "",
                    ActionType = x.ActionType ?? "",
                    EntityName = x.EntityName ?? "",
                    EntityId = x.EntityId,
                    Description = x.Description ?? ""
                })
                .ToList();

            // Clamp page if filters shrank the set
            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            lblTotal.Text = $"Total Entries: {_allRows.Count}";
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
        List<LogRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridLogs.DataSource = pageRows.Select(x => new
        {
            x.ActivityLogId,
            x.PerformedAt,
            x.Username,
            x.RoleCode,
            x.ActionType,
            x.EntityName,
            x.EntityId,
            x.Description
        }).ToList();

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
    //  COLUMN STYLING
    // ============================================================

    private void StyleColumns()
    {
        if (gridLogs.Columns.Contains("ActivityLogId"))
            gridLogs.Columns["ActivityLogId"].Visible = false;

        if (gridLogs.Columns.Contains("PerformedAt"))
        {
            gridLogs.Columns["PerformedAt"].HeaderText = "When";
            gridLogs.Columns["PerformedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            gridLogs.Columns["PerformedAt"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            gridLogs.Columns["PerformedAt"].Width = 160;
            gridLogs.Columns["PerformedAt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        if (gridLogs.Columns.Contains("Username"))
            gridLogs.Columns["Username"].HeaderText = "User";

        if (gridLogs.Columns.Contains("RoleCode"))
            gridLogs.Columns["RoleCode"].HeaderText = "Role";

        if (gridLogs.Columns.Contains("ActionType"))
            gridLogs.Columns["ActionType"].HeaderText = "Action";

        if (gridLogs.Columns.Contains("EntityName"))
            gridLogs.Columns["EntityName"].HeaderText = "Entity";

        if (gridLogs.Columns.Contains("EntityId"))
            gridLogs.Columns["EntityId"].HeaderText = "ID";

        if (gridLogs.Columns.Contains("Description"))
        {
            gridLogs.Columns["Description"].HeaderText = "Description";
            gridLogs.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}