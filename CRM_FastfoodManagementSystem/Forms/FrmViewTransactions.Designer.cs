namespace CRM.winForms.Forms
{
    partial class FrmViewTransactions
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblMethodFilter;
        private System.Windows.Forms.ComboBox cmbMethodFilter;
        private System.Windows.Forms.Label lblDateFilter;
        private System.Windows.Forms.ComboBox cmbDateFilter;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridView gridTransactions;

        // Layout containers (top)
        private System.Windows.Forms.TableLayoutPanel tblFilters;
        private System.Windows.Forms.FlowLayoutPanel flowMethod;
        private System.Windows.Forms.FlowLayoutPanel flowDate;

        // Pagination bar (bottom)
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
            this.lblMethodFilter = new System.Windows.Forms.Label();
            this.cmbMethodFilter = new System.Windows.Forms.ComboBox();
            this.lblDateFilter = new System.Windows.Forms.Label();
            this.cmbDateFilter = new System.Windows.Forms.ComboBox();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gridTransactions = new System.Windows.Forms.DataGridView();

            this.tblFilters = new System.Windows.Forms.TableLayoutPanel();
            this.flowMethod = new System.Windows.Forms.FlowLayoutPanel();
            this.flowDate = new System.Windows.Forms.FlowLayoutPanel();

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
            this.flowMethod.SuspendLayout();
            this.flowDate.SuspendLayout();
            this.pnlPager.SuspendLayout();
            this.tblPager.SuspendLayout();
            this.flowPageSize.SuspendLayout();
            this.tblNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactions)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlTop
            // ============================================================
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 96;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 8);

            // tblFilters — search | method | date group
            this.tblFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblFilters.Height = 40;
            this.tblFilters.ColumnCount = 3;
            this.tblFilters.RowCount = 1;
            this.tblFilters.Padding = new System.Windows.Forms.Padding(0);
            this.tblFilters.Margin = new System.Windows.Forms.Padding(0);

            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 560F));

            this.tblFilters.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));

            // txtSearch
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.txtSearch.PlaceholderText = "Search by order code or customer...";

            // flowMethod
            this.flowMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMethod.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowMethod.WrapContents = false;
            this.flowMethod.AutoSize = false;
            this.flowMethod.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.flowMethod.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);

            this.lblMethodFilter.AutoSize = true;
            this.lblMethodFilter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblMethodFilter.Text = "Method:";
            this.lblMethodFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbMethodFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethodFilter.Width = 140;

            this.flowMethod.Controls.Add(this.lblMethodFilter);
            this.flowMethod.Controls.Add(this.cmbMethodFilter);

            // flowDate
            this.flowDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowDate.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowDate.WrapContents = false;
            this.flowDate.AutoSize = false;
            this.flowDate.Margin = new System.Windows.Forms.Padding(0);
            this.flowDate.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);

            this.lblDateFilter.AutoSize = true;
            this.lblDateFilter.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblDateFilter.Text = "Date:";
            this.lblDateFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbDateFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateFilter.Width = 130;

            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFromDate.Text = "From:";
            this.lblFromDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Width = 110;
            this.dtpFromDate.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);

            this.lblToDate.AutoSize = true;
            this.lblToDate.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.lblToDate.Text = "To:";
            this.lblToDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Width = 110;
            this.dtpToDate.Margin = new System.Windows.Forms.Padding(0);

            this.flowDate.Controls.Add(this.lblDateFilter);
            this.flowDate.Controls.Add(this.cmbDateFilter);
            this.flowDate.Controls.Add(this.lblFromDate);
            this.flowDate.Controls.Add(this.dtpFromDate);
            this.flowDate.Controls.Add(this.lblToDate);
            this.flowDate.Controls.Add(this.dtpToDate);

            this.tblFilters.Controls.Add(this.txtSearch, 0, 0);
            this.tblFilters.Controls.Add(this.flowMethod, 1, 0);
            this.tblFilters.Controls.Add(this.flowDate, 2, 0);

            // lblTotal
            this.lblTotal.AutoSize = false;
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotal.Height = 28;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotal.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblTotal.Text = "Total Sales: ₱0.00";

            this.pnlTop.Controls.Add(this.lblTotal);
            this.pnlTop.Controls.Add(this.tblFilters);

            // ============================================================
            // pnlGridWrap
            // ============================================================
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.pnlGridWrap.Controls.Add(this.gridTransactions);

            this.gridTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTransactions.BackgroundColor = System.Drawing.Color.White;
            this.gridTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridTransactions.AllowUserToAddRows = false;
            this.gridTransactions.AllowUserToDeleteRows = false;
            this.gridTransactions.ReadOnly = true;
            this.gridTransactions.RowHeadersVisible = false;
            this.gridTransactions.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransactions.MultiSelect = false;
            this.gridTransactions.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // ============================================================
            // pnlPager
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

            this.btnFirstPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnFirstPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnFirstPage.Text = "«";
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnPrevPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnPrevPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPrevPage.Text = "‹";
            this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.lblPageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPageInfo.Text = "Page 1 of 1";

            this.btnNextPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnNextPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnNextPage.Text = "›";
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

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
            // FrmViewTransactions
            // ============================================================
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Transactions";
            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.pnlPager);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.tblFilters.ResumeLayout(false);
            this.flowMethod.ResumeLayout(false);
            this.flowMethod.PerformLayout();
            this.flowDate.ResumeLayout(false);
            this.flowDate.PerformLayout();
            this.pnlGridWrap.ResumeLayout(false);
            this.pnlPager.ResumeLayout(false);
            this.tblPager.ResumeLayout(false);
            this.tblPager.PerformLayout();
            this.flowPageSize.ResumeLayout(false);
            this.flowPageSize.PerformLayout();
            this.tblNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransactions)).EndInit();
            this.ResumeLayout(false);
        }
    }
}