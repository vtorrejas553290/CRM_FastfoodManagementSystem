namespace CRM.winForms.Forms
{
    partial class FrmSendSms
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.TableLayoutPanel tblBody;
        private System.Windows.Forms.FlowLayoutPanel flowTo;
        private System.Windows.Forms.Label lblToLabel;
        private System.Windows.Forms.Label lblTo;

        private System.Windows.Forms.TableLayoutPanel tblMessage;
        private System.Windows.Forms.FlowLayoutPanel flowPromotion;
        private System.Windows.Forms.Label lblPromotion;
        private System.Windows.Forms.ComboBox cmbPromotion;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.TextBox txtPreview;

        private System.Windows.Forms.Label lblHint;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSend;
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

            this.pnlBody = new System.Windows.Forms.Panel();
            this.tblBody = new System.Windows.Forms.TableLayoutPanel();
            this.flowTo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblToLabel = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();

            this.tblMessage = new System.Windows.Forms.TableLayoutPanel();
            this.flowPromotion = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPromotion = new System.Windows.Forms.Label();
            this.cmbPromotion = new System.Windows.Forms.ComboBox();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.txtPreview = new System.Windows.Forms.TextBox();

            this.lblHint = new System.Windows.Forms.Label();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.tblBody.SuspendLayout();
            this.flowTo.SuspendLayout();
            this.tblMessage.SuspendLayout();
            this.flowPromotion.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // FrmSendSms
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(820, 640);
            this.MinimumSize = new System.Drawing.Size(720, 520);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send SMS";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;

            // ============================================================
            // pnlHeader
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 64;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 16, 20, 12);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 18);
            this.lblTitle.Text = "Send SMS";

            this.pnlHeader.Controls.Add(this.lblTitle);

            // ============================================================
            // pnlBody — Dock Fill, contains tblBody
            // ============================================================
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);

            // ============================================================
            // tblBody — 1 column × 3 rows
            //   Row 0: To        (fixed 28)
            //   Row 1: Message   (Fill)
            //   Row 2: Hint      (fixed 40)
            // ============================================================
            this.tblBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBody.ColumnCount = 1;
            this.tblBody.RowCount = 3;
            this.tblBody.Padding = new System.Windows.Forms.Padding(0);
            this.tblBody.Margin = new System.Windows.Forms.Padding(0);

            this.tblBody.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblBody.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblBody.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblBody.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));

            // ---- Row 0: To row ----
            this.flowTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowTo.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowTo.WrapContents = false;
            this.flowTo.AutoSize = false;
            this.flowTo.Padding = new System.Windows.Forms.Padding(0);
            this.flowTo.Margin = new System.Windows.Forms.Padding(0);

            this.lblToLabel.AutoSize = true;
            this.lblToLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblToLabel.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.lblToLabel.Text = "To:";

            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblTo.Text = "Customer";

            this.flowTo.Controls.Add(this.lblToLabel);
            this.flowTo.Controls.Add(this.lblTo);

            this.tblBody.Controls.Add(this.flowTo, 0, 0);

            // ============================================================
            // Row 1: tblMessage — 2 columns × 2 rows
            // ============================================================
            this.tblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMessage.ColumnCount = 2;
            this.tblMessage.RowCount = 2;
            this.tblMessage.Padding = new System.Windows.Forms.Padding(0);
            this.tblMessage.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);

            this.tblMessage.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tblMessage.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));

            this.tblMessage.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tblMessage.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // ---- Promotion row ----
            this.flowPromotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPromotion.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowPromotion.WrapContents = false;
            this.flowPromotion.AutoSize = false;
            this.flowPromotion.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.flowPromotion.Margin = new System.Windows.Forms.Padding(0);

            this.lblPromotion.AutoSize = true;
            this.lblPromotion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPromotion.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblPromotion.Text = "Promotion:";

            this.cmbPromotion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotion.Width = 460;
            this.cmbPromotion.Margin = new System.Windows.Forms.Padding(0);

            this.flowPromotion.Controls.Add(this.lblPromotion);
            this.flowPromotion.Controls.Add(this.cmbPromotion);

            this.tblMessage.Controls.Add(this.flowPromotion, 0, 0);
            this.tblMessage.SetColumnSpan(this.flowPromotion, 2);

            // ---- Template ----
            var tblTemplate = new System.Windows.Forms.TableLayoutPanel();
            tblTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            tblTemplate.ColumnCount = 1;
            tblTemplate.RowCount = 2;
            tblTemplate.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            tblTemplate.Padding = new System.Windows.Forms.Padding(0);

            tblTemplate.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblTemplate.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tblTemplate.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.lblTemplate.Text = "MESSAGE TEMPLATE";
            this.lblTemplate.Anchor = System.Windows.Forms.AnchorStyles.Left;

            this.txtTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTemplate.Multiline = true;
            this.txtTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTemplate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTemplate.Margin = new System.Windows.Forms.Padding(0);

            tblTemplate.Controls.Add(this.lblTemplate, 0, 0);
            tblTemplate.Controls.Add(this.txtTemplate, 0, 1);

            // ---- Preview ----
            var tblPreview = new System.Windows.Forms.TableLayoutPanel();
            tblPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            tblPreview.ColumnCount = 1;
            tblPreview.RowCount = 2;
            tblPreview.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            tblPreview.Padding = new System.Windows.Forms.Padding(0);

            tblPreview.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblPreview.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tblPreview.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.lblPreview.AutoSize = true;
            this.lblPreview.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPreview.Margin = new System.Windows.Forms.Padding(0);
            this.lblPreview.Text = "PREVIEW";
            this.lblPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;

            this.txtPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreview.Multiline = true;
            this.txtPreview.ReadOnly = true;
            this.txtPreview.BackColor = System.Drawing.Color.White;
            this.txtPreview.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPreview.Margin = new System.Windows.Forms.Padding(0);

            tblPreview.Controls.Add(this.lblPreview, 0, 0);
            tblPreview.Controls.Add(this.txtPreview, 0, 1);

            this.tblMessage.Controls.Add(tblTemplate, 0, 1);
            this.tblMessage.Controls.Add(tblPreview, 1, 1);

            this.tblBody.Controls.Add(this.tblMessage, 0, 1);

            // ---- Row 2: Hint ----
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblHint.Margin = new System.Windows.Forms.Padding(0);
            this.lblHint.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHint.Text =
                "Send opens your default SMS app with the message prefilled. " +
                "Make sure your device (or Phone Link) can send SMS.";

            this.tblBody.Controls.Add(this.lblHint, 0, 2);

            this.pnlBody.Controls.Add(this.tblBody);

            // ============================================================
            // pnlButtons
            // ============================================================
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 64;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);

            this.btnCancel.Width = 100;
            this.btnCancel.Height = 40;
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnSend.Width = 140;
            this.btnSend.Height = 40;
            this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnSend.Text = "Send SMS";
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.FlatAppearance.BorderSize = 0;

            this.pnlButtons.Controls.Add(this.btnSend);
            this.pnlButtons.Controls.Add(this.btnCancel);

            this.pnlButtons.Resize += (_, __) =>
            {
                this.btnCancel.Location = new System.Drawing.Point(
                    this.pnlButtons.Width - this.btnCancel.Width - 20, 12);
                this.btnSend.Location = new System.Drawing.Point(
                    this.btnCancel.Left - this.btnSend.Width - 12, 12);
            };

            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.tblBody.ResumeLayout(false);
            this.tblBody.PerformLayout();
            this.flowTo.ResumeLayout(false);
            this.flowTo.PerformLayout();
            this.tblMessage.ResumeLayout(false);
            this.tblMessage.PerformLayout();
            this.flowPromotion.ResumeLayout(false);
            this.flowPromotion.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}