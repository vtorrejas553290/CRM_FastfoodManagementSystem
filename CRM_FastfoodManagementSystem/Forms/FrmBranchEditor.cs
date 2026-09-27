using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmBranchEditor : Form
{
    private readonly IBranchController _controller = new BranchController();

    private readonly int? _branchId;   // null = Add, otherwise Edit

    public FrmBranchEditor(int? branchId)
    {
        _branchId = branchId;
        InitializeComponent();
        ApplyTheme();

        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, __) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        Load += FrmBranchEditor_Load;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        lblCode.ForeColor = AppTheme.TextPrimary;
        lblName.ForeColor = AppTheme.TextPrimary;
        lblAddress.ForeColor = AppTheme.TextPrimary;
        lblContact.ForeColor = AppTheme.TextPrimary;
        lblError.ForeColor = AppTheme.Danger;

        AppTheme.StyleInput(txtCode);
        AppTheme.StyleInput(txtName);
        AppTheme.StyleInput(txtAddress);
        AppTheme.StyleInput(txtContact);

        AppTheme.StylePrimaryButton(btnSave);
        AppTheme.StyleSecondaryButton(btnCancel);

        lblError.Text = string.Empty;
    }

    private void FrmBranchEditor_Load(object? sender, EventArgs e)
    {
        if (_branchId is null)
        {
            // Add mode
            Text = "Add Branch";
            lblTitle.Text = "Add Branch";
            lblSubtitle.Text = "Create a new branch for this tenant";
            return;
        }

        // Edit mode — load existing
        Text = "Edit Branch";
        lblTitle.Text = "Edit Branch";
        lblSubtitle.Text = "Update branch information";

        try
        {
            var row = _controller.GetBranch(_branchId.Value, UserSession.CompanyId);
            if (row is null)
            {
                lblError.Text = "Branch not found.";
                return;
            }

            txtCode.Text = row.BranchCode;
            txtName.Text = row.BranchName;
            txtAddress.Text = row.Address;
            txtContact.Text = row.ContactNumber;
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        var code = txtCode.Text.Trim();
        var name = txtName.Text.Trim();
        var address = txtAddress.Text.Trim();
        var contact = txtContact.Text.Trim();

        // Quick client-side validation (server-side also validates)
        if (string.IsNullOrWhiteSpace(code))
        {
            lblError.Text = "Branch Code is required.";
            txtCode.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            lblError.Text = "Branch Name is required.";
            txtName.Focus();
            return;
        }

        try
        {
            btnSave.Enabled = false;

            if (_branchId is null)
            {
                // CREATE
                var newId = _controller.CreateBranch(
                    UserSession.CompanyId,
                    code, name,
                    string.IsNullOrWhiteSpace(address) ? null : address,
                    string.IsNullOrWhiteSpace(contact) ? null : contact,
                    out var error);

                if (newId is null)
                {
                    lblError.Text = error ?? "Failed to create branch.";
                    return;
                }
            }
            else
            {
                // UPDATE
                var ok = _controller.UpdateBranch(
                    _branchId.Value,
                    UserSession.CompanyId,
                    code, name,
                    string.IsNullOrWhiteSpace(address) ? null : address,
                    string.IsNullOrWhiteSpace(contact) ? null : contact,
                    out var error);

                if (!ok)
                {
                    lblError.Text = error ?? "Failed to update branch.";
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }
}