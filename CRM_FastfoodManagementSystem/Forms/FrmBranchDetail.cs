using CRM.domain.Entities;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmBranchDetail : Form
{
    private readonly int _branchId;

    public FrmBranchDetail(int branchId)
    {
        _branchId = branchId;
        InitializeComponent();
        ApplyTheme();

        btnClose.Click += (_, __) => Close();
        Load += FrmBranchDetail_Load;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        AppTheme.StyleGrid(gridManagers);
        AppTheme.StyleGrid(gridStaff);
        AppTheme.StyleSecondaryButton(btnClose);

        AppTheme.StyleLabel(lblManagersTitle);
        AppTheme.StyleLabel(lblStaffTitle);
    }

    private void FrmBranchDetail_Load(object? sender, EventArgs e)
    {
        try
        {
            using var db = AppServices.CreateTenantContext(UserSession.CompanyId);

            // Load the branch itself
            var branch = db.Branches
                .AsNoTracking()
                .FirstOrDefault(b => b.BranchId == _branchId);

            if (branch is null)
            {
                MessageBox.Show("Branch not found.", "Error");
                Close();
                return;
            }

            // Header info
            Text = $"Branch Detail — {branch.BranchCode}";
            lblBranchName.Text = branch.BranchName;
            lblBranchCode.Text = $"Code: {branch.BranchCode}";
            lblBranchMeta.Text =
                $"Status: {(branch.IsActive ? "Active" : "Archived")}   •   " +
                $"Contact: {(string.IsNullOrWhiteSpace(branch.ContactNumber) ? "—" : branch.ContactNumber)}   •   " +
                $"Address: {(string.IsNullOrWhiteSpace(branch.Address) ? "—" : branch.Address)}";

            // Load assigned users
            var users = db.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .Where(u => u.BranchId == _branchId)
                .OrderBy(u => u.FullName)
                .Select(u => new
                {
                    u.UserId,
                    u.Username,
                    u.FullName,
                    u.Email,
                    u.ContactNumber,
                    u.IsActive,
                    RoleCode = u.Role != null ? u.Role.RoleCode : "",
                    RoleName = u.Role != null ? u.Role.RoleName : ""
                })
                .ToList();

            // Managers grid
            var managers = users
                .Where(u => u.RoleCode == "MANAGER")
                .Select(u => new
                {
                    Name = u.FullName,
                    Username = u.Username,
                    Contact = string.IsNullOrWhiteSpace(u.ContactNumber) ? "—" : u.ContactNumber,
                    Email = string.IsNullOrWhiteSpace(u.Email) ? "—" : u.Email,
                    Status = u.IsActive ? "Active" : "Archived"
                })
                .ToList();

            gridManagers.DataSource = managers;
            StyleGrid(gridManagers);
            lblManagersTitle.Text = $"Managers ({managers.Count})";

            // Staff grid
            var staff = users
                .Where(u => u.RoleCode == "STAFF")
                .Select(u => new
                {
                    Name = u.FullName,
                    Username = u.Username,
                    Contact = string.IsNullOrWhiteSpace(u.ContactNumber) ? "—" : u.ContactNumber,
                    Email = string.IsNullOrWhiteSpace(u.Email) ? "—" : u.Email,
                    Status = u.IsActive ? "Active" : "Archived"
                })
                .ToList();

            gridStaff.DataSource = staff;
            StyleGrid(gridStaff);
            lblStaffTitle.Text = $"Staff ({staff.Count})";

            // Empty states
            if (managers.Count == 0)
                lblManagersTitle.Text = "Managers (0) — no manager assigned to this branch";

            if (staff.Count == 0)
                lblStaffTitle.Text = "Staff (0) — no staff assigned to this branch";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void StyleGrid(DataGridView grid)
    {
        if (grid.Columns.Contains("Name"))
        {
            grid.Columns["Name"].HeaderText = "Name";
            grid.Columns["Name"].FillWeight = 40;
        }
        if (grid.Columns.Contains("Username"))
        {
            grid.Columns["Username"].HeaderText = "Username";
            grid.Columns["Username"].FillWeight = 25;
        }
        if (grid.Columns.Contains("Contact"))
        {
            grid.Columns["Contact"].HeaderText = "Contact";
            grid.Columns["Contact"].FillWeight = 20;
        }
        if (grid.Columns.Contains("Email"))
        {
            grid.Columns["Email"].HeaderText = "Email";
            grid.Columns["Email"].FillWeight = 30;
        }
        if (grid.Columns.Contains("Status"))
        {
            grid.Columns["Status"].HeaderText = "Status";
            grid.Columns["Status"].FillWeight = 15;
        }
    }
}