namespace CRM.winForms.Forms
{
    partial class FrmContactCustomer
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblContactTitle;
        private System.Windows.Forms.Label lblPhoneLabel;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmailLabel;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblAddressLabel;
        private System.Windows.Forms.Label lblAddress;

        private System.Windows.Forms.Label lblContextTitle;
        private System.Windows.Forms.Label lblLastOrder;
        private System.Windows.Forms.Label lblPoints;

        private System.Windows.Forms.FlowLayoutPanel flowActions;
        private System.Windows.Forms.Button btnSms;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnCopy;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblContactTitle = new System.Windows.Forms.Label();
            this.lblPhoneLabel = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblEmailLabel = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblAddressLabel = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();

            this.lblContextTitle = new System.Windows.Forms.Label();
            this.lblLastOrder = new System.Windows.Forms.Label();
            this.lblPoints = new System.Windows.Forms.Label();

            this.flowActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSms = new System.Windows.Forms.Button();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.flowActions.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // FrmContactCustomer
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(900, 480);
            this.MinimumSize = new System.Drawing.Size(820, 420);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contact Customer";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.AutoScroll = true;

            // ============================================================
            // pnlHeader
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 100;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(32, 20, 32, 16);

            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblCustomerName.Location = new System.Drawing.Point(32, 20);
            this.lblCustomerName.Text = "Customer Name";

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.Location = new System.Drawing.Point(34, 60);
            this.lblSubtitle.Text = "Retention status";

            this.pnlHeader.Controls.Add(this.lblCustomerName);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // ============================================================
            // pnlBody
            // ============================================================
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(32, 16, 32, 16);
            this.pnlBody.AutoScroll = true;

            // ---- Contact information ----
            this.lblContactTitle.AutoSize = true;
            this.lblContactTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblContactTitle.Location = new System.Drawing.Point(32, 16);
            this.lblContactTitle.Text = "CONTACT INFORMATION";

            this.lblPhoneLabel.AutoSize = true;
            this.lblPhoneLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPhoneLabel.Location = new System.Drawing.Point(32, 52);
            this.lblPhoneLabel.Text = "Phone:";

            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblPhone.Location = new System.Drawing.Point(150, 52);
            this.lblPhone.Text = "—";

            this.lblEmailLabel.AutoSize = true;
            this.lblEmailLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmailLabel.Location = new System.Drawing.Point(32, 84);
            this.lblEmailLabel.Text = "Email:";

            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblEmail.Location = new System.Drawing.Point(150, 84);
            this.lblEmail.Text = "—";

            this.lblAddressLabel.AutoSize = true;
            this.lblAddressLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAddressLabel.Location = new System.Drawing.Point(32, 116);
            this.lblAddressLabel.Text = "Address:";

            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblAddress.Location = new System.Drawing.Point(150, 116);
            this.lblAddress.MaximumSize = new System.Drawing.Size(700, 0);
            this.lblAddress.Text = "—";

            // ---- Retention context ----
            this.lblContextTitle.AutoSize = true;
            this.lblContextTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblContextTitle.Location = new System.Drawing.Point(32, 168);
            this.lblContextTitle.Text = "RETENTION CONTEXT";

            this.lblLastOrder.AutoSize = true;
            this.lblLastOrder.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblLastOrder.Location = new System.Drawing.Point(32, 202);
            this.lblLastOrder.Text = "Last order: —";

            this.lblPoints.AutoSize = true;
            this.lblPoints.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblPoints.Location = new System.Drawing.Point(32, 232);
            this.lblPoints.Text = "Loyalty points: —";

            // ---- Contact actions ----
            this.flowActions.Location = new System.Drawing.Point(32, 280);
            this.flowActions.AutoSize = true;
            this.flowActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowActions.WrapContents = false;

            this.btnSms.Size = new System.Drawing.Size(120, 42);
            this.btnSms.Text = "SMS";
            this.btnSms.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.btnEmail.Size = new System.Drawing.Size(120, 42);
            this.btnEmail.Text = "Email";
            this.btnEmail.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.btnCopy.Size = new System.Drawing.Size(180, 42);
            this.btnCopy.Text = "Copy Contact";
            this.btnCopy.Margin = new System.Windows.Forms.Padding(0);

            this.flowActions.Controls.Add(this.btnSms);
            this.flowActions.Controls.Add(this.btnEmail);
            this.flowActions.Controls.Add(this.btnCopy);

            // Add to pnlBody
            this.pnlBody.Controls.Add(this.lblContactTitle);
            this.pnlBody.Controls.Add(this.lblPhoneLabel);
            this.pnlBody.Controls.Add(this.lblPhone);
            this.pnlBody.Controls.Add(this.lblEmailLabel);
            this.pnlBody.Controls.Add(this.lblEmail);
            this.pnlBody.Controls.Add(this.lblAddressLabel);
            this.pnlBody.Controls.Add(this.lblAddress);
            this.pnlBody.Controls.Add(this.lblContextTitle);
            this.pnlBody.Controls.Add(this.lblLastOrder);
            this.pnlBody.Controls.Add(this.lblPoints);
            this.pnlBody.Controls.Add(this.flowActions);

            // ============================================================
            // pnlButtons
            // ============================================================
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 70;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(32, 14, 32, 14);

            this.btnClose.Size = new System.Drawing.Size(100, 42);
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Resize += (_, __) =>
            {
                this.btnClose.Location = new System.Drawing.Point(
                    this.pnlButtons.Width - this.btnClose.Width - 32, 14);
            };

            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.flowActions.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}