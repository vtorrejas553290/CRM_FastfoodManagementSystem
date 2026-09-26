using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmActivityLogs : Form
{
    private readonly IActivityLogController _controller = new ActivityLogController();

    private List<ActivityLogRow> _allRows = new();
    private int _currentPage = 1;

    private bool _syncingDates = false;

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
            if (_syncingDates) return;

            _syncingDates = true;
            SyncDatePickersFromRange();
            _syncingDates = false;

            _currentPage = 1;
            LoadLogs();
        };

        dtpFromDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateFilter.SelectedItem?.ToString() != "Custom")
                cmbDateFilter.SelectedItem = "Custom";
            _syncingDates = false;

            _currentPage = 1;
            LoadLogs();
        };

        dtpToDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateFilter.SelectedItem?.ToString() != "Custom")
                cmbDateFilter.SelectedItem = "Custom";
            _syncingDates = false;

            _currentPage = 1;
            LoadLogs();
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
        AppTheme.StyleInput(cmbActionFilter);
        AppTheme.StyleInput(cmbDateFilter);
       
        AppTheme.StyleGrid(gridLogs);

        AppTheme.StyleLabel(lblActionFilter);
        AppTheme.StyleLabel(lblDateFilter);
        AppTheme.StyleLabel(lblFromDate);
        AppTheme.StyleLabel(lblToDate);

        lblTotal.Font = AppTheme.FontSubheading;
        lblTotal.ForeColor = AppTheme.Primary20;

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
            "All", "Login", "Logout", "Create", "Update",
            "Archive", "Restock", "Order", "Payment"
        });
        cmbActionFilter.SelectedIndex = 0;

        cmbDateFilter.Items.Clear();
        cmbDateFilter.Items.AddRange(new object[]
        {
            "All", "Today", "Last 7 Days", "Last 30 Days", "Custom"
        });

        _syncingDates = true;
        cmbDateFilter.SelectedIndex = 0;   // "All"
        SyncDatePickersFromRange();
        _syncingDates = false;
    }

    private void SyncDatePickersFromRange()
    {
        var now = DateTime.UtcNow;
        var sel = cmbDateFilter.SelectedItem?.ToString() ?? "All";

        switch (sel)
        {
            case "Today":
                dtpFromDate.Value = now.Date;
                dtpToDate.Value = now.Date;
                break;
            case "Last 7 Days":
                dtpFromDate.Value = now.AddDays(-7).Date;
                dtpToDate.Value = now.Date;
                break;
            case "Last 30 Days":
                dtpFromDate.Value = now.AddDays(-30).Date;
                dtpToDate.Value = now.Date;
                break;
            case "All":
            case "Custom":
                // leave current picker values alone
                break;
        }
    }

    private (DateTime? from, DateTime? to) GetFromTo()
    {
        var sel = cmbDateFilter.SelectedItem?.ToString() ?? "All";

        if (sel == "All")
            return (null, null);

        DateTime from = dtpFromDate.Value.Date;
        DateTime to = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
        return (from, to);
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    private void LoadLogs()
    {
        try
        {
            var (from, to) = GetFromTo();

            var filter = new ActivityLogFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                Action = cmbActionFilter.SelectedItem?.ToString() ?? "All",
                DateRange = cmbDateFilter.SelectedItem?.ToString() ?? "All",
                FromDate = from,
                ToDate = to
            };

            _allRows = _controller.GetActivityLogs(filter);

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            lblTotal.Text = $"Total Entries: {_allRows.Count}";
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
        List<ActivityLogRow> pageRows;

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

    private void StyleColumns()
    {
        var grid = gridLogs;

        if (grid.Columns.Contains("ActivityLogId")) grid.Columns["ActivityLogId"].Visible = false;

        SetColumnFixed("PerformedAt", "When", 160, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("Username", "User", 130, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("RoleCode", "Role", 100, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("ActionType", "Action", 110, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("EntityName", "Entity", 140, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("EntityId", "ID", 70, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("Description", "Description", 100, 200, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("PerformedAt"))
            grid.Columns["PerformedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.RowTemplate.Height = 32;
    }

    private void SetColumnFixed(string name, string header, int width, DataGridViewContentAlignment align)
    {
        if (!gridLogs.Columns.Contains(name)) return;
        var c = gridLogs.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridLogs.Columns.Contains(name)) return;
        var c = gridLogs.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }
}