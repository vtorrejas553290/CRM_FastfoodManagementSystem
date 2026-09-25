namespace CRM.winForms.Forms
{
    partial class FrmTermsEditor
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;

        // Existing terms section
        private System.Windows.Forms.Panel pnlTermsWrap;
        private System.Windows.Forms.Panel pnlTermsToolbar;
        private System.Windows.Forms.FlowLayoutPanel flowTermsActions;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.DataGridView gridTerms;

        // New version section
        private System.Windows.Forms.Panel pnlNew;
        private System.Windows.Forms.Label lblNewTitle;
        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Panel pnlFormFooter;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();

            this.pnlTermsWrap = new System.Windows.Forms.Panel();
            this.pnlTermsToolbar = new System.Windows.Forms.Panel();
            this.flowTermsActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnView = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnArchive = new System.Windows.Forms.Button();
            this.gridTerms = new System.Windows.Forms.DataGridView();

            this.pnlNew = new System.Windows.Forms.Panel();
            this.lblNewTitle = new System.Windows.Forms.Label();
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.pnlFormFooter = new System.Windows.Forms.Panel();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlTermsWrap.SuspendLayout();
            this.pnlTermsToolbar.SuspendLayout();
            this.flowTermsActions.SuspendLayout();
            this.pnlNew.SuspendLayout();
            this.tblForm.SuspendLayout();
            this.pnlFormFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTerms)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlHeader — title bar
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 52;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 12, 20, 8);

            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(20, 14);
            this.lblHeader.Text = "Existing Terms & Conditions";

            this.pnlHeader.Controls.Add(this.lblHeader);

            // ============================================================
            // pnlTermsWrap — top half: grid + toolbar
            // ============================================================
            this.pnlTermsWrap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTermsWrap.Height = 320;
            this.pnlTermsWrap.Padding = new System.Windows.Forms.Padding(20, 0, 20, 12);

            // pnlTermsToolbar — buttons in a right-aligned row above the grid
            this.pnlTermsToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTermsToolbar.Height = 46;
            this.pnlTermsToolbar.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);

            this.flowTermsActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowTermsActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowTermsActions.WrapContents = false;
            this.flowTermsActions.AutoSize = true;
            this.flowTermsActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowTermsActions.Padding = new System.Windows.Forms.Padding(0);

            this.btnView.Size = new System.Drawing.Size(130, 34);
            this.btnView.Text = "View / Print";
            this.btnView.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.btnRefresh.Size = new System.Drawing.Size(100, 34);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.btnArchive.Size = new System.Drawing.Size(100, 34);
            this.btnArchive.Text = "Archive";
            this.btnArchive.Margin = new System.Windows.Forms.Padding(0);

            this.flowTermsActions.Controls.Add(this.btnView);
            this.flowTermsActions.Controls.Add(this.btnRefresh);
            this.flowTermsActions.Controls.Add(this.btnArchive);

            this.pnlTermsToolbar.Controls.Add(this.flowTermsActions);

            // gridTerms
            this.gridTerms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTerms.AllowUserToAddRows = false;
            this.gridTerms.AllowUserToDeleteRows = false;
            this.gridTerms.ReadOnly = true;
            this.gridTerms.RowHeadersVisible = false;
            this.gridTerms.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTerms.MultiSelect = false;
            this.gridTerms.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridTerms.BackgroundColor = System.Drawing.Color.White;
            this.gridTerms.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlTermsWrap.Controls.Add(this.gridTerms);
            this.pnlTermsWrap.Controls.Add(this.pnlTermsToolbar);

            // ============================================================
            // pnlNew — bottom half: create new version form
            // ============================================================
            this.pnlNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNew.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.pnlNew.Margin = new System.Windows.Forms.Padding(20, 0, 20, 20);

            this.lblNewTitle.AutoSize = true;
            this.lblNewTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNewTitle.Location = new System.Drawing.Point(20, 16);
            this.lblNewTitle.Text = "Create New Version";

            // tblForm — 2 columns: label (140px) + input (fills)
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.ColumnCount = 2;
            this.tblForm.RowCount = 3;
            this.tblForm.Padding = new System.Windows.Forms.Padding(0, 44, 0, 60);
            this.tblForm.Margin = new System.Windows.Forms.Padding(0);
            this.tblForm.BackColor = System.Drawing.Color.Transparent;

            this.tblForm.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tblForm.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblForm.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tblForm.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tblForm.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // lblVersion
            this.lblVersion.AutoSize = true;
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblVersion.Text = "New Version:";
            this.lblVersion.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            // txtVersion
            this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersion.PlaceholderText = "v1.1";
            this.txtVersion.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.txtVersion.MaxLength = 20;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitle.Text = "Title:";
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            // txtTitle
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.txtTitle.MaxLength = 200;

            // lblContent
            this.lblContent.AutoSize = true;
            this.lblContent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblContent.Text = "Content:";
            this.lblContent.Margin = new System.Windows.Forms.Padding(0, 12, 8, 0);

            // txtContent
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Multiline = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.txtContent.AcceptsReturn = true;
            this.txtContent.WordWrap = true;

            this.tblForm.Controls.Add(this.lblVersion, 0, 0);
            this.tblForm.Controls.Add(this.txtVersion, 1, 0);
            this.tblForm.Controls.Add(this.lblTitle, 0, 1);
            this.tblForm.Controls.Add(this.txtTitle, 1, 1);
            this.tblForm.Controls.Add(this.lblContent, 0, 2);
            this.tblForm.Controls.Add(this.txtContent, 1, 2);

            // pnlFormFooter — Active checkbox + Save button + status, docked to bottom
            this.pnlFormFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFormFooter.Height = 52;
            this.pnlFormFooter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);

            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(0, 16);
            this.chkActive.Text = "Is Active";

            this.btnSave.Location = new System.Drawing.Point(140, 8);
            this.btnSave.Size = new System.Drawing.Size(150, 34);
            this.btnSave.Text = "Add Version";

            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(310, 18);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.pnlFormFooter.Controls.Add(this.chkActive);
            this.pnlFormFooter.Controls.Add(this.btnSave);
            this.pnlFormFooter.Controls.Add(this.lblStatus);

            this.pnlNew.Controls.Add(this.tblForm);
            this.pnlNew.Controls.Add(this.lblNewTitle);
            this.pnlNew.Controls.Add(this.pnlFormFooter);

            // Bring the title above the table
            this.lblNewTitle.BringToFront();

            // ============================================================
            // FrmTermsEditor
            // ============================================================
            this.ClientSize = new System.Drawing.Size(960, 740);
            this.Controls.Add(this.pnlNew);
            this.Controls.Add(this.pnlTermsWrap);
            this.Controls.Add(this.pnlHeader);

            this.MinimumSize = new System.Drawing.Size(960, 740);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terms and Conditions";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlTermsWrap.ResumeLayout(false);
            this.pnlTermsToolbar.ResumeLayout(false);
            this.pnlTermsToolbar.PerformLayout();
            this.flowTermsActions.ResumeLayout(false);
            this.pnlNew.ResumeLayout(false);
            this.pnlNew.PerformLayout();
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            this.pnlFormFooter.ResumeLayout(false);
            this.pnlFormFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTerms)).EndInit();
            this.ResumeLayout(false);
        }
    }
}