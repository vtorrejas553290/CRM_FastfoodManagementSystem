using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmUserManagement : Form
{
    private readonly IUserController _controller = new UserController();
    private readonly IBranchController _branchController = new BranchController();

    private List<UserRow> _allRows = new();
    private int _currentPage = 1;

    // Sentinel value used in the branch dropdown for "unassigned"
    private const int UnassignedBranchSentinel = -1;

    public FrmUserManagement()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridUsers,
            ("colEdit", "warning"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) =>
        {
            InitPager();
            InitRoleFilter();
            InitBranchFilter();
            LoadUsers();
        };

        btnAddUser.Click += BtnAddUser_Click;
        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadUsers();
        };
        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadUsers();
        };

        cmbRoleFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadUsers();
        };

        // NEW — branch filter
        cmbBranchFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadUsers();
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

        Text = UserSession.IsSuperAdmin
            ? "User Management — Tenant Admins"
            : "User Management — Managers and Staff";

        pnlTop.BackColor = AppTheme.Surface;
        pnlPager.BackColor = AppTheme.Surface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSuccessButton(btnAddUser);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridUsers);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleLabel(lblRoleFilter);
        AppTheme.StyleInput(cmbRoleFilter);

        // NEW — branch filter styling
        AppTheme.StyleLabel(lblBranchFilter);
        AppTheme.StyleInput(cmbBranchFilter);

        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);

        btnAddUser.Visible = !UserSession.IsSuperAdmin && GetAllowedRoleCodes().Length > 0;
        chkShowArchived.Visible = !UserSession.IsSuperAdmin;

        lblRoleFilter.Visible = !UserSession.IsSuperAdmin;
        cmbRoleFilter.Visible = !UserSession.IsSuperAdmin;

        // NEW — branch filter is visible only for Enterprise-plan tenant admins
        bool showBranchFilter = UserSession.HasBranching
                                && UserSession.IsAdmin
                                && !UserSession.IsSuperAdmin;
        lblBranchFilter.Visible = showBranchFilter;
        cmbBranchFilter.Visible = showBranchFilter;
    }

    private static string[] GetAllowedRoleCodes()
    {
        if (UserSession.IsSuperAdmin) return new[] { "ADMIN" };
        if (UserSession.IsAdmin) return new[] { "MANAGER", "STAFF" };
        return Array.Empty<string>();
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    private void InitRoleFilter()
    {
        cmbRoleFilter.Items.Clear();
        cmbRoleFilter.Items.AddRange(new object[] { "All Roles", "Manager", "Staff" });
        cmbRoleFilter.SelectedIndex = 0;
    }

    // NEW — populate the branch filter
    private void InitBranchFilter()
    {
        cmbBranchFilter.Items.Clear();

        if (!UserSession.HasBranching || !UserSession.IsAdmin || UserSession.IsSuperAdmin)
            return;

        try
        {
            var filter = new BranchFilter
            {
                Search = "",
                ShowArchived = false,
                CompanyId = UserSession.CompanyId
            };

            var branches = _branchController.GetBranches(filter);

            var items = new List<BranchFilterItem>
            {
                new BranchFilterItem { BranchIdFilter = null, Display = "All Branches" }
            };

            foreach (var b in branches.OrderBy(x => x.BranchCode))
            {
                items.Add(new BranchFilterItem
                {
                    BranchIdFilter = b.BranchId,
                    Display = $"{b.BranchCode} — {b.BranchName}"
                });
            }

            items.Add(new BranchFilterItem
            {
                BranchIdFilter = UnassignedBranchSentinel,
                Display = "Unassigned"
            });

            cmbBranchFilter.DisplayMember = nameof(BranchFilterItem.Display);
            cmbBranchFilter.ValueMember = nameof(BranchFilterItem.BranchIdFilter);
            cmbBranchFilter.DataSource = items;
            cmbBranchFilter.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load branches: " + ex.Message, "Error");
        }
    }

    private string GetSelectedRoleCode()
    {
        var pick = cmbRoleFilter.SelectedItem?.ToString() ?? "All Roles";
        return pick switch
        {
            "Manager" => "MANAGER",
            "Staff" => "STAFF",
            _ => "All"
        };
    }

    // NEW — read selected branch filter value
    private int? GetSelectedBranchFilter()
    {
        if (!cmbBranchFilter.Visible) return null;

        if (cmbBranchFilter.SelectedItem is BranchFilterItem item)
            return item.BranchIdFilter;

        return null;
    }

    private void LoadUsers()
    {
        try
        {
            if (!UserSession.IsSuperAdmin && !UserSession.IsAdmin)
            {
                _allRows = new List<UserRow>();
                RenderPage();
                return;
            }

            var filter = new UserFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                ShowArchived = chkShowArchived.Checked,
                IsSuperAdmin = UserSession.IsSuperAdmin,
                IsAdmin = UserSession.IsAdmin,
                CurrentUserId = UserSession.UserId,

                CompanyId = UserSession.CompanyId,
                ShowAllTenants = UserSession.IsSuperAdmin,

                RoleCodeFilter = UserSession.IsSuperAdmin ? "All" : GetSelectedRoleCode(),

                // NEW — branch filter
                BranchIdFilter = UserSession.IsSuperAdmin ? null : GetSelectedBranchFilter()
            };

            _allRows = _controller.GetUsers(filter);

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
        List<UserRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridUsers.DataSource = pageRows;
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
        for (int i = gridUsers.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridUsers.Columns[i];
            if (col.Name == "colEdit" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridUsers.Columns.RemoveAt(i);
        }

        if (UserSession.IsSuperAdmin)
            return;

        gridUsers.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEdit", "Edit", "Edit", "warning", 90));

        if (chkShowArchived.Checked)
        {
            gridUsers.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 110));

            AppTheme.ApplyStyleToButtonColumn(gridUsers, "colUnarchive", "success");
        }
        else
        {
            gridUsers.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 100));

            AppTheme.ApplyStyleToButtonColumn(gridUsers, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridUsers, "colEdit", "warning");

        gridUsers.CellContentClick -= GridUsers_CellContentClick;
        gridUsers.CellContentClick += GridUsers_CellContentClick;
    }

    private void StyleColumns()
    {
        var grid = gridUsers;

        if (grid.Columns.Contains("UserId")) grid.Columns["UserId"].Visible = false;

        if (UserSession.IsSuperAdmin && grid.Columns.Contains("TenantName"))
        {
            SetColumnFixed("TenantName", "Tenant", 180, DataGridViewContentAlignment.MiddleLeft);
        }
        else if (grid.Columns.Contains("TenantName"))
        {
            grid.Columns["TenantName"].Visible = false;
        }

        if (grid.Columns.Contains("CompanyId")) grid.Columns["CompanyId"].Visible = false;

        SetColumnFixed("Username", "Username", 140, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Role", "Role", 130, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Status", "Status", 110, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("CreatedAt", "Created", 130, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("FullName", "Full Name", 60, 160, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFill("Email", "Email", 40, 160, DataGridViewContentAlignment.MiddleLeft);

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
        if (!gridUsers.Columns.Contains(name)) return;
        var c = gridUsers.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridUsers.Columns.Contains(name)) return;
        var c = gridUsers.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridUsers.Rows.Count) return;
        var row = gridUsers.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridUsers.Columns[e.ColumnIndex].Name != "Status") return;

        var status = row.Cells["Status"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = status switch
        {
            "Active" => Color.FromArgb(22, 130, 60),
            "Archived" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    private void GridUsers_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridUsers.Columns[e.ColumnIndex];

        if (column.Name == "colEdit")
            HandleEdit(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
        else if (column.Name == "colUnarchive")
            HandleUnarchive(e.RowIndex);
    }

    private int? GetUserIdAtRow(int rowIndex)
    {
        var cell = gridUsers.Rows[rowIndex].Cells["UserId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void HandleEdit(int rowIndex)
    {
        var id = GetUserIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmUserEditor(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Update", "User", id.Value,
                $"User #{id.Value} updated");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "User updated.";
            LoadUsers();
        }
    }

    private void HandleArchive(int rowIndex)
    {
        var id = GetUserIdAtRow(rowIndex);
        if (id is null) return;

        if (id.Value == UserSession.UserId)
        {
            MessageBox.Show("You cannot archive your own account.", "Not allowed");
            return;
        }

        var confirm = MessageBox.Show(
            "Archive this user? They will no longer be able to log in.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext(UserSession.CompanyId);
            var user = db.Users.FirstOrDefault(x => x.UserId == id.Value);
            if (user is null) return;

            user.IsActive = false;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "User", id.Value,
                $"User '{user.Username}' archived");

            lblStatus.ForeColor = AppTheme.Danger;
            lblStatus.Text = "User archived.";
            LoadUsers();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void HandleUnarchive(int rowIndex)
    {
        var id = GetUserIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Unarchive this user? They will be able to log in again.",
            "Confirm Unarchive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext(UserSession.CompanyId);
            var user = db.Users.FirstOrDefault(x => x.UserId == id.Value);
            if (user is null) return;

            user.IsActive = true;
            db.SaveChanges();

            ActivityLogger.Log("Unarchive", "User", id.Value,
                $"User '{user.Username}' unarchived");

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "User unarchived.";
            LoadUsers();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void BtnAddUser_Click(object? sender, EventArgs e)
    {
        using var dlg = new FrmUserEditor(null);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Create", "User", null,
                "New user added");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "User added.";
            LoadUsers();
        }
    }

    // NEW — inner class for the branch dropdown data source
    private sealed class BranchFilterItem
    {
        public int? BranchIdFilter { get; set; }
        public string Display { get; set; } = "";
    }
}