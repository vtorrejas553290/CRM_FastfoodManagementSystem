using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmComplaints : Form
{
    private readonly IComplaintController _controller = new ComplaintController();

    private static readonly string[] Categories =
    {
        "Order Accuracy", "Food Quality", "Service", "Cleanliness"
    };

    private static readonly string[] Statuses =
    {
        "Open", "In Progress", "Resolved", "Closed", "Rejected"
    };

    private List<ComplaintRow> _allRows = new();
    private int _currentPage = 1;

    public FrmComplaints()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridComplaints,
            ("colView", "primary"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) =>
        {
            InitFilters();
            InitPager();
            LoadComplaints();
        };

        btnFileNew.Click += (_, __) => FileNew();

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadComplaints();
        };
        cmbCategoryFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadComplaints();
        };
        cmbStatusFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadComplaints();
        };
        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadComplaints();
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
        AppTheme.StyleInput(cmbCategoryFilter);
        AppTheme.StyleInput(cmbStatusFilter);
        AppTheme.StyleSuccessButton(btnFileNew);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridComplaints);
        AppTheme.StyleLabel(lblCategoryFilter);
        AppTheme.StyleLabel(lblStatusFilter);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;
        chkShowArchived.BackColor = System.Drawing.Color.Transparent;

        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);

        gridComplaints.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        gridComplaints.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
    }

    private void InitFilters()
    {
        cmbCategoryFilter.Items.Clear();
        cmbCategoryFilter.Items.Add("All");
        cmbCategoryFilter.Items.AddRange(Categories);
        cmbCategoryFilter.SelectedIndex = 0;

        cmbStatusFilter.Items.Clear();
        cmbStatusFilter.Items.Add("All");
        cmbStatusFilter.Items.AddRange(Statuses);
        cmbStatusFilter.SelectedIndex = 0;
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    // ============================================================
    //  DATA
    // ============================================================

    private void LoadComplaints()
    {
        try
        {
            var filter = new ComplaintFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                Category = cmbCategoryFilter.SelectedItem?.ToString() ?? "All",
                Status = cmbStatusFilter.SelectedItem?.ToString() ?? "All",
                ShowArchived = chkShowArchived.Checked
            };

            _allRows = _controller.GetComplaints(filter);

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);
            RenderPage();

            lblStatus.ForeColor = AppTheme.TextSecondary;
            lblStatus.Text = $"{_allRows.Count} complaint(s) — "
                + $"{_allRows.Count(x => x.Status == "Open")} open · "
                + $"{_allRows.Count(x => x.Status == "In Progress")} in progress · "
                + $"{_allRows.Count(x => x.Status == "Resolved")} resolved";
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
        List<ComplaintRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridComplaints.DataSource = pageRows;

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
        for (int i = gridComplaints.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridComplaints.Columns[i];
            if (col.Name == "colView" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridComplaints.Columns.RemoveAt(i);
        }

        gridComplaints.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colView", "View", "Open Complaint", "primary", 100));

        if (chkShowArchived.Checked)
        {
            gridComplaints.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 120));

            AppTheme.ApplyStyleToButtonColumn(gridComplaints, "colUnarchive", "success");
        }
        else
        {
            gridComplaints.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 110));

            AppTheme.ApplyStyleToButtonColumn(gridComplaints, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridComplaints, "colView", "primary");

        gridComplaints.CellContentClick -= Grid_CellContentClick;
        gridComplaints.CellContentClick += Grid_CellContentClick;
    }

    // ============================================================
    //  COLUMN LAYOUT
    // ============================================================

    private void StyleColumns()
    {
        var grid = gridComplaints;

        if (grid.Columns.Contains("ComplaintId")) grid.Columns["ComplaintId"].Visible = false;
        if (grid.Columns.Contains("IsArchived")) grid.Columns["IsArchived"].Visible = false;

        SetColumnFixed("ComplaintCode", "Code", 90, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Category", "Category", 140, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Severity", "Severity", 90, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("Status", "Status", 120, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("AssignedTo", "Assigned To", 180, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("SubmittedAt"))
        {
            var c = grid.Columns["SubmittedAt"];
            c.HeaderText = "Submitted";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = 130;
            c.MinimumWidth = 130;
            c.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        SetColumnFill("Subject", "Subject", 60, 180, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFill("CustomerName", "Customer", 40, 130, DataGridViewContentAlignment.MiddleLeft);

        foreach (var name in new[] { "colView", "colArchive", "colUnarchive" })
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

        grid.CellFormatting -= Grid_CellFormatting;
        grid.CellFormatting += Grid_CellFormatting;
    }

    private void SetColumnFixed(string name, string header, int width, DataGridViewContentAlignment align)
    {
        if (!gridComplaints.Columns.Contains(name)) return;
        var c = gridComplaints.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridComplaints.Columns.Contains(name)) return;
        var c = gridComplaints.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridComplaints.Rows.Count) return;
        var row = gridComplaints.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;

        var colName = gridComplaints.Columns[e.ColumnIndex].Name;

        if (colName == "Status")
        {
            var status = row.Cells["Status"]?.Value?.ToString() ?? "";
            e.CellStyle.ForeColor = status switch
            {
                "Open" => Color.FromArgb(30, 100, 200),
                "In Progress" => Color.FromArgb(200, 130, 0),
                "Resolved" => Color.FromArgb(22, 130, 60),
                "Closed" => Color.FromArgb(120, 120, 120),
                "Rejected" => Color.FromArgb(190, 40, 40),
                _ => AppTheme.TextPrimary
            };
            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        if (colName == "Severity")
        {
            var sev = row.Cells["Severity"]?.Value?.ToString() ?? "";
            e.CellStyle.ForeColor = sev switch
            {
                "Critical" => Color.FromArgb(190, 40, 40),
                "High" => Color.FromArgb(220, 100, 40),
                "Medium" => Color.FromArgb(200, 130, 0),
                "Low" => Color.FromArgb(22, 130, 60),
                _ => AppTheme.TextPrimary
            };
            e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }
    }

    private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var col = gridComplaints.Columns[e.ColumnIndex];
        var id = GetComplaintIdAtRow(e.RowIndex);
        if (id is null) return;

        if (col.Name == "colView") OpenDetail(id.Value);
        else if (col.Name == "colArchive") ArchiveComplaint(id.Value);
        else if (col.Name == "colUnarchive") UnarchiveComplaint(id.Value);
    }

    private int? GetComplaintIdAtRow(int rowIndex)
    {
        var cell = gridComplaints.Rows[rowIndex].Cells["ComplaintId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    // ============================================================
    //  ACTIONS
    // ============================================================

    private void FileNew()
    {
        using var dlg = new FrmComplaintEditor(null);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Complaint filed.";
            LoadComplaints();
        }
    }

    private void OpenDetail(int complaintId)
    {
        using var dlg = new FrmComplaintDetail(complaintId);
        dlg.ShowDialog(this);
        LoadComplaints();
    }

    private void ArchiveComplaint(int complaintId)
    {
        if (!UserSession.IsAdmin && !UserSession.IsManager)
        {
            MessageBox.Show("Only Admin and Manager can archive complaints.",
                "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            "Archive this complaint? It will be hidden from the active list.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var c = db.Complaints.FirstOrDefault(x => x.ComplaintId == complaintId);
            if (c is null) return;

            c.IsArchived = true;
            c.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "Complaint", complaintId,
                $"Complaint '{c.ComplaintCode}' archived");

            lblStatus.ForeColor = AppTheme.Danger;
            lblStatus.Text = "Complaint archived.";
            LoadComplaints();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void UnarchiveComplaint(int complaintId)
    {
        if (!UserSession.IsAdmin && !UserSession.IsManager)
        {
            MessageBox.Show("Only Admin and Manager can unarchive complaints.",
                "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();
            var c = db.Complaints.FirstOrDefault(x => x.ComplaintId == complaintId);
            if (c is null) return;

            c.IsArchived = false;
            c.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();

            ActivityLogger.Log("Unarchive", "Complaint", complaintId,
                $"Complaint '{c.ComplaintCode}' unarchived");

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Complaint restored.";
            LoadComplaints();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}