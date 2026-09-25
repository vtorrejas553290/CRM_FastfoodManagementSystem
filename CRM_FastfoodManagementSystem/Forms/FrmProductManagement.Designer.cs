namespace CRM.winForms.Forms
{
    partial class FrmProductManagement
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.CheckBox chkShowArchived;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView gridProducts;

        // Layout
        private System.Windows.Forms.TableLayoutPanel tblTop;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlGridWrap = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.chkShowArchived = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gridProducts = new System.Windows.Forms.DataGridView();
            this.tblTop = new System.Windows.Forms.TableLayoutPanel();

            this.pnlTop.SuspendLayout();
            this.pnlGridWrap.SuspendLayout();
            this.tblTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProducts)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlTop
            // ============================================================
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 65;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);

            // tblTop — search | refresh | add | show archived toggle
            this.tblTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTop.ColumnCount = 4;
            this.tblTop.RowCount = 1;
            this.tblTop.Padding = new System.Windows.Forms.Padding(0);
            this.tblTop.Margin = new System.Windows.Forms.Padding(0);

            this.tblTop.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTop.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tblTop.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblTop.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));

            this.tblTop.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));

            // txtSearch
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.txtSearch.PlaceholderText = "Search by code or name...";

            // btnRefresh
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnRefresh.Text = "Refresh";

            // btnAdd
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnAdd.Text = "Add Product";

            // chkShowArchived
            this.chkShowArchived.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShowArchived.Margin = new System.Windows.Forms.Padding(0);
            this.chkShowArchived.Text = "Show Archived";
            this.chkShowArchived.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkShowArchived.UseVisualStyleBackColor = true;

            this.tblTop.Controls.Add(this.txtSearch, 0, 0);
            this.tblTop.Controls.Add(this.btnRefresh, 1, 0);
            this.tblTop.Controls.Add(this.btnAdd, 2, 0);
            this.tblTop.Controls.Add(this.chkShowArchived, 3, 0);

            this.pnlTop.Controls.Add(this.tblTop);

            // ============================================================
            // pnlGridWrap
            // ============================================================
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

            // gridProducts
            this.gridProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProducts.AllowUserToAddRows = false;
            this.gridProducts.AllowUserToDeleteRows = false;
            this.gridProducts.ReadOnly = true;
            this.gridProducts.RowHeadersVisible = false;
            this.gridProducts.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridProducts.MultiSelect = false;
            this.gridProducts.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridProducts.BackgroundColor = System.Drawing.Color.White;
            this.gridProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlGridWrap.Controls.Add(this.gridProducts);

            // ============================================================
            // lblStatus
            // ============================================================
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 32;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ============================================================
            // FrmProductManagement
            // ============================================================
            this.ClientSize = new System.Drawing.Size(1100, 620);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Management";
            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.tblTop.ResumeLayout(false);
            this.pnlGridWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}