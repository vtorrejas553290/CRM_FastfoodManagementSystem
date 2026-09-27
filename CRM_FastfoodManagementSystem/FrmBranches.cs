using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmBranches : Form
{
    private readonly IBranchController _controller = new BranchController();

    private List<BranchRow> _allRows = new();
    private int _currentPage = 1;

    public FrmBranches()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridBranches,
            ("colEdit", "warning"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) =>
        {
            InitPager();
            LoadBranches();
        };

        btnAddBranch.Click += BtnAddBranch_Click;
        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadBranches();
        };
        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadBranches();
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

        Text = "Branch Management";

        pnlTop.BackColor = AppTheme.Surface;
        pnlPager.BackColor = AppTheme.Surface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSuccessButton(btnAddBranch);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridBranches);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;

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

    private void LoadBranches()
    {
        try
        {
            var filter = new BranchFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                ShowArchived = chkShowArchived.Checked,
                CompanyId = UserSession.CompanyId
            };

            _allRows = _controller.GetBranches(filter);

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();
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
        List<BranchRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridBranches.DataSource = pageRows;
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
        for (int i = gridBranches.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridBranches.Columns[i];
            if (col.Name == "colEdit" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridBranches.Columns.RemoveAt(i);
        }

        gridBranches.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEdit", "Edit", "Edit", "warning", 90));

        if (chkShowArchived.Checked)
        {
            gridBranches.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 110));

            AppTheme.ApplyStyleToButtonColumn(gridBranches, "colUnarchive", "success");
        }
        else
        {
            gridBranches.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 100));

            AppTheme.ApplyStyleToButtonColumn(gridBranches, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridBranches, "colEdit", "warning");

        gridBranches.CellContentClick -= GridBranches_CellContentClick;
        gridBranches.CellContentClick += GridBranches_CellContentClick;
    }

    private void StyleColumns()
    {
        var grid = gridBranches;

        if (grid.Columns.Contains("BranchId")) grid.Columns["BranchId"].Visible = false;

        SetColumnFixed("BranchCode", "Code", 120, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Status", "Status", 110, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("ContactNumber", "Contact", 150, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("CreatedAt", "Created", 130, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("BranchName", "Branch Name", 60, 200, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFill("Address", "Address", 60, 200, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("CreatedAt"))
            grid.Columns["CreatedAt"].DefaultCellStyle.Format = "yyyy-MM-dd";

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
        if (!gridBranches.Columns.Contains(name)) return;
        var c = gridBranches.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridBranches.Columns.Contains(name)) return;
        var c = gridBranches.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridBranches.Rows.Count) return;
        var row = gridBranches.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridBranches.Columns[e.ColumnIndex].Name != "Status") return;

        var status = row.Cells["Status"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = status switch
        {
            "Active" => Color.FromArgb(22, 130, 60),
            "Archived" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    // ============================================================
    //  ACTION HANDLERS
    // ============================================================

    private void GridBranches_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridBranches.Columns[e.ColumnIndex];

        if (column.Name == "colEdit")
            HandleEdit(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
        else if (column.Name == "colUnarchive")
            HandleUnarchive(e.RowIndex);
    }

    private int? GetBranchIdAtRow(int rowIndex)
    {
        var cell = gridBranches.Rows[rowIndex].Cells["BranchId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void HandleEdit(int rowIndex)
    {
        var id = GetBranchIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmBranchEditor(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Update", "Branch", id.Value,
                $"Branch #{id.Value} updated");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "Branch updated.";
            LoadBranches();
        }
    }

    private void HandleArchive(int rowIndex)
    {
        var id = GetBranchIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Archive this branch? It will no longer be assignable to users.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        if (!_controller.SetBranchActive(id.Value, UserSession.CompanyId, false, out var error))
        {
            MessageBox.Show(error ?? "Failed to archive branch.", "Error");
            return;
        }

        ActivityLogger.Log("Archive", "Branch", id.Value,
            $"Branch #{id.Value} archived");

        lblStatus.ForeColor = AppTheme.Danger;
        lblStatus.Text = "Branch archived.";
        LoadBranches();
    }

    private void HandleUnarchive(int rowIndex)
    {
        var id = GetBranchIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Unarchive this branch? It will be assignable to users again.",
            "Confirm Unarchive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        if (!_controller.SetBranchActive(id.Value, UserSession.CompanyId, true, out var error))
        {
            MessageBox.Show(error ?? "Failed to unarchive branch.", "Error");
            return;
        }

        ActivityLogger.Log("Unarchive", "Branch", id.Value,
            $"Branch #{id.Value} unarchived");

        lblStatus.ForeColor = AppTheme.SuccessGreen;
        lblStatus.Text = "Branch unarchived.";
        LoadBranches();
    }

    private void BtnAddBranch_Click(object? sender, EventArgs e)
    {
        using var dlg = new FrmBranchEditor(null);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Create", "Branch", null,
                "New branch added");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "Branch added.";
            LoadBranches();
        }
    }
}