namespace CRM.winForms.Forms
{
    partial class FrmAdminTermsGate
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.CheckBox chkIAccept;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnDecline;
        private System.Windows.Forms.Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.chkIAccept = new System.Windows.Forms.CheckBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnDecline = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(20, 18);
            this.lblHeader.Text = "Terms and Conditions — Action Required";

            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblVersion.Location = new System.Drawing.Point(20, 60);
            this.lblVersion.Text = "Version:";

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 85);
            this.lblTitle.Text = "Title";

            // =========================================================
            // txtContent — NO anchors, fixed size
            // =========================================================
            this.txtContent.Location = new System.Drawing.Point(20, 120);
            this.txtContent.Size = new System.Drawing.Size(700, 340);
            this.txtContent.Multiline = true;
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.WordWrap = true;
            this.txtContent.AcceptsReturn = true;
            this.txtContent.Font = new System.Drawing.Font("Consolas", 10F);

            this.chkIAccept.AutoSize = true;
            this.chkIAccept.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkIAccept.Location = new System.Drawing.Point(20, 480);
            this.chkIAccept.Text = "I have read and accept the Terms and Conditions above.";

            this.btnAccept.Location = new System.Drawing.Point(20, 515);
            this.btnAccept.Size = new System.Drawing.Size(160, 38);
            this.btnAccept.Text = "Accept && Continue";
            this.btnAccept.Enabled = false;

            this.btnDecline.Location = new System.Drawing.Point(190, 515);
            this.btnDecline.Size = new System.Drawing.Size(160, 38);
            this.btnDecline.Text = "Decline && Logout";

            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(20, 565);

            this.ClientSize = new System.Drawing.Size(760, 610);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.chkIAccept);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.btnDecline);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terms and Conditions — Action Required";
            this.ControlBox = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}