using CRM.domain.Entities;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmSuperAdminTerms : Form
{
    // All three tenant CompanyIds, per the seeded master DB.
    private static readonly int[] TenantCompanyIds = { 3, 4, 5 };

    // Tenant admins are UserId=1002 in every tenant DB. Used to satisfy the
    // FK on TermsAndCondition.CreatedByUserId without changing the schema.
    // The real authorship is recorded in CreatedByRoleCode = "SUPERADMIN".
    private const int TenantAdminFkUserId = 1002;

    private const string SuperAdminRoleCode = "SUPERADMIN";

    public FrmSuperAdminTerms()
    {
        InitializeComponent();
        ApplyTheme();

        Load += (_, __) => LoadTerms();
        btnRefresh.Click += (_, __) => LoadTerms();
        btnView.Click += BtnView_Click;
        btnArchive.Click += BtnArchive_Click;
        btnSave.Click += BtnSave_Click;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);

        pnlHeader.BackColor = AppTheme.Surface;
        pnlTermsWrap.BackColor = AppTheme.ContentSurface;
        pnlTermsToolbar.BackColor = AppTheme.Surface;
        pnlNew.BackColor = AppTheme.Surface;
        pnlFormFooter.BackColor = AppTheme.Surface;

        lblHeader.Font = AppTheme.FontHeading;
        lblHeader.ForeColor = AppTheme.TextPrimary;

        lblNewTitle.Font = AppTheme.FontSubheading;
        lblNewTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleGrid(gridTerms);
        AppTheme.StylePrimaryButton(btnView);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleDangerButton(btnArchive);
        AppTheme.StyleSuccessButton(btnSave);

        foreach (var lbl in new[] { lblVersion, lblTitle, lblContent })
            AppTheme.StyleLabel(lbl);

        foreach (var txt in new Control[] { txtVersion, txtTitle, txtContent })
            AppTheme.StyleInput(txt);

        chkActive.Font = AppTheme.FontBody;
        chkActive.ForeColor = AppTheme.TextPrimary;
        chkActive.BackColor = System.Drawing.Color.Transparent;

        lblStatus.Font = AppTheme.FontSmall;
        lblStatus.ForeColor = AppTheme.TextSecondary;
    }

    // ============================================================
    //  LOAD (reads from Tenant A as the representative copy)
    // ============================================================

    private void LoadTerms()
    {
        try
        {
            // Read from Tenant A. After every successful push all three tenants
            // have the same SuperAdmin rows, so one read is enough.
            using var db = AppServices.CreateTenantContext(TenantCompanyIds[0]);

            var terms = db.TermsAndConditions
                .AsNoTracking()
                .Where(x => x.CreatedByRoleCode == SuperAdminRoleCode)
                .OrderByDescending(x => x.EffectiveFrom)
                .Select(x => new
                {
                    x.TermsAndConditionId,
                    x.Version,
                    x.Title,
                    ContentPreview = x.Content.Length > 80
                        ? x.Content.Substring(0, 80) + "..."
                        : x.Content,
                    Status = x.IsActive ? "Active" : "Archived",
                    x.EffectiveFrom
                })
                .ToList();

            gridTerms.DataSource = terms;

            lblStatus.ForeColor = AppTheme.TextSecondary;
            lblStatus.Text = terms.Count == 0
                ? "No SuperAdmin terms yet."
                : $"{terms.Count} version(s) shown. Editing pushes to all 3 tenants.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private int? GetSelectedTermsId()
    {
        if (gridTerms.CurrentRow?.DataBoundItem is null) return null;
        dynamic row = gridTerms.CurrentRow.DataBoundItem;
        return (int)row.TermsAndConditionId;
    }

    // ============================================================
    //  VIEW — shows the row from Tenant A
    // ============================================================

    private void BtnView_Click(object? sender, EventArgs e)
    {
        var id = GetSelectedTermsId();
        if (id is null)
        {
            MessageBox.Show("Select a Terms and Conditions row first.", "No selection");
            return;
        }

        using var viewer = new FrmTermsViewer(id.Value);
        viewer.ShowDialog();
    }

    // ============================================================
    //  ARCHIVE — flips IsActive=false on that version in every tenant
    // ============================================================

    private void BtnArchive_Click(object? sender, EventArgs e)
    {
        var id = GetSelectedTermsId();
        if (id is null)
        {
            MessageBox.Show("Select a Terms and Conditions row first.", "No selection");
            return;
        }

        var confirm = MessageBox.Show(
            "Archive this version across ALL 3 tenants? Admins will no longer see it as Active.",
            "Confirm Archive (all tenants)",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        // We captured the version string from the grid row so we can find the
        // matching row in every tenant even if their TermsAndConditionId differs.
        if (gridTerms.CurrentRow?.DataBoundItem is null) return;
        dynamic selected = gridTerms.CurrentRow.DataBoundItem;
        string versionToArchive = (string)selected.Version;

        var failures = new List<string>();
        int archived = 0;

        foreach (var companyId in TenantCompanyIds)
        {
            try
            {
                using var db = AppServices.CreateTenantContext(companyId);

                var row = db.TermsAndConditions.FirstOrDefault(x =>
                    x.CreatedByRoleCode == SuperAdminRoleCode &&
                    x.Version == versionToArchive);

                if (row is null) continue;

                row.IsActive = false;
                db.SaveChanges();
                archived++;
            }
            catch (Exception ex)
            {
                failures.Add($"Tenant {companyId}: {ex.Message}");
            }
        }

        if (failures.Count > 0)
        {
            MessageBox.Show(
                $"Archived in {archived}/{TenantCompanyIds.Length} tenant(s).\n\n" +
                string.Join("\n", failures),
                "Partial archive",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        else
        {
            MessageBox.Show($"Archived in all {archived} tenant(s).", "Success");
        }

        LoadTerms();
    }

    // ============================================================
    //  SAVE — inserts the new version into every tenant DB
    // ============================================================

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblStatus.ForeColor = AppTheme.Error;
        lblStatus.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(txtVersion.Text))
        {
            lblStatus.Text = "Version is required (e.g., v1.1)";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            lblStatus.Text = "Title is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtContent.Text) || txtContent.Text.Length < 20)
        {
            lblStatus.Text = "Content must be at least 20 characters.";
            return;
        }

        var version = txtVersion.Text.Trim();
        var title = txtTitle.Text.Trim();
        var content = txtContent.Text.Trim();
        var isActive = chkActive.Checked;

        var failures = new List<string>();
        int inserted = 0;

        foreach (var companyId in TenantCompanyIds)
        {
            try
            {
                using var db = AppServices.CreateTenantContext(companyId);

                // Skip if this version already exists in this tenant
                if (db.TermsAndConditions.Any(x =>
                    x.CreatedByRoleCode == SuperAdminRoleCode &&
                    x.Version == version))
                {
                    continue;
                }

                db.TermsAndConditions.Add(new TermsAndCondition
                {
                    Version = version,
                    Title = title,
                    Content = content,
                    IsActive = isActive,
                    EffectiveFrom = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    // FK-safe: tenant admin user id, same in every tenant
                    CreatedByUserId = TenantAdminFkUserId,
                    // Truthful authorship
                    CreatedByRoleCode = SuperAdminRoleCode
                });

                db.SaveChanges();
                inserted++;
            }
            catch (Exception ex)
            {
                failures.Add($"Tenant {companyId}: {ex.Message}");
            }
        }

        if (failures.Count > 0)
        {
            lblStatus.ForeColor = AppTheme.WarningAmber;
            lblStatus.Text = $"Pushed to {inserted}/{TenantCompanyIds.Length} tenants. {failures.Count} failure(s).";
            MessageBox.Show(
                $"Pushed to {inserted} of {TenantCompanyIds.Length} tenants.\n\n" +
                string.Join("\n", failures),
                "Partial push",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        else if (inserted == 0)
        {
            lblStatus.ForeColor = AppTheme.Error;
            lblStatus.Text = $"Version '{version}' already exists in all tenants.";
            return;
        }
        else
        {
            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = $"Version '{version}' pushed to all {inserted} tenants.";
        }

        txtVersion.Clear();
        txtTitle.Clear();
        txtContent.Clear();

        LoadTerms();
    }
}