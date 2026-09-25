using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace CRM.winForms.Forms;

public partial class FrmSendEmail : Form
{
    private readonly int _customerId;
    private readonly string _customerName;
    private readonly string _email;

    public FrmSendEmail(int customerId, string customerName, string email)
    {
        _customerId = customerId;
        _customerName = customerName;
        _email = email;

        InitializeComponent();
        ApplyTheme();

        btnOpenInMail.Click += BtnOpenInMail_Click;
        btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblTitle.ForeColor = AppTheme.TextPrimary;
        lblCustomer.ForeColor = AppTheme.TextSecondary;
        lblCustomer.Text = _customerName;

        AppTheme.StyleLabel(lblToLabel);
        AppTheme.StyleLabel(lblSubjectLabel);
        AppTheme.StyleLabel(lblBodyLabel);

        AppTheme.StyleInput(txtTo);
        AppTheme.StyleInput(txtSubject);
        AppTheme.StyleInput(txtBody);

        AppTheme.StyleSuccessButton(btnOpenInMail);
        AppTheme.StyleNeutralButton(btnCancel);
    }

    // Form load — populate default subject and body
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        txtTo.Text = _email;
        txtSubject.Text = $"A special offer from Fastfood MS, {_customerName}!";
        txtBody.Text =
            $"Hi {_customerName},\r\n\r\n" +
            "We miss you! It's been a while since your last visit, and we'd love to see you again.\r\n\r\n" +
            "Here's a special offer just for you — come by and enjoy a discount on your next order.\r\n\r\n" +
            "See you soon!\r\n" +
            "Fastfood MS";
    }

    private void BtnOpenInMail_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSubject.Text) &&
            string.IsNullOrWhiteSpace(txtBody.Text))
        {
            MessageBox.Show("Please enter a subject or a message before sending.",
                "Empty email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            // Build mailto URI with subject and body
            string subject = Uri.EscapeDataString(txtSubject.Text.Trim());
            string body = Uri.EscapeDataString(txtBody.Text.Trim());
            string mailto = $"mailto:{_email}?subject={subject}&body={body}";

            Process.Start(new ProcessStartInfo
            {
                FileName = mailto,
                UseShellExecute = true
            });

            // Log the send
            ActivityLogger.Log("Email", "Customer", _customerId,
                $"Opened email compose for '{_customerName}' <{_email}> | Subject: {txtSubject.Text.Trim()}");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not open mail client:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}