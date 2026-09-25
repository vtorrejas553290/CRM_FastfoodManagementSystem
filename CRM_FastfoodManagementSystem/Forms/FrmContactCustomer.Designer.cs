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
        private System.Windows.Forms.Button btnCall;
        private System.Windows.Forms.Button btnSms;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnCopy;

        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.TableLayoutPanel tblLog;
        private System.Windows.Forms.Label lblOutcome;
        private System.Windows.Forms.ComboBox cmbOutcome;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblFollowUp;
        private System.Windows.Forms.FlowLayoutPanel flowFollowUp;
        private System.Windows.Forms.CheckBox chkFollowUp;
        private System.Windows.Forms.DateTimePicker dtpFollowUp;

        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.DataGridView gridContactHistory;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSaveAndClose;
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
            this.btnCall = new System.Windows.Forms.Button();
            this.btnSms = new System.Windows.Forms.Button();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();

            this.lblLogTitle = new System.Windows.Forms.Label();
            this.tblLog = new System.Windows.Forms.TableLayoutPanel();
            this.lblOutcome = new System.Windows.Forms.Label();
            this.cmbOutcome = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblFollowUp = new System.Windows.Forms.Label();
            this.flowFollowUp = new System.Windows.Forms.FlowLayoutPanel();
            this.chkFollowUp = new System.Windows.Forms.CheckBox();
            this.dtpFollowUp = new System.Windows.Forms.DateTimePicker();

            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.gridContactHistory = new System.Windows.Forms.DataGridView();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.flowActions.SuspendLayout();
            this.tblLog.SuspendLayout();
            this.flowFollowUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridContactHistory)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // pnlHeader — taller so name + subtitle have room
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
            // pnlBody — larger padding, wider content
            // ============================================================
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(32, 16, 32, 16);
            this.pnlBody.AutoScroll = true;

            // ---- Contact info ----
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

            // ---- Contact actions (larger buttons) ----
            this.flowActions.Location = new System.Drawing.Point(32, 272);
            this.flowActions.AutoSize = true;
            this.flowActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowActions.WrapContents = false;

            this.btnCall.Size = new System.Drawing.Size(120, 42);
            this.btnCall.Text = "Call";
            this.btnCall.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.btnSms.Size = new System.Drawing.Size(120, 42);
            this.btnSms.Text = "SMS";
            this.btnSms.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.btnEmail.Size = new System.Drawing.Size(120, 42);
            this.btnEmail.Text = "Email";
            this.btnEmail.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.btnCopy.Size = new System.Drawing.Size(180, 42);
            this.btnCopy.Text = "Copy Contact";
            this.btnCopy.Margin = new System.Windows.Forms.Padding(0);

            this.flowActions.Controls.Add(this.btnCall);
            this.flowActions.Controls.Add(this.btnSms);
            this.flowActions.Controls.Add(this.btnEmail);
            this.flowActions.Controls.Add(this.btnCopy);

            // ---- Log the outcome section ----
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.Location = new System.Drawing.Point(32, 340);
            this.lblLogTitle.Text = "LOG THE OUTCOME";

            this.tblLog.Location = new System.Drawing.Point(32, 376);
            this.tblLog.Size = new System.Drawing.Size(816, 260);
            this.tblLog.ColumnCount = 2;
            this.tblLog.RowCount = 3;
            this.tblLog.Padding = new System.Windows.Forms.Padding(0);
            this.tblLog.Margin = new System.Windows.Forms.Padding(0);

            this.tblLog.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tblLog.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblLog.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblLog.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLog.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));

            // Row 0 — Outcome
            this.lblOutcome.AutoSize = true;
            this.lblOutcome.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblOutcome.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOutcome.Text = "Outcome:";
            this.lblOutcome.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.cmbOutcome.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutcome.Anchor = System.Windows.Forms.AnchorStyles.Left |
                                     System.Windows.Forms.AnchorStyles.Right;
            this.cmbOutcome.Width = 320;
            this.cmbOutcome.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);

            this.tblLog.Controls.Add(this.lblOutcome, 0, 0);
            this.tblLog.Controls.Add(this.cmbOutcome, 1, 0);

            // Row 1 — Notes
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNotes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNotes.Text = "Notes:";
            this.lblNotes.Margin = new System.Windows.Forms.Padding(0, 8, 12, 0);

            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Multiline = true;
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.MaxLength = 1000;
            this.txtNotes.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);

            this.tblLog.Controls.Add(this.lblNotes, 0, 1);
            this.tblLog.Controls.Add(this.txtNotes, 1, 1);

            // Row 2 — Follow-up
            this.lblFollowUp.AutoSize = true;
            this.lblFollowUp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFollowUp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFollowUp.Text = "Follow-up:";
            this.lblFollowUp.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.flowFollowUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFollowUp.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowFollowUp.WrapContents = false;
            this.flowFollowUp.AutoSize = false;
            this.flowFollowUp.Margin = new System.Windows.Forms.Padding(0);
            this.flowFollowUp.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);

            this.chkFollowUp.AutoSize = true;
            this.chkFollowUp.Text = "Schedule";
            this.chkFollowUp.Margin = new System.Windows.Forms.Padding(0, 6, 20, 0);

            this.dtpFollowUp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFollowUp.Width = 160;
            this.dtpFollowUp.Enabled = false;
            this.dtpFollowUp.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);

            this.flowFollowUp.Controls.Add(this.chkFollowUp);
            this.flowFollowUp.Controls.Add(this.dtpFollowUp);

            this.tblLog.Controls.Add(this.lblFollowUp, 0, 2);
            this.tblLog.Controls.Add(this.flowFollowUp, 1, 2);

            // ---- Contact history section ----
            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.Location = new System.Drawing.Point(32, 656);
            this.lblHistoryTitle.Text = "CONTACT HISTORY";

            this.gridContactHistory.Location = new System.Drawing.Point(32, 692);
            this.gridContactHistory.Size = new System.Drawing.Size(816, 200);
            this.gridContactHistory.ReadOnly = true;
            this.gridContactHistory.AllowUserToAddRows = false;
            this.gridContactHistory.AllowUserToDeleteRows = false;
            this.gridContactHistory.RowHeadersVisible = false;
            this.gridContactHistory.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridContactHistory.MultiSelect = false;
            this.gridContactHistory.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridContactHistory.BackgroundColor = System.Drawing.Color.White;
            this.gridContactHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridContactHistory.ColumnHeadersHeight = 38;

            // Add everything to pnlBody
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
            this.pnlBody.Controls.Add(this.lblLogTitle);
            this.pnlBody.Controls.Add(this.tblLog);
            this.pnlBody.Controls.Add(this.lblHistoryTitle);
            this.pnlBody.Controls.Add(this.gridContactHistory);

            // ============================================================
            // pnlButtons
            // ============================================================
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 70;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(32, 14, 32, 14);

            this.btnSaveAndClose.Location = new System.Drawing.Point(556, 14);
            this.btnSaveAndClose.Size = new System.Drawing.Size(180, 42);
            this.btnSaveAndClose.Text = "Save & Close";
            this.btnSaveAndClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnClose.Location = new System.Drawing.Point(748, 14);
            this.btnClose.Size = new System.Drawing.Size(100, 42);
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnSaveAndClose);
            this.pnlButtons.Controls.Add(this.btnClose);

            // ============================================================
            // FrmContactCustomer
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 950);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contact Customer";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.flowActions.ResumeLayout(false);
            this.tblLog.ResumeLayout(false);
            this.tblLog.PerformLayout();
            this.flowFollowUp.ResumeLayout(false);
            this.flowFollowUp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridContactHistory)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}