using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CRM.winForms.Forms;

public partial class FrmRegisterCustomer : Form
{
    private readonly int? _customerId;
    private int? _activeTermsId;

    public FrmRegisterCustomer() : this(null) { }

    public FrmRegisterCustomer(int? customerId)
    {
        _customerId = customerId;
        InitializeComponent();

        ApplyTheme();

        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
        btnViewTerms.Click += BtnViewTerms_Click;

        Load += FrmRegisterCustomer_Load;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        Text = _customerId is null ? "Register Customer" : "Edit Customer";

        foreach (var lbl in new[] { lblCode, lblFirstName, lblMiddleName, lblLastName, lblContact, lblEmail, lblAddress, lblBirthday })
            AppTheme.StyleLabel(lbl);

        foreach (var txt in new Control[] { txtCode, txtFirstName, txtMiddleName, txtLastName, txtContact, txtEmail, txtAddress })
            AppTheme.StyleInput(txt);

        dtpBirthday.Font = AppTheme.FontBody;

        AppTheme.StyleSuccessButton(btnSave);
        AppTheme.StylePrimaryButton(btnViewTerms);
        AppTheme.StyleNeutralButton(btnCancel);

        lblStatus.Font = AppTheme.FontSmall;
    }

    private void FrmRegisterCustomer_Load(object? sender, EventArgs e)
    {
        LoadActiveTerms();

        if (_customerId is null) return;

        // EDIT MODE — prefill
        try
        {
            using var db = AppServices.CreateTenantContext();
            var c = db.Customers.AsNoTracking().FirstOrDefault(x => x.CustomerId == _customerId.Value);
            if (c is null) return;

            txtCode.Text = c.CustomerCode;
            txtCode.Enabled = false;
            txtFirstName.Text = c.FirstName;
            txtMiddleName.Text = c.MiddleName ?? "";
            txtLastName.Text = c.LastName;
            txtContact.Text = c.ContactNumber;
            txtEmail.Text = c.EmailAddress;
            txtAddress.Text = c.Address;
            dtpBirthday.Value = c.Birthday ?? DateTime.Today;

            chkAcceptTerms.Enabled = false;
            chkAcceptTerms.Text = "Terms already recorded (cannot be changed here)";
            btnViewTerms.Enabled = true;
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }

    private void LoadActiveTerms()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var terms = db.TermsAndConditions
                .AsNoTracking()
                .Where(x => x.IsActive && x.CreatedByRoleCode == "ADMIN")
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefault();

            if (terms is null)
            {
                terms = db.TermsAndConditions
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.EffectiveFrom)
                    .FirstOrDefault();
            }

            if (terms is null)
            {
                _activeTermsId = null;
                chkAcceptTerms.Enabled = false;
                btnViewTerms.Enabled = false;
                chkAcceptTerms.Text = "No active Terms and Conditions available";
                return;
            }

            _activeTermsId = terms.TermsAndConditionId;
            chkAcceptTerms.Text = $"Customer accepted {terms.Title} ({terms.Version})";
            chkAcceptTerms.Enabled = true;
            btnViewTerms.Enabled = true;
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }

    private void BtnViewTerms_Click(object? sender, EventArgs e)
    {
        if (_activeTermsId is null)
        {
            MessageBox.Show("No active Terms and Conditions available.", "Notice");
            return;
        }

        using var viewer = new FrmTermsViewer(_activeTermsId.Value);
        viewer.ShowDialog(this);
    }

    /// <summary>
    /// Builds the display name: "First Middle Last" (middle omitted if empty).
    /// </summary>
    private static string ComposeFullName(string firstName, string? middleName, string lastName)
    {
        var parts = new List<string> { firstName.Trim() };

        if (!string.IsNullOrWhiteSpace(middleName))
            parts.Add(middleName.Trim());

        parts.Add(lastName.Trim());

        return string.Join(" ", parts);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblStatus.ForeColor = AppTheme.Error;
        lblStatus.Text = string.Empty;

        // ============ VALIDATION ============

        if (string.IsNullOrWhiteSpace(txtCode.Text) || txtCode.Text.Trim().Length < 3)
        {
            lblStatus.Text = "Customer Code must be at least 3 characters.";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtFirstName.Text) || txtFirstName.Text.Trim().Length < 2)
        {
            lblStatus.Text = "First Name must be at least 2 characters.";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtLastName.Text) || txtLastName.Text.Trim().Length < 2)
        {
            lblStatus.Text = "Last Name must be at least 2 characters.";
            return;
        }

        if (!string.IsNullOrWhiteSpace(txtContact.Text))
        {
            if (!Regex.IsMatch(txtContact.Text.Trim(), @"^[0-9\+\-\s()]{7,20}$"))
            {
                lblStatus.Text = "Contact Number must be 7–20 digits.";
                return;
            }
        }

        if (!string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblStatus.Text = "Email Address is not in a valid format.";
                return;
            }
        }

        if (dtpBirthday.Value.Date > DateTime.Today)
        {
            lblStatus.Text = "Birthday cannot be in the future.";
            return;
        }

        // Compose full display name
        string firstName = txtFirstName.Text.Trim();
        string? middleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim();
        string lastName = txtLastName.Text.Trim();
        string fullName = ComposeFullName(firstName, middleName, lastName);

        // ============ SAVE ============
        try
        {
            using var db = AppServices.CreateTenantContext();

            if (_customerId is null)
            {
                // CREATE
                if (db.Customers.Any(x => x.CustomerCode == txtCode.Text.Trim()))
                {
                    lblStatus.Text = "Customer Code already exists.";
                    return;
                }

                var customer = new Customer
                {
                    CustomerCode = txtCode.Text.Trim(),
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    CustomerName = fullName,
                    ContactNumber = txtContact.Text.Trim(),
                    EmailAddress = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Birthday = dtpBirthday.Value.Date,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                db.Customers.Add(customer);
                db.SaveChanges();

                if (chkAcceptTerms.Checked && _activeTermsId is not null)
                {
                    db.TermsAcceptances.Add(new TermsAcceptance
                    {
                        TermsAndConditionId = _activeTermsId.Value,
                        CustomerId = customer.CustomerId,
                        AcceptedAt = DateTime.UtcNow,
                        IpAddress = "127.0.0.1"
                    });
                    db.SaveChanges();
                }

                ActivityLogger.Log("Create", "Customer", customer.CustomerId,
                    $"Customer '{fullName}' registered");

                lblStatus.ForeColor = AppTheme.Success;
                lblStatus.Text = $"Customer '{fullName}' registered successfully.";

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // UPDATE
                var customer = db.Customers.FirstOrDefault(x => x.CustomerId == _customerId.Value);
                if (customer is null)
                {
                    lblStatus.Text = "Customer no longer exists.";
                    return;
                }

                customer.FirstName = firstName;
                customer.MiddleName = middleName;
                customer.LastName = lastName;
                customer.CustomerName = fullName;
                customer.ContactNumber = txtContact.Text.Trim();
                customer.EmailAddress = txtEmail.Text.Trim();
                customer.Address = txtAddress.Text.Trim();
                customer.Birthday = dtpBirthday.Value.Date;

                db.SaveChanges();

                ActivityLogger.Log("Update", "Customer", customer.CustomerId,
                    $"Customer '{fullName}' updated");

                lblStatus.ForeColor = AppTheme.Success;
                lblStatus.Text = "Customer updated successfully.";

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