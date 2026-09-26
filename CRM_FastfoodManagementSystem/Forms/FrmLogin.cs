using CRM.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmLogin : Form
{
    public FrmLogin()
    {
        InitializeComponent();
        ApplyTheme();
        btnLogin.Click += BtnLogin_Click;
        btnExit.Click += BtnExit_Click;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        lblUser.ForeColor = AppTheme.TextPrimary;
        lblPass.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleInput(txtUsername);
        AppTheme.StyleInput(txtPassword);
        AppTheme.StylePrimaryButton(btnLogin);
        AppTheme.StyleSecondaryButton(btnExit);

        lblError.ForeColor = AppTheme.Danger;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            lblError.Text = "Username and password are required.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            var user = db.Users.Include(x => x.Role).AsNoTracking()
                .FirstOrDefault(x => x.Username == username);

            if (user is null || !user.IsActive)
            {
                lblError.Text = "Invalid username or password.";
                return;
            }

            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                lblError.Text = "Invalid username or password.";
                return;
            }

            UserSession.UserId = user.UserId;
            UserSession.Username = user.Username;
            UserSession.FullName = user.FullName;
            UserSession.RoleCode = user.Role?.RoleCode ?? string.Empty;
            UserSession.RoleName = user.Role?.RoleName ?? string.Empty;

            // Log the login attempt
            ActivityLogger.Log("Login", "User", user.UserId,
                $"{user.Username} logged in ({UserSession.RoleCode})");

            if (UserSession.IsAdmin)
            {
                var pending = AppServices.GetUnacceptedSuperAdminTerms(UserSession.UserId);
                if (pending is not null)
                {
                    using var gate = new FrmAdminTermsGate(pending.TermsAndConditionId);
                    gate.ShowDialog(this);
                    if (!gate.WasAccepted)
                    {
                        UserSession.Clear();
                        lblError.Text = "You must accept the Terms and Conditions to continue.";
                        return;
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    private void BtnExit_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}