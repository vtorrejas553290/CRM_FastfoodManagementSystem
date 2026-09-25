using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace CRM.winForms.Forms;

public partial class FrmTermsEditor : Form
{
    public FrmTermsEditor()
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

        // Panels
        pnlHeader.BackColor = AppTheme.Surface;
        pnlTermsWrap.BackColor = AppTheme.ContentSurface;
        pnlTermsToolbar.BackColor = AppTheme.Surface;
        pnlNew.BackColor = AppTheme.Surface;
        pnlFormFooter.BackColor = AppTheme.Surface;

        // Header
        lblHeader.Font = AppTheme.FontHeading;
        lblHeader.ForeColor = AppTheme.TextPrimary;

        // Section title
        lblNewTitle.Font = AppTheme.FontSubheading;
        lblNewTitle.ForeColor = AppTheme.TextPrimary;

        // Grid + buttons
        AppTheme.StyleGrid(gridTerms);
        AppTheme.StylePrimaryButton(btnView);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleDangerButton(btnArchive);
        AppTheme.StyleSuccessButton(btnSave);

        // Form labels + inputs
        foreach (var lbl in new[] { lblVersion, lblTitle, lblContent })
            AppTheme.StyleLabel(lbl);

        foreach (var txt in new Control[] { txtVersion, txtTitle, txtContent })
            AppTheme.StyleInput(txt);

        // Checkbox
        chkActive.Font = AppTheme.FontBody;
        chkActive.ForeColor = AppTheme.TextPrimary;
        chkActive.BackColor = System.Drawing.Color.Transparent;

        // Status
        lblStatus.Font = AppTheme.FontSmall;
        lblStatus.ForeColor = AppTheme.TextSecondary;

        // Dynamic title
        lblHeader.Text = $"Existing Terms & Conditions ({UserSession.RoleCode})";
    }

    private void LoadTerms()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var myRole = UserSession.RoleCode;

            var terms = db.TermsAndConditions
                .AsNoTracking()
                .Where(x => x.CreatedByRoleCode == myRole)
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

            if (terms.Count == 0)
            {
                lblStatus.ForeColor = AppTheme.TextSecondary;
                lblStatus.Text = $"No {myRole} terms yet.";
            }
            else
            {
                lblStatus.Text = string.Empty;
            }
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

    private void BtnArchive_Click(object? sender, EventArgs e)
    {
        var id = GetSelectedTermsId();
        if (id is null)
        {
            MessageBox.Show("Select a Terms and Conditions row first.", "No selection");
            return;
        }

        var confirm = MessageBox.Show(
            "Archive this version? It will no longer be shown to Staff.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();

            var terms = db.TermsAndConditions
                .FirstOrDefault(x => x.TermsAndConditionId == id.Value);

            if (terms is null)
            {
                MessageBox.Show("Terms not found.", "Error");
                return;
            }

            if (terms.CreatedByRoleCode != UserSession.RoleCode)
            {
                MessageBox.Show(
                    $"You can only archive {UserSession.RoleCode} terms.",
                    "Not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            terms.IsActive = false;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "TermsAndCondition", id.Value,
                $"T&C '{terms.Version}' archived");

            MessageBox.Show("Terms archived.", "Success");
            LoadTerms();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Archive failed: {ex.Message}", "Error");
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblStatus.ForeColor = AppTheme.Error;
        lblStatus.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(txtVersion.Text))
        {
            lblStatus.Text = "Version is required (e.g., v1.0)";
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

        try
        {
            using var db = AppServices.CreateTenantContext();

            var myRole = UserSession.RoleCode;
            var version = txtVersion.Text.Trim();

            if (db.TermsAndConditions.Any(x =>
                x.CreatedByRoleCode == myRole && x.Version == version))
            {
                lblStatus.Text = $"You already have a '{version}' version.";
                return;
            }

            db.TermsAndConditions.Add(new TermsAndCondition
            {
                Version = version,
                Title = txtTitle.Text.Trim(),
                Content = txtContent.Text.Trim(),
                IsActive = chkActive.Checked,
                EffectiveFrom = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = UserSession.UserId,
                CreatedByRoleCode = myRole
            });

            db.SaveChanges();

            ActivityLogger.Log("Create", "TermsAndCondition", null,
                $"T&C '{version}' created ({myRole})");

            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "New Terms version saved.";

            txtVersion.Clear();
            txtTitle.Clear();
            txtContent.Clear();

            LoadTerms();
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }
}