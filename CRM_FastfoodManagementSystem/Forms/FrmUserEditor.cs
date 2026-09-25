using CRM.domain.Entities;
using CRM.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
    private CheckBox _chkActive = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

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
        ClientSize = new Size(500, 430);

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

        _chkActive = new CheckBox
        {
            Text = "Is Active",
            Location = new Point(150, 258),
            Checked = true,
            AutoSize = true
        };
        _chkActive.Font = AppTheme.FontBody;
        _chkActive.ForeColor = AppTheme.TextPrimary;

        _btnSave = new Button { Text = "Save", Location = new Point(150, 300), Width = 140 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(300, 300), Width = 140 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label
        {
            Location = new Point(20, 355),
            AutoSize = true,
            MaximumSize = new Size(460, 40)
        };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[]
        {
            lblUsername, _txtUsername, lblPassword, _txtPassword,
            lblFullName, _txtFullName, lblEmail, _txtEmail,
            lblContact, _txtContact, lblRole, _cmbRole,
            _chkActive, _btnSave, _btnCancel, _lblStatus
        });
    }

    private void FrmUserEditor_Load(object? sender, EventArgs e)
    {
        LoadRoles();

        if (_userId is null) return;

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

            // On edit, password field shows a note instead of allowing edit
            _txtPassword.Enabled = false;
            _txtPassword.Text = "(unchanged)";
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
                    CreatedAt = DateTime.UtcNow
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
}