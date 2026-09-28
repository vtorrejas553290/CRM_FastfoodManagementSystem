using System;
using System.Linq;
using System.Windows.Forms;
using CRM.domain.Entities;
using CRM.infrastructure;
using CRM.infrastructure.Data;
using CRM.infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmLogin : Form
{
    private const string SuperAdminCode = "SUPERADMIN";

    private readonly PasswordHasher<User> _userHasher = new PasswordHasher<User>();

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

        lblCompanyCode.ForeColor = AppTheme.TextPrimary;
        lblUser.ForeColor = AppTheme.TextPrimary;
        lblPass.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleInput(txtCompanyCode);
        AppTheme.StyleInput(txtUsername);
        AppTheme.StyleInput(txtPassword);
        AppTheme.StylePrimaryButton(btnLogin);
        AppTheme.StyleSecondaryButton(btnExit);

        lblError.ForeColor = AppTheme.Danger;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        var companyCode = txtCompanyCode.Text.Trim();
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(companyCode))
        {
            lblError.Text = "Company Code is required.";
            return;
        }
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            lblError.Text = "Username and password are required.";
            return;
        }

        try
        {
            bool ok = string.Equals(companyCode, SuperAdminCode, StringComparison.OrdinalIgnoreCase)
                ? TryLoginSuperAdmin(username, password)
                : TryLoginTenantUser(companyCode, username, password);

            if (!ok) return;

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    // ---------------------------------------------------------------
    // SUPER ADMIN — master DB AspNetUsers (Identity)
    // ---------------------------------------------------------------
    private bool TryLoginSuperAdmin(string username, string password)
    {
        using var master = AppServices.CreateMasterContext();

        var aspUser = master.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.UserName == username);

        if (aspUser is null)
        {
            lblError.Text = "Super Admin account not found.";
            return false;
        }

        var identityHasher = new PasswordHasher<object>();
        var verify = identityHasher.VerifyHashedPassword(
            new object(),
            aspUser.PasswordHash ?? string.Empty,
            password);

        if (verify == PasswordVerificationResult.Failed)
        {
            lblError.Text = "Invalid Super Admin password.";
            return false;
        }

        UserSession.UserId = 0;
        UserSession.Username = aspUser.UserName ?? username;
        UserSession.FullName = aspUser.UserName ?? username;
        UserSession.RoleCode = "SUPERADMIN";
        UserSession.RoleName = "Super Admin";

        UserSession.CompanyId = 0;
        UserSession.CompanyCode = SuperAdminCode;
        UserSession.CompanyName = "Master";

        UserSession.BranchId = null;
        UserSession.BranchName = string.Empty;

        UserSession.HasMainTransaction = true;
        UserSession.HasDataCollection = true;
        UserSession.HasBusinessIntelligence = true;
        UserSession.HasActions = true;
        UserSession.HasBranching = true;

        // NEW — SuperAdmin has no tenant context, sees everything
        AppServices.CurrentCompanyId = 0;
        AppServices.CurrentBranchId = null;
        AppServices.CurrentUserIsAdmin = true;

        ActivityLogger.Log("Login", "AspNetUsers", null,
            $"Super Admin '{username}' logged in.");

        return true;
    }

    // ---------------------------------------------------------------
    // TENANT USER — master routing → tenant DB
    // ---------------------------------------------------------------
    private bool TryLoginTenantUser(string companyCode, string username, string password)
    {
        int companyId;
        string companyName;

        using (var master = AppServices.CreateMasterContext())
        {
            var company = master.Companies
                .AsNoTracking()
                .FirstOrDefault(c => c.CompanyCode == companyCode && c.IsActive);

            if (company is null)
            {
                lblError.Text = "Unknown or inactive Company Code.";
                return false;
            }

            companyId = company.CompanyId;
            companyName = company.CompanyName;
        }

        using (var tenant = AppServices.CreateTenantContext(companyId))
        {
            var user = tenant.Users
                .Include(x => x.Role)
                .Include(x => x.Branch)
                .AsNoTracking()
                .FirstOrDefault(x => x.Username == username);

            if (user is null || !user.IsActive)
            {
                lblError.Text = "Invalid username or password.";
                return false;
            }

            if (!PasswordHasher.Verify(password, user.PasswordHash))
            {
                lblError.Text = "Invalid username or password.";
                return false;
            }

            UserSession.UserId = user.UserId;
            UserSession.Username = user.Username;
            UserSession.FullName = user.FullName;
            UserSession.RoleCode = user.Role?.RoleCode ?? string.Empty;
            UserSession.RoleName = user.Role?.RoleName ?? string.Empty;

            UserSession.CompanyId = companyId;
            UserSession.CompanyCode = companyCode;
            UserSession.CompanyName = companyName;

            UserSession.BranchId = user.BranchId;
            UserSession.BranchName = user.Branch?.BranchName ?? string.Empty;
        }

        using (var master = AppServices.CreateMasterContext())
        {
            var sub = master.Subscriptions
                .Include(s => s.Plan)
                .AsNoTracking()
                .FirstOrDefault(s => s.CompanyId == companyId && s.IsActive);

            if (sub?.Plan is null)
            {
                lblError.Text = "No active subscription for this company.";
                return false;
            }

            UserSession.HasMainTransaction = sub.Plan.HasMainTransaction;
            UserSession.HasDataCollection = sub.Plan.HasDataCollection;
            UserSession.HasBusinessIntelligence = sub.Plan.HasBusinessIntelligence;
            UserSession.HasActions = sub.Plan.HasActions;
            UserSession.HasBranching = sub.Plan.HasBranching;
        }

        // NEW — set the ambient tenant + branch context so every form and
        // controller that calls AppServices.CreateTenantContext() routes to
        // this tenant's DB, and controllers can filter by this user's branch.
        AppServices.CurrentCompanyId = companyId;
        AppServices.CurrentBranchId = UserSession.BranchId;
        AppServices.CurrentUserIsAdmin = UserSession.IsAdmin;

        ActivityLogger.Log("Login", "User", UserSession.UserId,
            $"{UserSession.Username} ({UserSession.RoleCode}) logged in from {UserSession.CompanyCode}");

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
                    // NEW — clear all three ambient contexts on abort
                    AppServices.CurrentCompanyId = 0;
                    AppServices.CurrentBranchId = null;
                    AppServices.CurrentUserIsAdmin = false;
                    lblError.Text = "You must accept the Terms and Conditions to continue.";
                    return false;
                }
            }
        }

        return true;
    }

    private void BtnExit_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}