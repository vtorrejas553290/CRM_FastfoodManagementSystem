namespace CRM.winForms.Forms
{
    partial class FrmSendEmail
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCustomer;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblToLabel;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Label lblSubjectLabel;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label lblBodyLabel;
        private System.Windows.Forms.TextBox txtBody;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnOpenInMail;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();

            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblToLabel = new System.Windows.Forms.Label();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.lblSubjectLabel = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.lblBodyLabel = new System.Windows.Forms.Label();
            this.txtBody = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnOpenInMail = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 84;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 12);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(24, 16);
            this.lblTitle.Text = "Send Email";

            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            this.lblCustomer.Location = new System.Drawing.Point(26, 50);
            this.lblCustomer.Text = "Customer";

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblCustomer);

            // pnlBody
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);

            this.lblToLabel.AutoSize = true;
            this.lblToLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblToLabel.Location = new System.Drawing.Point(24, 12);
            this.lblToLabel.Text = "TO";

            this.txtTo.Location = new System.Drawing.Point(24, 34);
            this.txtTo.Size = new System.Drawing.Size(560, 25);
            this.txtTo.ReadOnly = true;

            this.lblSubjectLabel.AutoSize = true;
            this.lblSubjectLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubjectLabel.Location = new System.Drawing.Point(24, 72);
            this.lblSubjectLabel.Text = "SUBJECT";

            this.txtSubject.Location = new System.Drawing.Point(24, 94);
            this.txtSubject.Size = new System.Drawing.Size(560, 25);
            this.txtSubject.MaxLength = 200;

            this.lblBodyLabel.AutoSize = true;
            this.lblBodyLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBodyLabel.Location = new System.Drawing.Point(24, 132);
            this.lblBodyLabel.Text = "MESSAGE";

            this.txtBody.Location = new System.Drawing.Point(24, 154);
            this.txtBody.Size = new System.Drawing.Size(560, 220);
            this.txtBody.Multiline = true;
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.MaxLength = 2000;

            this.pnlBody.Controls.Add(this.lblToLabel);
            this.pnlBody.Controls.Add(this.txtTo);
            this.pnlBody.Controls.Add(this.lblSubjectLabel);
            this.pnlBody.Controls.Add(this.txtSubject);
            this.pnlBody.Controls.Add(this.lblBodyLabel);
            this.pnlBody.Controls.Add(this.txtBody);

            // pnlButtons
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 60;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);

            this.btnOpenInMail.Location = new System.Drawing.Point(364, 12);
            this.btnOpenInMail.Size = new System.Drawing.Size(180, 34);
            this.btnOpenInMail.Text = "Open in Mail Client";
            this.btnOpenInMail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnCancel.Location = new System.Drawing.Point(554, 12);
            this.btnCancel.Size = new System.Drawing.Size(90, 34);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnOpenInMail);
            this.pnlButtons.Controls.Add(this.btnCancel);

            // FrmSendEmail
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 470);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send Email";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}