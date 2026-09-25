namespace CRM.winForms.Forms
{
    partial class FrmCustomerRetention
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblPointsInfo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView gridCustomers;

        // Top layout
        private System.Windows.Forms.TableLayoutPanel tblFilters;
        private System.Windows.Forms.FlowLayoutPanel flowStatus;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cmbStatusFilter;

        // Pagination bar
        private System.Windows.Forms.Panel pnlPager;
        private System.Windows.Forms.TableLayoutPanel tblPager;
        private System.Windows.Forms.FlowLayoutPanel flowPageSize;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.TableLayoutPanel tblNav;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Label lblShowing;

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
            this.lblPointsInfo = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gridCustomers = new System.Windows.Forms.DataGridView();

            this.tblFilters = new System.Windows.Forms.TableLayoutPanel();
            this.flowStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();

            this.pnlPager = new System.Windows.Forms.Panel();
            this.tblPager = new System.Windows.Forms.TableLayoutPanel();
            this.flowPageSize = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.cmbPageSize = new System.Windows.Forms.ComboBox();
            this.tblNav = new System.Windows.Forms.TableLayoutPanel();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.lblShowing = new System.Windows.Forms.Label();

            this.pnlTop.SuspendLayout();
            this.pnlGridWrap.SuspendLayout();
            this.tblFilters.SuspendLayout();
            this.flowStatus.SuspendLayout();
            this.pnlPager.SuspendLayout();
            this.tblPager.SuspendLayout();
            this.flowPageSize.SuspendLayout();
            this.tblNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCustomers)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlTop — header with filter row + info line
            // ============================================================
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 100;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 8);

            // tblFilters: search | status filter | refresh
            this.tblFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblFilters.Height = 40;
            this.tblFilters.ColumnCount = 3;
            this.tblFilters.RowCount = 1;
            this.tblFilters.Padding = new System.Windows.Forms.Padding(0);
            this.tblFilters.Margin = new System.Windows.Forms.Padding(0);

            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));

            this.tblFilters.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));

            // txtSearch
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.txtSearch.PlaceholderText = "Search by customer code or name...";

            // flowStatus: "Status:" + combo
            this.flowStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowStatus.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowStatus.WrapContents = false;
            this.flowStatus.AutoSize = false;
            this.flowStatus.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.flowStatus.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);

            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblStatusFilter.Text = "Status:";
            this.lblStatusFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Width = 150;

            this.flowStatus.Controls.Add(this.lblStatusFilter);
            this.flowStatus.Controls.Add(this.cmbStatusFilter);

            // btnRefresh
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Text = "Refresh";

            this.tblFilters.Controls.Add(this.txtSearch, 0, 0);
            this.tblFilters.Controls.Add(this.flowStatus, 1, 0);
            this.tblFilters.Controls.Add(this.btnRefresh, 2, 0);

            // lblPointsInfo (below the filter row)
            this.lblPointsInfo.AutoSize = false;
            this.lblPointsInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPointsInfo.Height = 28;
            this.lblPointsInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            this.lblPointsInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPointsInfo.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblPointsInfo.Text = "Loyalty: ₱1 spent = 1 point earned. 100 points = ₱1 discount.";

            this.pnlTop.Controls.Add(this.lblPointsInfo);
            this.pnlTop.Controls.Add(this.tblFilters);

            // ============================================================
            // pnlGridWrap
            // ============================================================
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

            this.gridCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCustomers.BackgroundColor = System.Drawing.Color.White;
            this.gridCustomers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridCustomers.AllowUserToAddRows = false;
            this.gridCustomers.AllowUserToDeleteRows = false;
            this.gridCustomers.ReadOnly = true;
            this.gridCustomers.RowHeadersVisible = false;
            this.gridCustomers.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCustomers.MultiSelect = false;
            this.gridCustomers.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.pnlGridWrap.Controls.Add(this.gridCustomers);

            // ============================================================
            // pnlPager — pagination bar
            // ============================================================
            this.pnlPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPager.Height = 52;
            this.pnlPager.BackColor = System.Drawing.Color.White;
            this.pnlPager.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);

            this.tblPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPager.ColumnCount = 3;
            this.tblPager.RowCount = 1;
            this.tblPager.Padding = new System.Windows.Forms.Padding(0);
            this.tblPager.Margin = new System.Windows.Forms.Padding(0);

            this.tblPager.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPager.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPager.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));

            this.tblPager.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // flowPageSize
            this.flowPageSize.AutoSize = true;
            this.flowPageSize.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPageSize.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowPageSize.WrapContents = false;
            this.flowPageSize.Margin = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.flowPageSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowPageSize.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);

            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblPageSize.Text = "Rows per page:";
            this.lblPageSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPageSize.Width = 90;
            this.cmbPageSize.Margin = new System.Windows.Forms.Padding(0);

            this.flowPageSize.Controls.Add(this.lblPageSize);
            this.flowPageSize.Controls.Add(this.cmbPageSize);

            // tblNav
            this.tblNav.AutoSize = true;
            this.tblNav.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblNav.ColumnCount = 5;
            this.tblNav.RowCount = 1;
            this.tblNav.Margin = new System.Windows.Forms.Padding(0);
            this.tblNav.Padding = new System.Windows.Forms.Padding(0);
            this.tblNav.Anchor = System.Windows.Forms.AnchorStyles.Left;

            this.tblNav.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblNav.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblNav.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tblNav.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblNav.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));

            this.tblNav.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));

            // btnFirstPage
            this.btnFirstPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnFirstPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnFirstPage.Text = "«";
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // btnPrevPage
            this.btnPrevPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnPrevPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPrevPage.Text = "‹";
            this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // lblPageInfo
            this.lblPageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPageInfo.Text = "Page 1 of 1";

            // btnNextPage
            this.btnNextPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnNextPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnNextPage.Text = "›";
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // btnLastPage
            this.btnLastPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLastPage.Margin = new System.Windows.Forms.Padding(0);
            this.btnLastPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLastPage.Text = "»";
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.tblNav.Controls.Add(this.btnFirstPage, 0, 0);
            this.tblNav.Controls.Add(this.btnPrevPage, 1, 0);
            this.tblNav.Controls.Add(this.lblPageInfo, 2, 0);
            this.tblNav.Controls.Add(this.btnNextPage, 3, 0);
            this.tblNav.Controls.Add(this.btnLastPage, 4, 0);

            // lblShowing
            this.lblShowing.AutoSize = true;
            this.lblShowing.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblShowing.Margin = new System.Windows.Forms.Padding(0);
            this.lblShowing.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblShowing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblShowing.Text = "Showing 0–0 of 0";

            this.tblPager.Controls.Add(this.flowPageSize, 0, 0);
            this.tblPager.Controls.Add(this.tblNav, 1, 0);
            this.tblPager.Controls.Add(this.lblShowing, 2, 0);

            this.pnlPager.Controls.Add(this.tblPager);

            // ============================================================
            // lblStatus
            // ============================================================
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 32;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ============================================================
            // FrmCustomerRetention
            // ============================================================
            this.ClientSize = new System.Drawing.Size(1150, 720);
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Retention — Loyalty Points";
            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.pnlPager);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.tblFilters.ResumeLayout(false);
            this.flowStatus.ResumeLayout(false);
            this.flowStatus.PerformLayout();
            this.pnlGridWrap.ResumeLayout(false);
            this.pnlPager.ResumeLayout(false);
            this.tblPager.ResumeLayout(false);
            this.tblPager.PerformLayout();
            this.flowPageSize.ResumeLayout(false);
            this.flowPageSize.PerformLayout();
            this.tblNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCustomers)).EndInit();
            this.ResumeLayout(false);
        }
    }
}