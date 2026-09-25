namespace CRM.winForms.Forms
{
    partial class FrmUserManagement
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.CheckBox chkShowArchived;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView gridUsers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.chkShowArchived = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gridUsers = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).BeginInit();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 65;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);
            this.pnlTop.BackColor = System.Drawing.Color.White;

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(16, 20);
            this.txtSearch.Size = new System.Drawing.Size(320, 25);
            this.txtSearch.PlaceholderText = "Search by username or name...";

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(346, 18);
            this.btnRefresh.Size = new System.Drawing.Size(100, 32);
            this.btnRefresh.Text = "Refresh";

            // btnAddUser
            this.btnAddUser.Location = new System.Drawing.Point(456, 18);
            this.btnAddUser.Size = new System.Drawing.Size(140, 32);
            this.btnAddUser.Text = "Add User";

            // chkShowArchived
            this.chkShowArchived.Location = new System.Drawing.Point(610, 22);
            this.chkShowArchived.Size = new System.Drawing.Size(160, 25);
            this.chkShowArchived.Text = "Show archived";
            this.chkShowArchived.UseVisualStyleBackColor = true;

            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.btnAddUser);
            this.pnlTop.Controls.Add(this.chkShowArchived);

            // lblStatus
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 32;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // gridUsers
            this.gridUsers.Dock = System.Windows.Forms.DockStyle.Fill;

            // wrap
            var pnlGridWrap = new System.Windows.Forms.Panel();
            pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGridWrap.Padding = new System.Windows.Forms.Padding(16);
            pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            pnlGridWrap.Controls.Add(this.gridUsers);

            // FrmUserManagement
            this.ClientSize = new System.Drawing.Size(1100, 620);
            this.Controls.Add(pnlGridWrap);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Management";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUsers)).EndInit();
            this.ResumeLayout(false);
        }
    }
}