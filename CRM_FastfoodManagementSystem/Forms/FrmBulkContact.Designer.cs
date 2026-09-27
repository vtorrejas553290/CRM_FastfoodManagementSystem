namespace CRM.winForms.Forms
{
    partial class FrmBulkContact
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tblRoot;

        // Row 0 — header
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        // Row 1 — channel + promotion
        private System.Windows.Forms.TableLayoutPanel tblTopRow;
        private System.Windows.Forms.FlowLayoutPanel flowChannel;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.RadioButton rbSms;
        private System.Windows.Forms.RadioButton rbEmail;
        private System.Windows.Forms.RadioButton rbBoth;
        private System.Windows.Forms.FlowLayoutPanel flowPromotion;
        private System.Windows.Forms.Label lblPromotion;
        private System.Windows.Forms.ComboBox cmbPromotion;

        // Row 2 — template + preview
        private System.Windows.Forms.TableLayoutPanel tblMessages;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.TextBox txtPreview;

        // Row 3 — recipients
        private System.Windows.Forms.TableLayoutPanel tblRecipients;
        private System.Windows.Forms.Label lblRecipients;
        private System.Windows.Forms.DataGridView gridRecipients;
        private System.Windows.Forms.Label lblSummary;

        // Row 4 — footer
        private System.Windows.Forms.FlowLayoutPanel flowFooter;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tblRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.tblTopRow = new System.Windows.Forms.TableLayoutPanel();
            this.flowChannel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblChannel = new System.Windows.Forms.Label();
            this.rbSms = new System.Windows.Forms.RadioButton();
            this.rbEmail = new System.Windows.Forms.RadioButton();
            this.rbBoth = new System.Windows.Forms.RadioButton();

            this.flowPromotion = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPromotion = new System.Windows.Forms.Label();
            this.cmbPromotion = new System.Windows.Forms.ComboBox();

            this.tblMessages = new System.Windows.Forms.TableLayoutPanel();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.txtPreview = new System.Windows.Forms.TextBox();

            this.tblRecipients = new System.Windows.Forms.TableLayoutPanel();
            this.lblRecipients = new System.Windows.Forms.Label();
            this.gridRecipients = new System.Windows.Forms.DataGridView();
            this.lblSummary = new System.Windows.Forms.Label();

            this.flowFooter = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.tblRoot.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tblTopRow.SuspendLayout();
            this.flowChannel.SuspendLayout();
            this.flowPromotion.SuspendLayout();
            this.tblMessages.SuspendLayout();
            this.tblRecipients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecipients)).BeginInit();
            this.flowFooter.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // FrmBulkContact
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1024, 720);
            this.MinimumSize = new System.Drawing.Size(820, 560);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bulk Contact";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.AutoScroll = true;

            // ============================================================
            // tblRoot — 1 column × 5 rows
            //   Row 0: header (fixed 80)
            //   Row 1: channel + promotion (fixed 84)
            //   Row 2: template + preview (40%)
            //   Row 3: recipients (60%)
            //   Row 4: footer (fixed 68)
            // ============================================================
            this.tblRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRoot.ColumnCount = 1;
            this.tblRoot.RowCount = 5;
            this.tblRoot.Padding = new System.Windows.Forms.Padding(0);
            this.tblRoot.Margin = new System.Windows.Forms.Padding(0);

            this.tblRoot.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblRoot.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tblRoot.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tblRoot.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblRoot.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblRoot.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));

            // ============================================================
            // Row 0: pnlHeader
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 12, 20, 10);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 12);
            this.lblTitle.Text = "Bulk Contact";

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.Location = new System.Drawing.Point(22, 46);
            this.lblSubtitle.Text = "Send one message to many customers at once";

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // ============================================================
            // Row 1: tblTopRow — 2 columns
            //   Col 0: channel radios
            //   Col 1: promotion dropdown
            // ============================================================
            this.tblTopRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTopRow.ColumnCount = 2;
            this.tblTopRow.RowCount = 1;
            this.tblTopRow.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
            this.tblTopRow.Margin = new System.Windows.Forms.Padding(0);
            this.tblTopRow.BackColor = System.Drawing.Color.White;

            this.tblTopRow.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tblTopRow.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblTopRow.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // ---- flowChannel ----
            this.flowChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowChannel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowChannel.WrapContents = false;
            this.flowChannel.AutoSize = false;
            this.flowChannel.Padding = new System.Windows.Forms.Padding(0);
            this.flowChannel.Margin = new System.Windows.Forms.Padding(0);

            this.lblChannel.AutoSize = true;
            this.lblChannel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblChannel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblChannel.Text = "CHANNEL";

            var flowRadios = new System.Windows.Forms.FlowLayoutPanel();
            flowRadios.AutoSize = true;
            flowRadios.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            flowRadios.WrapContents = false;
            flowRadios.Margin = new System.Windows.Forms.Padding(0);

            this.rbSms.AutoSize = true;
            this.rbSms.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.rbSms.Text = "SMS";

            this.rbEmail.AutoSize = true;
            this.rbEmail.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.rbEmail.Text = "Email";

            this.rbBoth.AutoSize = true;
            this.rbBoth.Margin = new System.Windows.Forms.Padding(0);
            this.rbBoth.Text = "Both";

            flowRadios.Controls.Add(this.rbSms);
            flowRadios.Controls.Add(this.rbEmail);
            flowRadios.Controls.Add(this.rbBoth);

            this.flowChannel.Controls.Add(this.lblChannel);
            this.flowChannel.Controls.Add(flowRadios);

            // ---- flowPromotion ----
            this.flowPromotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPromotion.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPromotion.WrapContents = false;
            this.flowPromotion.AutoSize = false;
            this.flowPromotion.Padding = new System.Windows.Forms.Padding(0);
            this.flowPromotion.Margin = new System.Windows.Forms.Padding(0);

            this.lblPromotion.AutoSize = true;
            this.lblPromotion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPromotion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblPromotion.Text = "PROMOTION";

            this.cmbPromotion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotion.Width = 460;
            this.cmbPromotion.Margin = new System.Windows.Forms.Padding(0);

            this.flowPromotion.Controls.Add(this.lblPromotion);
            this.flowPromotion.Controls.Add(this.cmbPromotion);

            this.tblTopRow.Controls.Add(this.flowChannel, 0, 0);
            this.tblTopRow.Controls.Add(this.flowPromotion, 1, 0);

            // ============================================================
            // Row 2: tblMessages — 2 columns
            //   Col 0: template
            //   Col 1: preview
            // ============================================================
            this.tblMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMessages.ColumnCount = 2;
            this.tblMessages.RowCount = 1;
            this.tblMessages.Padding = new System.Windows.Forms.Padding(20, 6, 20, 6);
            this.tblMessages.Margin = new System.Windows.Forms.Padding(0);
            this.tblMessages.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            this.tblMessages.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tblMessages.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));

            this.tblMessages.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // ---- Template label + box ----
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

            // ---- Preview label + box ----
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

            this.tblMessages.Controls.Add(tblTemplate, 0, 0);
            this.tblMessages.Controls.Add(tblPreview, 1, 0);

            // ============================================================
            // Row 3: tblRecipients — 1 column × 3 rows
            //   Row 0: label
            //   Row 1: grid (fills)
            //   Row 2: summary
            // ============================================================
            this.tblRecipients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRecipients.ColumnCount = 1;
            this.tblRecipients.RowCount = 3;
            this.tblRecipients.Padding = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.tblRecipients.Margin = new System.Windows.Forms.Padding(0);
            this.tblRecipients.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            this.tblRecipients.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblRecipients.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tblRecipients.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblRecipients.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));

            this.lblRecipients.AutoSize = true;
            this.lblRecipients.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRecipients.Margin = new System.Windows.Forms.Padding(0);
            this.lblRecipients.Text = "RECIPIENTS";
            this.lblRecipients.Anchor = System.Windows.Forms.AnchorStyles.Left;

            this.gridRecipients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRecipients.AutoSize = false;
            this.gridRecipients.BackgroundColor = System.Drawing.Color.White;
            this.gridRecipients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridRecipients.ColumnHeadersHeight = 34;
            this.gridRecipients.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);

            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblSummary.Margin = new System.Windows.Forms.Padding(0);
            this.lblSummary.Text = "0 selected";
            this.lblSummary.Anchor = System.Windows.Forms.AnchorStyles.Left;

            this.tblRecipients.Controls.Add(this.lblRecipients, 0, 0);
            this.tblRecipients.Controls.Add(this.gridRecipients, 0, 1);
            this.tblRecipients.Controls.Add(this.lblSummary, 0, 2);

            // ============================================================
            // Row 4: flowFooter (right-aligned)
            // ============================================================
            this.flowFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFooter.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowFooter.WrapContents = false;
            this.flowFooter.AutoSize = false;
            this.flowFooter.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.flowFooter.Margin = new System.Windows.Forms.Padding(0);
            this.flowFooter.BackColor = System.Drawing.Color.White;

            this.btnClose.Width = 120;
            this.btnClose.Height = 44;
            this.btnClose.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnSend.Width = 180;
            this.btnSend.Height = 44;
            this.btnSend.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend.Text = "Send to All Selected";
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.FlatAppearance.BorderSize = 0;

            this.flowFooter.Controls.Add(this.btnClose);
            this.flowFooter.Controls.Add(this.btnSend);

            // ============================================================
            // Add rows to tblRoot
            // ============================================================
            this.tblRoot.Controls.Add(this.pnlHeader, 0, 0);
            this.tblRoot.Controls.Add(this.tblTopRow, 0, 1);
            this.tblRoot.Controls.Add(this.tblMessages, 0, 2);
            this.tblRoot.Controls.Add(this.tblRecipients, 0, 3);
            this.tblRoot.Controls.Add(this.flowFooter, 0, 4);

            // ============================================================
            // Add tblRoot to form
            // ============================================================
            this.Controls.Add(this.tblRoot);

            this.tblRoot.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tblTopRow.ResumeLayout(false);
            this.flowChannel.ResumeLayout(false);
            this.flowChannel.PerformLayout();
            this.flowPromotion.ResumeLayout(false);
            this.flowPromotion.PerformLayout();
            this.tblMessages.ResumeLayout(false);
            this.tblRecipients.ResumeLayout(false);
            this.tblRecipients.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecipients)).EndInit();
            this.flowFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}