using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmRegisterCustomer : Form
{
    private readonly int? _customerId;
    private int? _activeTermsId;

    /// <summary>
    /// True if the customer being edited has already accepted the CURRENT
    /// active terms. In edit mode, this locks the checkbox to checked state.
    /// </summary>
    private bool _alreadyAcceptedCurrentTerms = false;

    public FrmRegisterCustomer() : this(null) { }

    public FrmRegisterCustomer(int? customerId)
    {
        _customerId = customerId;
        InitializeComponent();

        ApplyTheme();

        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
        btnViewTerms.Click += BtnViewTerms_Click;

        // Enable / disable Save in response to the checkbox.
        chkAcceptTerms.CheckedChanged += (_, __) => UpdateSaveEnabled();

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

        chkAcceptTerms.Font = AppTheme.FontBody;
        chkAcceptTerms.ForeColor = AppTheme.TextPrimary;
    }

    private void FrmRegisterCustomer_Load(object? sender, EventArgs e)
    {
        LoadActiveTerms();

        if (_customerId is null)
        {
            // ---- CREATE MODE ----
            // Start unchecked. Save button will be disabled until the user
            // ticks the box (if there are terms to accept).
            _alreadyAcceptedCurrentTerms = false;
            chkAcceptTerms.Checked = false;
            chkAcceptTerms.Enabled = _activeTermsId is not null;
            UpdateSaveEnabled();
            return;
        }

        // ---- EDIT MODE ----
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

            // Does this customer already have an acceptance of the active terms?
            if (_activeTermsId is not null)
            {
                _alreadyAcceptedCurrentTerms = db.TermsAcceptances
                    .AsNoTracking()
                    .Any(a => a.CustomerId == _customerId.Value
                           && a.TermsAndConditionId == _activeTermsId.Value);

                if (_alreadyAcceptedCurrentTerms)
                {
                    // Locked: checked + disabled.
                    chkAcceptTerms.Checked = true;
                    chkAcceptTerms.Enabled = false;
                    chkAcceptTerms.Text = "Customer accepted Terms & Conditions (cannot be changed)";
                }
                else
                {
                    // Not accepted yet: unchecked, enabled, required to save.
                    chkAcceptTerms.Checked = false;
                    chkAcceptTerms.Enabled = true;
                    chkAcceptTerms.Text = "Customer accepted Terms & Conditions";
                }
            }
            else
            {
                chkAcceptTerms.Enabled = false;
                chkAcceptTerms.Checked = false;
            }

            UpdateSaveEnabled();
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

    private static string ComposeFullName(string firstName, string? middleName, string lastName)
    {
        var parts = new List<string> { firstName.Trim() };

        if (!string.IsNullOrWhiteSpace(middleName))
            parts.Add(middleName.Trim());

        parts.Add(lastName.Trim());

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Called when the agreement checkbox changes, or when the form loads.
    /// Save is enabled unless there are terms that the customer hasn't accepted yet
    /// and the checkbox is unchecked.
    /// </summary>
    private void UpdateSaveEnabled()
    {
        bool termsExist = _activeTermsId is not null;
        bool mustAccept = termsExist && !_alreadyAcceptedCurrentTerms;
        bool accepted = chkAcceptTerms.Checked;

        // Save is enabled unless the user is required to accept and hasn't.
        btnSave.Enabled = !(mustAccept && !accepted);
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

        // ============ TERMS ENFORCEMENT ============
        // Hard gate: if the customer hasn't already accepted and there ARE
        // active terms, require the checkbox to be checked.
        bool termsExist = _activeTermsId is not null;

        if (termsExist && !_alreadyAcceptedCurrentTerms && !chkAcceptTerms.Checked)
        {
            lblStatus.Text = "Please accept the Terms and Conditions before saving.";
            MessageBox.Show(
                "The customer must accept the Terms and Conditions before you can save.",
                "Terms Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

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

                // Record the acceptance now that we have the new customer id.
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

                // If a newer terms version is active and the user just accepted it,
                // write the acceptance row now.
                if (termsExist
                    && !_alreadyAcceptedCurrentTerms
                    && chkAcceptTerms.Checked
                    && _activeTermsId is not null)
                {
                    db.TermsAcceptances.Add(new TermsAcceptance
                    {
                        TermsAndConditionId = _activeTermsId.Value,
                        CustomerId = customer.CustomerId,
                        AcceptedAt = DateTime.UtcNow,
                        IpAddress = "127.0.0.1"
                    });
                    db.SaveChanges();

                    _alreadyAcceptedCurrentTerms = true;
                }

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