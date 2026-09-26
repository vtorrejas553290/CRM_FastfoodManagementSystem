using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmFeedbackStatusEditor : Form
{
    private readonly int _feedbackId;

    private Label _lblCustomerValue = new();
    private Label _lblRatingValue = new();
    private Label _lblCategoryValue = new();
    private Label _lblSubmittedValue = new();
    private TextBox _txtComments = new();
    private ComboBox _cmbStatus = new();
    private Button _btnSave = new();
    private Button _btnCancel = new();
    private Label _lblStatus = new();

    public FrmFeedbackStatusEditor(int feedbackId)
    {
        _feedbackId = feedbackId;
        BuildUi();
        Load += FrmFeedbackStatusEditor_Load;
    }

    private void BuildUi()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        Text = "Edit Feedback Status";
        ClientSize = new Size(560, 500);

        // Read-only info panel
        var lblCustomer = new Label { Text = "Customer:", Location = new Point(20, 20), AutoSize = true };
        AppTheme.StyleLabel(lblCustomer);
        _lblCustomerValue = new Label { Location = new Point(140, 20), AutoSize = true, Font = AppTheme.FontBody };
        _lblCustomerValue.ForeColor = AppTheme.TextPrimary;

        var lblRating = new Label { Text = "Rating:", Location = new Point(20, 55), AutoSize = true };
        AppTheme.StyleLabel(lblRating);
        _lblRatingValue = new Label { Location = new Point(140, 55), AutoSize = true, Font = AppTheme.FontBody };
        _lblRatingValue.ForeColor = AppTheme.TextPrimary;

        var lblCategory = new Label { Text = "Category:", Location = new Point(20, 90), AutoSize = true };
        AppTheme.StyleLabel(lblCategory);
        _lblCategoryValue = new Label { Location = new Point(140, 90), AutoSize = true, Font = AppTheme.FontBody };
        _lblCategoryValue.ForeColor = AppTheme.TextPrimary;

        var lblSubmitted = new Label { Text = "Submitted:", Location = new Point(20, 125), AutoSize = true };
        AppTheme.StyleLabel(lblSubmitted);
        _lblSubmittedValue = new Label { Location = new Point(140, 125), AutoSize = true, Font = AppTheme.FontBody };
        _lblSubmittedValue.ForeColor = AppTheme.TextPrimary;

        var lblComments = new Label { Text = "Comments:", Location = new Point(20, 160), AutoSize = true };
        AppTheme.StyleLabel(lblComments);
        _txtComments = new TextBox
        {
            Location = new Point(140, 160),
            Width = 400,
            Height = 140,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            BackColor = AppTheme.Primary90,
            ForeColor = AppTheme.TextPrimary,
            Font = AppTheme.FontBody
        };

        var lblStatus = new Label { Text = "Status:", Location = new Point(20, 320), AutoSize = true, Font = AppTheme.FontSubheading };
        lblStatus.ForeColor = AppTheme.TextPrimary;

        _cmbStatus = new ComboBox
        {
            Location = new Point(140, 318),
            Width = 240,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        AppTheme.StyleInput(_cmbStatus);

        _btnSave = new Button { Text = "Save Status", Location = new Point(140, 370), Width = 160 };
        AppTheme.StyleSuccessButton(_btnSave);
        _btnSave.Click += BtnSave_Click;

        _btnCancel = new Button { Text = "Cancel", Location = new Point(320, 370), Width = 160 };
        AppTheme.StyleNeutralButton(_btnCancel);
        _btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

        _lblStatus = new Label
        {
            Location = new Point(20, 420),
            AutoSize = true,
            MaximumSize = new Size(520, 40)
        };
        AppTheme.StyleLabel(_lblStatus);

        Controls.AddRange(new Control[]
        {
            lblCustomer, _lblCustomerValue,
            lblRating, _lblRatingValue,
            lblCategory, _lblCategoryValue,
            lblSubmitted, _lblSubmittedValue,
            lblComments, _txtComments,
            lblStatus, _cmbStatus,
            _btnSave, _btnCancel, _lblStatus
        });
    }

    private void FrmFeedbackStatusEditor_Load(object? sender, EventArgs e)
    {
        _cmbStatus.Items.Clear();
        _cmbStatus.Items.AddRange(new object[] { "New", "Reviewed", "Resolved", "Archived" });

        try
        {
            using var db = AppServices.CreateTenantContext();
            var fb = db.CustomerFeedbacks
                .Include(x => x.Customer)
                .AsNoTracking()
                .FirstOrDefault(x => x.CustomerFeedbackId == _feedbackId);

            if (fb is null)
            {
                MessageBox.Show("Feedback not found.", "Error");
                Close();
                return;
            }

            _lblCustomerValue.Text = fb.Customer?.CustomerName ?? "(deleted)";
            _lblRatingValue.Text = new string('★', fb.Rating) + new string('☆', 5 - fb.Rating) + $"  ({fb.Rating}/5)";
            _lblCategoryValue.Text = fb.Category ?? "(none)";
            _lblSubmittedValue.Text = fb.SubmittedAt.ToString("yyyy-MM-dd HH:mm");
            _txtComments.Text = fb.Comments ?? string.Empty;

            // Preselect current status
            var idx = _cmbStatus.Items.IndexOf(fb.Status);
            _cmbStatus.SelectedIndex = idx >= 0 ? idx : 0;
        }
        catch (Exception ex)
        {
            _lblStatus.ForeColor = AppTheme.Error;
            _lblStatus.Text = ex.Message;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        _lblStatus.ForeColor = AppTheme.Error;
        _lblStatus.Text = string.Empty;

        if (_cmbStatus.SelectedItem is null)
        {
            _lblStatus.Text = "Please select a status.";
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();
            var fb = db.CustomerFeedbacks.FirstOrDefault(x => x.CustomerFeedbackId == _feedbackId);
            if (fb is null)
            {
                _lblStatus.Text = "Feedback no longer exists.";
                return;
            }

            fb.Status = _cmbStatus.SelectedItem.ToString() ?? fb.Status;
            db.SaveChanges();

            ActivityLogger.Log("Update", "CustomerFeedback", _feedbackId,
                $"Feedback #{_feedbackId} status set to '{fb.Status}'");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = ex.Message;
        }
    }
}