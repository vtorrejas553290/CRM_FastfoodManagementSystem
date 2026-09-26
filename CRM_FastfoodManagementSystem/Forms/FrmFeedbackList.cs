using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmFeedbackList : Form
{
    private readonly IFeedbackController _controller = new FeedbackController();

    private readonly bool _canAdd;
    private readonly bool _canManage;
    private readonly bool _canView;

    private List<FeedbackRow> _allRows = new();
    private int _currentPage = 1;

    public FrmFeedbackList()
    {
        _canAdd = UserSession.IsStaff;
        _canManage = UserSession.IsAdmin || UserSession.IsManager;
        _canView = UserSession.IsStaff || UserSession.IsAdmin || UserSession.IsManager;

        InitializeComponent();
        ApplyTheme();

        Load += (_, __) =>
        {
            InitStatusFilter();
            InitStarsFilter();
            InitPager();
            LoadFeedback();
        };

        
        btnAdd.Click += BtnAdd_Click;

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadFeedback();
        };
        cmbFilterStatus.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadFeedback();
        };
        cmbFilterStars.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadFeedback();
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
        AppTheme.StyleInput(cmbFilterStatus);
        AppTheme.StyleInput(cmbFilterStars);
        
        AppTheme.StyleSuccessButton(btnAdd);
        AppTheme.StyleGrid(gridFeedback);

        AppTheme.StyleLabel(lblSearch);
        AppTheme.StyleLabel(lblFilterStatus);
        AppTheme.StyleLabel(lblFilterStars);

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

        btnAdd.Visible = _canAdd;

        if (_canManage)
        {
            AppTheme.RegisterButtonColumnStyles(gridFeedback,
                ("colEditStatus", "warning"),
                ("colArchive", "danger"));
        }
    }

    private void InitStatusFilter()
    {
        cmbFilterStatus.Items.Clear();
        cmbFilterStatus.Items.Add("All");
        cmbFilterStatus.Items.Add("New");
        cmbFilterStatus.Items.Add("Reviewed");
        cmbFilterStatus.Items.Add("Resolved");
        cmbFilterStatus.Items.Add("Archived");
        cmbFilterStatus.SelectedIndex = 0;
    }

    private void InitStarsFilter()
    {
        cmbFilterStars.Items.Clear();
        cmbFilterStars.Items.Add("All");
        cmbFilterStars.Items.Add("5 ★");
        cmbFilterStars.Items.Add("4 ★");
        cmbFilterStars.Items.Add("3 ★");
        cmbFilterStars.Items.Add("2 ★");
        cmbFilterStars.Items.Add("1 ★");
        cmbFilterStars.SelectedIndex = 0;
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

    private void LoadFeedback()
    {
        try
        {
            if (!_canView)
            {
                _allRows = new List<FeedbackRow>();
                RenderPage();
                return;
            }

            var filter = new FeedbackFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                Status = cmbFilterStatus.SelectedItem?.ToString() ?? "All",
                StarsText = cmbFilterStars.SelectedItem?.ToString() ?? "All"
            };

            _allRows = _controller.GetFeedback(filter);

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
        List<FeedbackRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridFeedback.DataSource = pageRows;

        if (_canManage)
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
        for (int i = gridFeedback.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridFeedback.Columns[i];
            if (col.Name == "colEditStatus" || col.Name == "colArchive")
                gridFeedback.Columns.RemoveAt(i);
        }

        gridFeedback.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEditStatus", "Edit", "Edit Status", "warning", 110));

        gridFeedback.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colArchive", "Archive", "Archive", "danger", 100));

        AppTheme.ApplyStyleToButtonColumn(gridFeedback, "colEditStatus", "warning");
        AppTheme.ApplyStyleToButtonColumn(gridFeedback, "colArchive", "danger");

        gridFeedback.CellContentClick -= GridFeedback_CellContentClick;
        gridFeedback.CellContentClick += GridFeedback_CellContentClick;
    }

    private void StyleColumns()
    {
        var grid = gridFeedback;

        if (grid.Columns.Contains("CustomerFeedbackId")) grid.Columns["CustomerFeedbackId"].Visible = false;

        SetColumnFixed("Rating", "Rating", 90, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("Category", "Category", 130, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Status", "Status", 120, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("SubmittedAt", "Submitted", 130, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("Customer", "Customer", 40, 130, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFill("Comments", "Comments", 60, 180, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("SubmittedAt"))
            grid.Columns["SubmittedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

        foreach (var name in new[] { "colEditStatus", "colArchive" })
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
        if (!gridFeedback.Columns.Contains(name)) return;
        var c = gridFeedback.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridFeedback.Columns.Contains(name)) return;
        var c = gridFeedback.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridFeedback.Rows.Count) return;
        var row = gridFeedback.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridFeedback.Columns[e.ColumnIndex].Name != "Status") return;

        var status = row.Cells["Status"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = status switch
        {
            "New" => Color.FromArgb(30, 100, 200),
            "Reviewed" => Color.FromArgb(200, 130, 0),
            "Resolved" => Color.FromArgb(22, 130, 60),
            "Archived" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    // ============================================================
    //  CLICK DISPATCH
    // ============================================================

    private void GridFeedback_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridFeedback.Columns[e.ColumnIndex];

        if (column.Name == "colEditStatus")
            HandleEditStatus(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
    }

    private int? GetFeedbackIdAtRow(int rowIndex)
    {
        var cell = gridFeedback.Rows[rowIndex].Cells["CustomerFeedbackId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (!_canAdd)
        {
            MessageBox.Show("Only Staff can add feedback.", "Not allowed");
            return;
        }

        using var dlg = new FrmFeedbackEntry(null);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadFeedback();
        }
    }

    private void HandleEditStatus(int rowIndex)
    {
        if (!_canManage)
        {
            MessageBox.Show("Only Admin and Manager can edit feedback status.", "Not allowed");
            return;
        }

        var id = GetFeedbackIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmFeedbackStatusEditor(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadFeedback();
        }
    }

    private void HandleArchive(int rowIndex)
    {
        if (!_canManage)
        {
            MessageBox.Show("Only Admin and Manager can archive feedback.", "Not allowed");
            return;
        }

        var id = GetFeedbackIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Archive this feedback? It will be marked as 'Archived'.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var fb = db.CustomerFeedbacks.FirstOrDefault(x => x.CustomerFeedbackId == id.Value);
            if (fb is null) return;

            fb.Status = "Archived";
            db.SaveChanges();

            ActivityLogger.Log("Archive", "CustomerFeedback", id.Value,
                $"Feedback #{id.Value} archived");

            MessageBox.Show("Feedback archived.", "Success");
            LoadFeedback();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}