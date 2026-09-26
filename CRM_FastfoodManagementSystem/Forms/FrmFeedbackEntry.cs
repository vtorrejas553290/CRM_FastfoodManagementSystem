using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmFeedbackEntry : Form
{
    private readonly int? _feedbackId;

    public FrmFeedbackEntry() : this(null) { }

    public FrmFeedbackEntry(int? feedbackId)
    {
        _feedbackId = feedbackId;
        InitializeComponent();

        ApplyTheme();

        Load += FrmFeedbackEntry_Load;
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        Text = _feedbackId is null ? "Add Feedback" : "Edit Feedback";

        foreach (var lbl in new[] { lblCustomer, lblRating, lblCategory, lblStatusField, lblComments })
            AppTheme.StyleLabel(lbl);

        AppTheme.StyleInput(cmbCustomer);
        AppTheme.StyleInput(cmbCategory);
        AppTheme.StyleInput(cmbStatus);
        AppTheme.StyleInput(numRating);
        AppTheme.StyleInput(txtComments);

        AppTheme.StyleSuccessButton(btnSave);
        AppTheme.StyleNeutralButton(btnCancel);

        lblStatus.Font = AppTheme.FontSmall;

        // Staff can only ADD (no edit mode). Lock the status field.
        cmbStatus.Enabled = false;
    }

    private void FrmFeedbackEntry_Load(object? sender, EventArgs e)
    {
        // Populate customers
        using (var db = AppServices.CreateTenantContext())
        {
            var customers = db.Customers
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.CustomerName)
                .Select(x => new { x.CustomerId, x.CustomerName })
                .ToList();

            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "CustomerName";
            cmbCustomer.ValueMember = "CustomerId";
        }

        // Populate category
        cmbCategory.Items.Clear();
        cmbCategory.Items.AddRange(new object[] { "Food", "Service", "Cleanliness", "Speed" });
        cmbCategory.SelectedIndex = 0;

        // Populate status (locked — Staff can't change)
        cmbStatus.Items.Clear();
        cmbStatus.Items.AddRange(new object[] { "New", "Reviewed", "Resolved", "Archived" });
        cmbStatus.SelectedIndex = 0;

        // EDIT MODE — prefill (kept for backward compatibility, but hidden in normal flow)
        if (_feedbackId is null) return;

        using (var db = AppServices.CreateTenantContext())
        {
            var fb = db.CustomerFeedbacks
                .AsNoTracking()
                .FirstOrDefault(x => x.CustomerFeedbackId == _feedbackId.Value);

            if (fb is null) return;

            cmbCustomer.SelectedValue = fb.CustomerId;
            numRating.Value = fb.Rating;
            cmbCategory.SelectedItem = fb.Category ?? "Food";
            cmbStatus.SelectedItem = fb.Status ?? "New";
            txtComments.Text = fb.Comments ?? string.Empty;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblStatus.ForeColor = AppTheme.Error;
        lblStatus.Text = string.Empty;

        // ============ VALIDATION ============
        if (cmbCustomer.SelectedValue is null)
        {
            lblStatus.Text = "Select a customer.";
            return;
        }

        if (numRating.Value < 1 || numRating.Value > 5)
        {
            lblStatus.Text = "Rating must be between 1 and 5.";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtComments.Text) || txtComments.Text.Trim().Length < 3)
        {
            lblStatus.Text = "Comments must be at least 3 characters.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            if (_feedbackId is null)
            {
                // === ADD ===
                db.CustomerFeedbacks.Add(new CustomerFeedback
                {
                    CustomerId = (int)cmbCustomer.SelectedValue,
                    Rating = (int)numRating.Value,
                    Comments = txtComments.Text.Trim(),
                    Category = cmbCategory.SelectedItem?.ToString(),
                    Status = "New",
                    SubmittedAt = DateTime.UtcNow
                });

                db.SaveChanges();

                lblStatus.ForeColor = AppTheme.Success;
                lblStatus.Text = "Feedback recorded.";

                txtComments.Clear();
                numRating.Value = 3;
                cmbStatus.SelectedIndex = 0;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // === EDIT (kept in code — but not reachable from UI now) ===
                var fb = db.CustomerFeedbacks
                    .FirstOrDefault(x => x.CustomerFeedbackId == _feedbackId.Value);

                if (fb is null)
                {
                    lblStatus.Text = "Feedback no longer exists.";
                    return;
                }

                fb.CustomerId = (int)cmbCustomer.SelectedValue;
                fb.Rating = (int)numRating.Value;
                fb.Category = cmbCategory.SelectedItem?.ToString();
                fb.Comments = txtComments.Text.Trim();

                db.SaveChanges();

                lblStatus.ForeColor = AppTheme.Success;
                lblStatus.Text = "Feedback updated.";

                DialogResult = DialogResult.OK;
                Close();
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }
}