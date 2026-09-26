namespace CRM.winForms.Forms
{
    partial class FrmReceipt
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.Label lblReceiptText;

        private System.Windows.Forms.Panel pnlFooter;
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.pnlScroll = new System.Windows.Forms.Panel();
            this.lblReceiptText = new System.Windows.Forms.Label();

            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // FrmReceipt
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(620, 760);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Receipt";

            // ============================================================
            // pnlHeader
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 72;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 14, 24, 12);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblTitle.Location = new System.Drawing.Point(24, 14);
            this.lblTitle.Text = "Payment Received";

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblSubtitle.Location = new System.Drawing.Point(26, 46);
            this.lblSubtitle.Text = "Receipt for the completed transaction";

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // ============================================================
            // pnlScroll — scrollable container for the receipt
            // ============================================================
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.AutoScroll = true;
            this.pnlScroll.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlScroll.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);

            // lblReceiptText — a single AutoSize label with monospaced font.
            // AutoSize = true guarantees no clipping: the label grows to fit
            // its text, and pnlScroll scrolls if it exceeds the visible area.
            this.lblReceiptText.AutoSize = true;
            this.lblReceiptText.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular);
            this.lblReceiptText.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblReceiptText.BackColor = System.Drawing.Color.White;
            this.lblReceiptText.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.lblReceiptText.Location = new System.Drawing.Point(24, 16);
            this.lblReceiptText.Text = "Receipt";

            this.pnlScroll.Controls.Add(this.lblReceiptText);

            // ============================================================
            // pnlFooter
            // ============================================================
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 60;
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(24, 10, 24, 10);

            this.btnPrint.Location = new System.Drawing.Point(360, 12);
            this.btnPrint.Size = new System.Drawing.Size(120, 36);
            this.btnPrint.Text = "Print";
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.FlatAppearance.BorderSize = 0;

            this.btnClose.Location = new System.Drawing.Point(490, 12);
            this.btnClose.Size = new System.Drawing.Size(110, 36);
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlFooter.Controls.Add(this.btnPrint);
            this.pnlFooter.Controls.Add(this.btnClose);

            // ============================================================
            // Compose
            // ============================================================
            this.Controls.Add(this.pnlScroll);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlScroll.ResumeLayout(false);
            this.pnlScroll.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}