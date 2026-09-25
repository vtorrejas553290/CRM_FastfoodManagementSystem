namespace CRM.winForms.Forms
{
    partial class FrmCustomerList
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnRegisterCustomer;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkShowArchived;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView gridCustomers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnRegisterCustomer = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkShowArchived = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gridCustomers = new System.Windows.Forms.DataGridView();

            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCustomers)).BeginInit();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 68;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);

            // btnRegisterCustomer (FIRST on the left)
            this.btnRegisterCustomer.Location = new System.Drawing.Point(16, 18);
            this.btnRegisterCustomer.Size = new System.Drawing.Size(180, 31);
            this.btnRegisterCustomer.Text = "+  Register Customer";
            this.btnRegisterCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegisterCustomer.FlatAppearance.BorderSize = 0;

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(210, 20);
            this.txtSearch.Size = new System.Drawing.Size(320, 27);
            this.txtSearch.PlaceholderText = "Search by code or name...";

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(540, 18);
            this.btnRefresh.Size = new System.Drawing.Size(100, 31);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // chkShowArchived
            this.chkShowArchived.Location = new System.Drawing.Point(650, 22);
            this.chkShowArchived.Size = new System.Drawing.Size(160, 25);
            this.chkShowArchived.Text = "Show archived";
            this.chkShowArchived.UseVisualStyleBackColor = true;

            this.pnlTop.Controls.Add(this.btnRegisterCustomer);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.chkShowArchived);

            // lblStatus
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 30;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            // gridCustomers
            this.gridCustomers.Dock = System.Windows.Forms.DockStyle.Fill;

            var pnlGridWrap = new System.Windows.Forms.Panel();
            pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGridWrap.Padding = new System.Windows.Forms.Padding(16);
            pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            pnlGridWrap.Controls.Add(this.gridCustomers);

            // FrmCustomerList
            this.ClientSize = new System.Drawing.Size(1100, 620);
            this.Controls.Add(pnlGridWrap);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer List";

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCustomers)).EndInit();
            this.ResumeLayout(false);
        }
    }
}