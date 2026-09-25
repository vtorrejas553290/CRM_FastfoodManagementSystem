namespace CRM.winForms.Forms
{
#nullable disable

    partial class FrmTermsViewer
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblVersion
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblVersion.Location = new System.Drawing.Point(20, 18);
            this.lblVersion.Text = "Version:";

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 45);
            this.lblTitle.Text = "Title";

            // txtContent
            this.txtContent.Location = new System.Drawing.Point(20, 90);
            this.txtContent.Size = new System.Drawing.Size(660, 380);
            this.txtContent.Multiline = true;
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.WordWrap = true;
            this.txtContent.AcceptsReturn = true;
            this.txtContent.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtContent.TabIndex = 0;
            this.txtContent.TabStop = false;

            // btnPrint
            this.btnPrint.Location = new System.Drawing.Point(20, 485);
            this.btnPrint.Size = new System.Drawing.Size(180, 38);
            this.btnPrint.Text = "Print / Save as PDF";

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(210, 485);
            this.btnClose.Size = new System.Drawing.Size(120, 38);
            this.btnClose.Text = "Close";

            // FrmTermsViewer
            this.ClientSize = new System.Drawing.Size(710, 545);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terms and Conditions";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}