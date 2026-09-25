using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmUserManagement : Form
{
    public FrmUserManagement()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridUsers,
            ("colEdit", "warning"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) => LoadUsers();
        btnRefresh.Click += (_, __) => LoadUsers();
        btnAddUser.Click += BtnAddUser_Click;
        chkShowArchived.CheckedChanged += (_, __) => LoadUsers();
        txtSearch.TextChanged += (_, __) => LoadUsers();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);

        Text = UserSession.IsSuperAdmin
            ? "User Management — Admins"
            : "User Management — Managers and Staff";

        pnlTop.BackColor = AppTheme.Surface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleSuccessButton(btnAddUser);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridUsers);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;

        btnAddUser.Visible = GetAllowedRoleCodes().Length > 0;
    }

    private static string[] GetAllowedRoleCodes()
    {
        if (UserSession.IsSuperAdmin) return new[] { "ADMIN" };
        if (UserSession.IsAdmin) return new[] { "MANAGER", "STAFF" };
        return Array.Empty<string>();
    }

    private void LoadUsers()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var search = txtSearch.Text.Trim().ToLower();
            var showArchived = chkShowArchived.Checked;

            var query = db.Users.Include(x => x.Role).AsNoTracking();

            if (UserSession.IsSuperAdmin)
                query = query.Where(x => x.Role!.RoleCode == "ADMIN" || x.UserId == UserSession.UserId);
            else if (UserSession.IsAdmin)
                query = query.Where(x => x.Role!.RoleCode == "MANAGER" || x.Role!.RoleCode == "STAFF");
            else
            {
                gridUsers.DataSource = new List<object>();
                return;
            }

            // Show only the relevant set: archived when toggled, active otherwise
            if (showArchived)
                query = query.Where(x => !x.IsActive);
            else
                query = query.Where(x => x.IsActive);

            var users = query
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    x.Username.ToLower().Contains(search) ||
                    x.FullName.ToLower().Contains(search))
                .OrderBy(x => x.UserId)
                .Select(x => new
                {
                    x.UserId,
                    x.Username,
                    x.FullName,
                    x.Email,
                    Role = x.Role!.RoleName,
                    Status = x.IsActive ? "Active" : "Archived",
                    x.CreatedAt
                })
                .ToList();

            gridUsers.DataSource = users;
            BuildActionColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BuildActionColumns()
    {
        for (int i = gridUsers.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridUsers.Columns[i];
            if (col.Name == "colEdit" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridUsers.Columns.RemoveAt(i);
        }

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
            using var db = AppServices.CreateTenantContext();
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
            using var db = AppServices.CreateTenantContext();
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
}