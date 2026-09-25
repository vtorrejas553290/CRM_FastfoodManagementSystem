using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmComplaintEditor : Form
{
    private static readonly string[] Categories =
    {
        "Order Accuracy", "Food Quality", "Service", "Cleanliness"
    };

    private static readonly string[] Severities =
    {
        "Low", "Medium", "High", "Critical"
    };

    private readonly int? _complaintId;

    private class CustomerOption
    {
        public int CustomerId { get; set; }
        public string Display { get; set; } = "";
        public override string ToString() => Display;
    }

    private class OrderOption
    {
        public int? OrderId { get; set; }
        public string Display { get; set; } = "";
        public override string ToString() => Display;
    }

    public FrmComplaintEditor(int? complaintId)
    {
        _complaintId = complaintId;
        InitializeComponent();
        ApplyTheme();

        Load += FrmComplaintEditor_Load;
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;
        cmbCustomer.SelectedIndexChanged += (_, __) => ReloadOrdersForCustomer();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblTitle.ForeColor = AppTheme.TextPrimary;
        lblSubtitle.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[]
        {
            lblCustomer, lblOrder, lblCategory, lblSeverity, lblSubject, lblDescription
        })
            lbl.ForeColor = AppTheme.TextSecondary;

        AppTheme.StyleInput(cmbCustomer);
        AppTheme.StyleInput(cmbOrder);
        AppTheme.StyleInput(cmbCategory);
        AppTheme.StyleInput(cmbSeverity);
        AppTheme.StyleInput(txtSubject);
        AppTheme.StyleInput(txtDescription);

        AppTheme.StyleSuccessButton(btnSave);
        AppTheme.StyleNeutralButton(btnCancel);

        // Populate dropdowns
        cmbCategory.Items.Clear();
        cmbCategory.Items.AddRange(Categories);
        cmbCategory.SelectedIndex = 0;

        cmbSeverity.Items.Clear();
        cmbSeverity.Items.AddRange(Severities);
        cmbSeverity.SelectedIndex = 1; // Medium
    }

    private void FrmComplaintEditor_Load(object? sender, EventArgs e)
    {
        // Load customers
        using var db = AppServices.CreateTenantContext();

        var customers = db.Customers
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.CustomerName)
            .Take(500)
            .ToList()
            .Select(c => new CustomerOption
            {
                CustomerId = c.CustomerId,
                Display = $"{c.CustomerCode} — {c.CustomerName}"
            })
            .ToList();

        cmbCustomer.Items.Clear();
        foreach (var c in customers)
            cmbCustomer.Items.Add(c);

        // If editing, load existing complaint
        if (_complaintId is int id)
        {
            var complaint = db.Complaints.AsNoTracking().FirstOrDefault(x => x.ComplaintId == id);
            if (complaint is null)
            {
                MessageBox.Show("Complaint not found.", "Error");
                Close();
                return;
            }

            Text = $"Edit Complaint — {complaint.ComplaintCode}";
            lblTitle.Text = "Edit Complaint";
            lblSubtitle.Text = complaint.ComplaintCode;

            // Select customer
            var target = customers.FirstOrDefault(x => x.CustomerId == complaint.CustomerId);
            if (target is not null)
                cmbCustomer.SelectedItem = target;

            // Reload orders and select the saved one
            ReloadOrdersForCustomer();

            if (complaint.OrderId is int oid)
            {
                foreach (OrderOption opt in cmbOrder.Items)
                {
                    if (opt.OrderId == oid) { cmbOrder.SelectedItem = opt; break; }
                }
            }

            cmbCategory.SelectedItem = complaint.Category;
            cmbSeverity.SelectedItem = complaint.Severity;
            txtSubject.Text = complaint.Subject;
            txtDescription.Text = complaint.Description;
        }
        else
        {
            Text = "File Complaint";
            if (cmbCustomer.Items.Count > 0) cmbCustomer.SelectedIndex = 0;
        }
    }

    private void ReloadOrdersForCustomer()
    {
        cmbOrder.Items.Clear();
        cmbOrder.Items.Add(new OrderOption { OrderId = null, Display = "(No specific order)" });

        if (cmbCustomer.SelectedItem is not CustomerOption cust) return;

        using var db = AppServices.CreateTenantContext();
        var cutoff = DateTime.UtcNow.AddDays(-30);

        var orders = db.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == cust.CustomerId && o.OrderDate >= cutoff)
            .OrderByDescending(o => o.OrderDate)
            .Take(50)
            .ToList()
            .Select(o => new OrderOption
            {
                OrderId = o.OrderId,
                Display = $"{o.OrderCode} — {o.OrderDate:yyyy-MM-dd} — ₱{o.TotalAmount:N2} ({o.Status})"
            })
            .ToList();

        foreach (var o in orders)
            cmbOrder.Items.Add(o);

        cmbOrder.SelectedIndex = 0;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        // Validation
        if (cmbCustomer.SelectedItem is not CustomerOption customer)
        {
            MessageBox.Show("Please select a customer.", "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtSubject.Text))
        {
            MessageBox.Show("Please enter a subject.", "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text.Length < 10)
        {
            MessageBox.Show("Description must be at least 10 characters.", "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedOrderId = (cmbOrder.SelectedItem as OrderOption)?.OrderId;

        try
        {
            using var db = AppServices.CreateTenantContext();

            if (_complaintId is int id)
            {
                var complaint = db.Complaints.FirstOrDefault(x => x.ComplaintId == id);
                if (complaint is null) return;

                complaint.CustomerId = customer.CustomerId;
                complaint.OrderId = selectedOrderId;
                complaint.Category = cmbCategory.SelectedItem?.ToString() ?? "Other";
                complaint.Severity = cmbSeverity.SelectedItem?.ToString() ?? "Medium";
                complaint.Subject = txtSubject.Text.Trim();
                complaint.Description = txtDescription.Text.Trim();
                complaint.UpdatedAt = DateTime.UtcNow;

                db.SaveChanges();

                ActivityLogger.Log("Update", "Complaint", id,
                    $"Complaint '{complaint.ComplaintCode}' updated");
            }
            else
            {
                // Generate ComplaintCode
                var last = db.Complaints
                    .OrderByDescending(x => x.ComplaintId)
                    .Select(x => x.ComplaintId)
                    .FirstOrDefault();
                var code = $"CMP-{(last + 1):D5}";

                var complaint = new Complaint
                {
                    ComplaintCode = code,
                    CustomerId = customer.CustomerId,
                    OrderId = selectedOrderId,
                    Category = cmbCategory.SelectedItem?.ToString() ?? "Other",
                    Severity = cmbSeverity.SelectedItem?.ToString() ?? "Medium",
                    Subject = txtSubject.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Status = "Open",
                    SubmittedAt = DateTime.UtcNow,
                    CreatedByUserId = UserSession.UserId,
                    IsArchived = false
                };

                db.Complaints.Add(complaint);
                db.SaveChanges();

                ActivityLogger.Log("Create", "Complaint", complaint.ComplaintId,
                    $"Complaint '{complaint.ComplaintCode}' filed");
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}