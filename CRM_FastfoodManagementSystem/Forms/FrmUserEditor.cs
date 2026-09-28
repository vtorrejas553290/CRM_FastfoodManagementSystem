using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmUserEditor : Form
{
    private readonly int? _userId;

    private TextBox _txtUsername = new();
    private TextBox _txtPassword = new();
    private TextBox _txtFullName = new();
    private TextBox _txtEmail = new();
    private TextBox _txtContact = new();
    private ComboBox _cmbRole = new();
    private Label _lblBranch = new();
    private ComboBox _cmbBranch = new();
    private CheckBox _chkActive = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

    private readonly IBranchController _branchController = new BranchController();

    // Holds the current user's BranchId when editing (null = none)
    private int? _loadedBranchId = null;

    public FrmUserEditor() : this(null) { }

    public FrmUserEditor(int? userId)
    {
        _userId = userId;
        BuildUi();
        Load += FrmUserEditor_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = _userId is null ? "Add User" : "Edit User";
        ClientSize = new Size(500, 500);

        var lblUsername = new Label { Text = "Username:", Location = new Point(20, 20), AutoSize = true };
        AppTheme.StyleLabel(lblUsername);
        _txtUsername = new TextBox { Location = new Point(150, 18), Width = 300 };
        AppTheme.StyleInput(_txtUsername);

        var lblPassword = new Label { Text = "Password:", Location = new Point(20, 60), AutoSize = true };
        AppTheme.StyleLabel(lblPassword);
        _txtPassword = new TextBox { Location = new Point(150, 58), Width = 300, UseSystemPasswordChar = true };
        AppTheme.StyleInput(_txtPassword);

        var lblFullName = new Label { Text = "Full Name:", Location = new Point(20, 100), AutoSize = true };
        AppTheme.StyleLabel(lblFullName);
        _txtFullName = new TextBox { Location = new Point(150, 98), Width = 300 };
        AppTheme.StyleInput(_txtFullName);

        var lblEmail = new Label { Text = "Email:", Location = new Point(20, 140), AutoSize = true };
        AppTheme.StyleLabel(lblEmail);
        _txtEmail = new TextBox { Location = new Point(150, 138), Width = 300 };
        AppTheme.StyleInput(_txtEmail);

        var lblContact = new Label { Text = "Contact:", Location = new Point(20, 180), AutoSize = true };
        AppTheme.StyleLabel(lblContact);
        _txtContact = new TextBox { Location = new Point(150, 178), Width = 300 };
        AppTheme.StyleInput(_txtContact);

        var lblRole = new Label { Text = "Role:", Location = new Point(20, 220), AutoSize = true };
        AppTheme.StyleLabel(lblRole);
        _cmbRole = new ComboBox
        {
            Location = new Point(150, 218),
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        AppTheme.StyleInput(_cmbRole);
        _cmbRole.SelectedIndexChanged += (_, __) => UpdateBranchVisibilityByRole();

        // ---- Branch row (hidden for ADMIN role and for plans without Branching) ----
        _lblBranch = new Label { Text = "Branch:", Location = new Point(20, 260), AutoSize = true };
        AppTheme.StyleLabel(_lblBranch);

        _cmbBranch = new ComboBox
        {
            Location = new Point(150, 258),
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        AppTheme.StyleInput(_cmbBranch);

        _chkActive = new CheckBox
        {
            Text = "Is Active",
            Location = new Point(150, 298),
            Checked = true,
            AutoSize = true
        };
        _chkActive.Font = AppTheme.FontBody;
        _chkActive.ForeColor = AppTheme.TextPrimary;

        _btnSave = new Button { Text = "Save", Location = new Point(150, 340), Width = 140 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(300, 340), Width = 140 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label
        {
            Location = new Point(20, 395),
            AutoSize = true,
            MaximumSize = new Size(460, 80)
        };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[]
        {
            lblUsername, _txtUsername, lblPassword, _txtPassword,
            lblFullName, _txtFullName, lblEmail, _txtEmail,
            lblContact, _txtContact, lblRole, _cmbRole,
            _lblBranch, _cmbBranch,
            _chkActive, _btnSave, _btnCancel, _lblStatus
        });
    }

    private void FrmUserEditor_Load(object? sender, EventArgs e)
    {
        LoadRoles();
        LoadBranches();

        if (_userId is null)
        {
            // Add mode — default role selection determines branch visibility
            UpdateBranchVisibilityByRole();
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();
            var u = db.Users.AsNoTracking().FirstOrDefault(x => x.UserId == _userId.Value);
            if (u is null) { Close(); return; }

            _txtUsername.Text = u.Username;
            _txtUsername.Enabled = false;      // immutable
            _txtFullName.Text = u.FullName;
            _txtEmail.Text = u.Email;
            _txtContact.Text = u.ContactNumber;
            _cmbRole.SelectedValue = u.RoleId;
            _chkActive.Checked = u.IsActive;

            // Load the current branch selection
            _loadedBranchId = u.BranchId;
            if (_cmbBranch.Visible && u.BranchId.HasValue)
            {
                _cmbBranch.SelectedValue = u.BranchId.Value;
            }

            // On edit, password field shows a note instead of allowing edit
            _txtPassword.Enabled = false;
            _txtPassword.Text = "(unchanged)";

            UpdateBranchVisibilityByRole();
        }
        catch (Exception ex)
        {
            _lblStatus.ForeColor = AppTheme.Error;
            _lblStatus.Text = ex.Message;
        }
    }

    private void LoadRoles()
    {
        using var db = AppServices.CreateTenantContext();

        var allowed = GetAllowedRoleCodes();

        var roles = db.Roles
            .AsNoTracking()
            .Where(x => allowed.Contains(x.RoleCode))
            .OrderBy(x => x.RoleId)
            .Select(x => new { x.RoleId, x.RoleName })
            .ToList();

        _cmbRole.DataSource = roles;
        _cmbRole.DisplayMember = "RoleName";
        _cmbRole.ValueMember = "RoleId";
    }

    /// <summary>
    /// Loads the branches available for assignment in this tenant.
    /// Adds a "(None)" option at the top.
    /// Only runs if the tenant's plan includes Branching.
    /// </summary>
    private void LoadBranches()
    {
        _cmbBranch.Items.Clear();

        if (!UserSession.HasBranching || UserSession.IsSuperAdmin)
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

            // Build a list with a "(None)" placeholder first
            var list = new List<BranchPickItem>
            {
                new BranchPickItem { BranchId = 0, Display = "(None)" }
            };

            foreach (var b in branches.OrderBy(x => x.BranchCode))
            {
                list.Add(new BranchPickItem
                {
                    BranchId = b.BranchId,
                    Display = $"{b.BranchCode} — {b.BranchName}"
                });
            }

            _cmbBranch.DataSource = list;
            _cmbBranch.DisplayMember = nameof(BranchPickItem.Display);
            _cmbBranch.ValueMember = nameof(BranchPickItem.BranchId);

            // If editing and the user already has a branch, preselect it.
            // Otherwise default to "(None)".
            if (_loadedBranchId.HasValue &&
                list.Any(x => x.BranchId == _loadedBranchId.Value))
            {
                _cmbBranch.SelectedValue = _loadedBranchId.Value;
            }
            else
            {
                _cmbBranch.SelectedValue = 0;
            }
        }
        catch (Exception ex)
        {
            _lblStatus.ForeColor = AppTheme.Error;
            _lblStatus.Text = "Failed to load branches: " + ex.Message;
        }
    }

    /// <summary>
    /// Show the Branch dropdown only when:
    ///  - the current tenant plan has Branching,
    ///  - we're not SuperAdmin, and
    ///  - the selected role is MANAGER or STAFF (not ADMIN).
    /// </summary>
    private void UpdateBranchVisibilityByRole()
    {
        bool planHasBranching = UserSession.HasBranching && !UserSession.IsSuperAdmin;

        string? roleCode = null;
        if (_cmbRole.SelectedValue is int roleId)
        {
            // roleId → roleCode mapping is inferred from selected item's display.
            // The Role entity has RoleCode but our DataSource only had RoleId + RoleName.
            // We'll look it up cheaply from the DB.
            try
            {
                using var db = AppServices.CreateTenantContext();
                roleCode = db.Roles.AsNoTracking()
                    .Where(r => r.RoleId == roleId)
                    .Select(r => r.RoleCode)
                    .FirstOrDefault();
            }
            catch { roleCode = null; }
        }

        bool roleCanHaveBranch = roleCode == "MANAGER" || roleCode == "STAFF";

        bool show = planHasBranching && roleCanHaveBranch;

        _lblBranch.Visible = show;
        _cmbBranch.Visible = show;
    }

    private static string[] GetAllowedRoleCodes()
    {
        if (UserSession.IsSuperAdmin) return new[] { "ADMIN" };
        if (UserSession.IsAdmin) return new[] { "MANAGER", "STAFF" };
        return Array.Empty<string>();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        // ============ VALIDATION ============
        if (string.IsNullOrWhiteSpace(_txtUsername.Text) || _txtUsername.Text.Trim().Length < 3)
        {
            _lblStatus.Text = "Username must be at least 3 characters.";
            return;
        }

        if (_userId is null &&
            (string.IsNullOrWhiteSpace(_txtPassword.Text) || _txtPassword.Text.Length < 6))
        {
            _lblStatus.Text = "Password must be at least 6 characters.";
            return;
        }

        if (string.IsNullOrWhiteSpace(_txtFullName.Text) || _txtFullName.Text.Trim().Length < 3)
        {
            _lblStatus.Text = "Full Name must be at least 3 characters.";
            return;
        }

        if (!string.IsNullOrWhiteSpace(_txtEmail.Text))
        {
            if (!Regex.IsMatch(_txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _lblStatus.Text = "Email Address is not in a valid format.";
                return;
            }
        }

        if (_cmbRole.SelectedValue is null)
        {
            _lblStatus.Text = "Please select a role.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            int roleId = (int)_cmbRole.SelectedValue;
            var selectedRole = db.Roles.AsNoTracking().FirstOrDefault(x => x.RoleId == roleId);

            if (selectedRole is null)
            {
                _lblStatus.Text = "Selected role is invalid.";
                return;
            }

            var allowed = GetAllowedRoleCodes();
            if (!allowed.Contains(selectedRole.RoleCode))
            {
                _lblStatus.Text = $"You are not allowed to assign the {selectedRole.RoleName} role.";
                return;
            }

            // Determine the BranchId to save
            int? branchIdToSave = null;

            bool roleCanHaveBranch = selectedRole.RoleCode == "MANAGER"
                                   || selectedRole.RoleCode == "STAFF";
            bool planHasBranching = UserSession.HasBranching && !UserSession.IsSuperAdmin;

            if (planHasBranching && roleCanHaveBranch && _cmbBranch.Visible)
            {
                if (_cmbBranch.SelectedValue is int pickedId && pickedId > 0)
                    branchIdToSave = pickedId;
                // pickedId == 0  →  "(None)"  →  leave null
            }
            // ADMIN or plan without Branching  →  branchIdToSave stays null

            if (_userId is null)
            {
                // ============ ADD ============
                if (db.Users.Any(x => x.Username == _txtUsername.Text.Trim()))
                {
                    _lblStatus.Text = "Username already exists.";
                    return;
                }

                db.Users.Add(new User
                {
                    Username = _txtUsername.Text.Trim(),
                    PasswordHash = PasswordHasher.Hash(_txtPassword.Text),
                    FullName = _txtFullName.Text.Trim(),
                    Email = _txtEmail.Text.Trim(),
                    ContactNumber = _txtContact.Text.Trim(),
                    RoleId = roleId,
                    IsActive = _chkActive.Checked,
                    CreatedAt = DateTime.UtcNow,
                    BranchId = branchIdToSave
                });

                db.SaveChanges();

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // ============ EDIT ============
                var user = db.Users.FirstOrDefault(x => x.UserId == _userId.Value);
                if (user is null)
                {
                    _lblStatus.Text = "User no longer exists.";
                    return;
                }

                user.FullName = _txtFullName.Text.Trim();
                user.Email = _txtEmail.Text.Trim();
                user.ContactNumber = _txtContact.Text.Trim();
                user.RoleId = roleId;
                user.IsActive = _chkActive.Checked;
                user.BranchId = branchIdToSave;

                db.SaveChanges();

                DialogResult = DialogResult.OK;
                Close();
            }
        }
        catch (Exception ex)
        {
            _lblStatus.Text = ex.Message;
        }
    }

    /// <summary>
    /// Small internal class used only as the combo box's data source.
    /// </summary>
    private sealed class BranchPickItem
    {
        public int BranchId { get; set; }
        public string Display { get; set; } = "";
    }
}