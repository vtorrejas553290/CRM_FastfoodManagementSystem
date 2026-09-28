namespace CRM.winForms.Forms
{
    partial class FrmBranchDetail
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblBranchName;
        private System.Windows.Forms.Label lblBranchCode;
        private System.Windows.Forms.Label lblBranchMeta;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblManagersTitle;
        private System.Windows.Forms.DataGridView gridManagers;
        private System.Windows.Forms.Label lblStaffTitle;
        private System.Windows.Forms.DataGridView gridStaff;

        private System.Windows.Forms.Panel pnlFooter;
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
            this.lblBranchName = new System.Windows.Forms.Label();
            this.lblBranchCode = new System.Windows.Forms.Label();
            this.lblBranchMeta = new System.Windows.Forms.Label();

            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblManagersTitle = new System.Windows.Forms.Label();
            this.gridManagers = new System.Windows.Forms.DataGridView();
            this.lblStaffTitle = new System.Windows.Forms.Label();
            this.gridStaff = new System.Windows.Forms.DataGridView();

            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridManagers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStaff)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlHeader
            // ============================================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(51, 117, 160);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 110;
            this.pnlHeader.Controls.Add(this.lblBranchMeta);
            this.pnlHeader.Controls.Add(this.lblBranchCode);
            this.pnlHeader.Controls.Add(this.lblBranchName);

            // lblBranchName
            this.lblBranchName.AutoSize = false;
            this.lblBranchName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBranchName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBranchName.ForeColor = System.Drawing.Color.White;
            this.lblBranchName.Height = 46;
            this.lblBranchName.Padding = new System.Windows.Forms.Padding(22, 14, 0, 0);
            this.lblBranchName.Text = "Branch Name";

            // lblBranchCode
            this.lblBranchCode.AutoSize = false;
            this.lblBranchCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBranchCode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBranchCode.ForeColor = System.Drawing.Color.FromArgb(204, 223, 232);
            this.lblBranchCode.Height = 26;
            this.lblBranchCode.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.lblBranchCode.Text = "Code: —";

            // lblBranchMeta
            this.lblBranchMeta.AutoSize = false;
            this.lblBranchMeta.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBranchMeta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblBranchMeta.ForeColor = System.Drawing.Color.FromArgb(204, 223, 232);
            this.lblBranchMeta.Height = 34;
            this.lblBranchMeta.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.lblBranchMeta.Text = "—";

            // ============================================================
            // pnlBody
            // ============================================================
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.pnlBody.Padding = new System.Windows.Forms.Padding(16);

            // lblManagersTitle
            this.lblManagersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblManagersTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblManagersTitle.ForeColor = System.Drawing.Color.FromArgb(0, 41, 68);
            this.lblManagersTitle.Height = 28;
            this.lblManagersTitle.Text = "Managers";
            this.lblManagersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // gridManagers
            this.gridManagers.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridManagers.Height = 140;
            this.gridManagers.AllowUserToAddRows = false;
            this.gridManagers.AllowUserToDeleteRows = false;
            this.gridManagers.ReadOnly = true;
            this.gridManagers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridManagers.MultiSelect = false;
            this.gridManagers.RowHeadersVisible = false;
            this.gridManagers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // lblStaffTitle
            this.lblStaffTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStaffTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblStaffTitle.ForeColor = System.Drawing.Color.FromArgb(0, 41, 68);
            this.lblStaffTitle.Height = 28;
            this.lblStaffTitle.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.lblStaffTitle.Text = "Staff";
            this.lblStaffTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // gridStaff
            this.gridStaff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStaff.AllowUserToAddRows = false;
            this.gridStaff.AllowUserToDeleteRows = false;
            this.gridStaff.ReadOnly = true;
            this.gridStaff.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStaff.MultiSelect = false;
            this.gridStaff.RowHeadersVisible = false;
            this.gridStaff.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // Note the order of Add() matters for Dock=Top stacking
            this.pnlBody.Controls.Add(this.gridStaff);
            this.pnlBody.Controls.Add(this.lblStaffTitle);
            this.pnlBody.Controls.Add(this.gridManagers);
            this.pnlBody.Controls.Add(this.lblManagersTitle);

            // ============================================================
            // pnlFooter
            // ============================================================
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 60;
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);

            this.btnClose.Text = "Close";
            this.btnClose.Width = 140;
            this.btnClose.Height = 38;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;

            this.pnlFooter.Controls.Add(this.btnClose);

            // ============================================================
            // FrmBranchDetail
            // ============================================================
            this.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.ClientSize = new System.Drawing.Size(720, 620);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Branch Detail";

            this.pnlHeader.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridManagers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridStaff)).EndInit();
            this.ResumeLayout(false);
        }
    }
}