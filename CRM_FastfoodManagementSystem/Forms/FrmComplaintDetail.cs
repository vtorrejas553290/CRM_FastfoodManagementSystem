using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmComplaintDetail : Form
{
    private static readonly string[] Statuses =
    {
        "Open", "In Progress", "Resolved", "Closed", "Rejected"
    };

    private readonly int _complaintId;
    private int? _assignedUserId;

    public FrmComplaintDetail(int complaintId)
    {
        _complaintId = complaintId;
        InitializeComponent();
        ApplyTheme();

        Load += FrmComplaintDetail_Load;
        btnEdit.Click += (_, __) => EditComplaint();
        btnSave.Click += (_, __) => SaveChanges();
        btnClose.Click += (_, __) => Close();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblCode.ForeColor = AppTheme.TextPrimary;
        lblMeta.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[]
        {
            lblCustomerLabel, lblOrderLabel, lblCategoryLabel, lblSeverityLabel,
            lblSubjectLabel, lblDescriptionLabel, lblStatusLabel, lblAssignedLabel,
            lblResolutionLabel
        })
            lbl.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[]
        {
            lblCustomer, lblOrder, lblCategory, lblSeverity, lblSubject
        })
            lbl.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleInput(txtDescription);
        AppTheme.StyleInput(cmbStatus);
        AppTheme.StyleInput(cmbAssignedTo);
        AppTheme.StyleInput(txtResolution);

        AppTheme.StyleSecondaryButton(btnEdit);
        AppTheme.StyleSuccessButton(btnSave);
        AppTheme.StyleNeutralButton(btnClose);

        // Status dropdown
        cmbStatus.Items.Clear();
        cmbStatus.Items.AddRange(Statuses);

        // Only Manager/Admin can edit status + resolution
        bool canResolve = UserSession.IsAdmin || UserSession.IsManager;
        cmbStatus.Enabled = canResolve;
        cmbAssignedTo.Enabled = canResolve;
        txtResolution.ReadOnly = !canResolve;
        btnSave.Visible = canResolve;
    }

    private void FrmComplaintDetail_Load(object? sender, EventArgs e)
    {
        using var db = AppServices.CreateTenantContext();

        var complaint = db.Complaints
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .Include(x => x.AssignedToUser)
            .AsNoTracking()
            .FirstOrDefault(x => x.ComplaintId == _complaintId);

        if (complaint is null)
        {
            MessageBox.Show("Complaint not found.", "Error");
            Close();
            return;
        }

        Text = $"Complaint — {complaint.ComplaintCode}";
        lblCode.Text = complaint.ComplaintCode;
        lblMeta.Text = $"Filed {complaint.SubmittedAt:yyyy-MM-dd HH:mm}"
            + (complaint.UpdatedAt is DateTime u ? $"  ·  Updated {u:yyyy-MM-dd HH:mm}" : "");

        lblCustomer.Text = complaint.Customer?.CustomerName ?? "(deleted)";
        lblOrder.Text = complaint.Order is not null
            ? $"{complaint.Order.OrderCode} — {complaint.Order.OrderDate:yyyy-MM-dd} — ₱{complaint.Order.TotalAmount:N2}"
            : "(none)";
        lblCategory.Text = complaint.Category;
        lblSeverity.Text = complaint.Severity;
        lblSubject.Text = complaint.Subject;
        txtDescription.Text = complaint.Description;
        txtResolution.Text = complaint.ResolutionNotes ?? "";

        // Color severity
        lblSeverity.ForeColor = complaint.Severity switch
        {
            "Critical" => Color.FromArgb(190, 40, 40),
            "High" => Color.FromArgb(220, 100, 40),
            "Medium" => Color.FromArgb(200, 130, 0),
            "Low" => Color.FromArgb(22, 130, 60),
            _ => AppTheme.TextPrimary
        };

        // Status
        var idx = Array.IndexOf(Statuses, complaint.Status);
        if (idx >= 0) cmbStatus.SelectedIndex = idx;
        _assignedUserId = complaint.AssignedToUserId;

        // Load assignable users (Admin + Manager only)
        var assignables = db.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .Where(x => x.IsActive && x.Role != null
                     && (x.Role.RoleCode == "ADMIN" || x.Role.RoleCode == "MANAGER"))
            .OrderBy(x => x.FullName)
            .ToList();

        cmbAssignedTo.Items.Clear();
        cmbAssignedTo.Items.Add(new AssignableUser { UserId = null, Display = "(unassigned)" });

        foreach (var usr in assignables)
        {
            cmbAssignedTo.Items.Add(new AssignableUser
            {
                UserId = usr.UserId,
                Display = $"{usr.FullName} ({usr.Role!.RoleCode})"
            });
        }

        // Select current
        for (int i = 0; i < cmbAssignedTo.Items.Count; i++)
        {
            if ((cmbAssignedTo.Items[i] as AssignableUser)?.UserId == _assignedUserId)
            {
                cmbAssignedTo.SelectedIndex = i;
                break;
            }
        }
        if (cmbAssignedTo.SelectedIndex < 0) cmbAssignedTo.SelectedIndex = 0;
    }

    private class AssignableUser
    {
        public int? UserId { get; set; }
        public string Display { get; set; } = "";
        public override string ToString() => Display;
    }

    private void EditComplaint()
    {
        using var dlg = new FrmComplaintEditor(_complaintId);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            // Reload
            FrmComplaintDetail_Load(null, EventArgs.Empty);
        }
    }

    private void SaveChanges()
    {
        if (!UserSession.IsAdmin && !UserSession.IsManager)
        {
            MessageBox.Show("Only Admin and Manager can update complaint status.",
                "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var newStatus = cmbStatus.SelectedItem?.ToString() ?? "Open";
        var assignedId = (cmbAssignedTo.SelectedItem as AssignableUser)?.UserId;
        var resolution = txtResolution.Text.Trim();

        // Validate: resolving / closing / rejecting requires notes
        if ((newStatus == "Resolved" || newStatus == "Closed" || newStatus == "Rejected")
            && string.IsNullOrWhiteSpace(resolution))
        {
            MessageBox.Show($"Please provide resolution notes before marking as {newStatus}.",
                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();
            var complaint = db.Complaints.FirstOrDefault(x => x.ComplaintId == _complaintId);
            if (complaint is null) return;

            complaint.Status = newStatus;
            complaint.AssignedToUserId = assignedId;
            complaint.ResolutionNotes = string.IsNullOrWhiteSpace(resolution) ? null : resolution;
            complaint.UpdatedAt = DateTime.UtcNow;

            if (newStatus == "Resolved" || newStatus == "Closed")
                complaint.ResolvedAt = DateTime.UtcNow;

            db.SaveChanges();

            ActivityLogger.Log("Update", "Complaint", _complaintId,
                $"Complaint '{complaint.ComplaintCode}' → {newStatus}");

            MessageBox.Show("Complaint updated.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}