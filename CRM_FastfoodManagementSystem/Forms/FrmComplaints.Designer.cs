namespace CRM.winForms.Forms
{
    partial class FrmComplaints
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tblFilters;
        private System.Windows.Forms.FlowLayoutPanel flowSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.FlowLayoutPanel flowCategory;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.FlowLayoutPanel flowStatus;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.CheckBox chkShowArchived;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnFileNew;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.DataGridView gridComplaints;

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
            this.tblFilters = new System.Windows.Forms.TableLayoutPanel();
            this.flowSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.flowCategory = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCategoryFilter = new System.Windows.Forms.Label();
            this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            this.flowStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.chkShowArchived = new System.Windows.Forms.CheckBox();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnFileNew = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlGridWrap = new System.Windows.Forms.Panel();
            this.gridComplaints = new System.Windows.Forms.DataGridView();

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
            this.tblFilters.SuspendLayout();
            this.flowSearch.SuspendLayout();
            this.flowCategory.SuspendLayout();
            this.flowStatus.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.pnlGridWrap.SuspendLayout();
            this.pnlPager.SuspendLayout();
            this.tblPager.SuspendLayout();
            this.flowPageSize.SuspendLayout();
            this.tblNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridComplaints)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlTop
            // ============================================================
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 96;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 8);

            // tblFilters — 4 columns × 2 rows
            //   Row 0: Search | Category | Status | (right: File Complaint)
            //   Row 1: Show archived
            this.tblFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblFilters.Height = 74;
            this.tblFilters.ColumnCount = 4;
            this.tblFilters.RowCount = 2;
            this.tblFilters.Padding = new System.Windows.Forms.Padding(0);
            this.tblFilters.Margin = new System.Windows.Forms.Padding(0);

            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblFilters.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblFilters.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblFilters.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));

            // ---- flowSearch: "Search:" + txtSearch ----
            this.flowSearch.AutoSize = true;
            this.flowSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowSearch.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowSearch.WrapContents = false;
            this.flowSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowSearch.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.flowSearch.Padding = new System.Windows.Forms.Padding(0);

            this.txtSearch.Width = 420;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);

            this.flowSearch.Controls.Add(this.txtSearch);

            // ---- flowCategory ----
            this.flowCategory.AutoSize = true;
            this.flowCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowCategory.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowCategory.WrapContents = false;
            this.flowCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowCategory.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.flowCategory.Padding = new System.Windows.Forms.Padding(0);

            this.lblCategoryFilter.AutoSize = true;
            this.lblCategoryFilter.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblCategoryFilter.Text = "Category:";
            this.lblCategoryFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.Width = 160;
            this.cmbCategoryFilter.Margin = new System.Windows.Forms.Padding(0);

            this.flowCategory.Controls.Add(this.lblCategoryFilter);
            this.flowCategory.Controls.Add(this.cmbCategoryFilter);

            // ---- flowStatus ----
            this.flowStatus.AutoSize = true;
            this.flowStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowStatus.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowStatus.WrapContents = false;
            this.flowStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowStatus.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.flowStatus.Padding = new System.Windows.Forms.Padding(0);

            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblStatusFilter.Text = "Status:";
            this.lblStatusFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Width = 150;
            this.cmbStatusFilter.Margin = new System.Windows.Forms.Padding(0);

            this.flowStatus.Controls.Add(this.lblStatusFilter);
            this.flowStatus.Controls.Add(this.cmbStatusFilter);

            // ---- flowButtons: File Complaint, right-aligned ----
            this.flowButtons.AutoSize = true;
            this.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowButtons.WrapContents = false;
            this.flowButtons.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowButtons.Padding = new System.Windows.Forms.Padding(0);

            this.btnFileNew.Width = 170;
            this.btnFileNew.Height = 32;
            this.btnFileNew.Margin = new System.Windows.Forms.Padding(0);
            this.btnFileNew.Text = "+ File Complaint";

            this.flowButtons.Controls.Add(this.btnFileNew);

            // ---- Row 1: chkShowArchived ----
            this.chkShowArchived.AutoSize = true;
            this.chkShowArchived.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.chkShowArchived.Text = "Show archived";
            this.chkShowArchived.UseVisualStyleBackColor = true;

            // Add to grid
            this.tblFilters.Controls.Add(this.flowSearch, 0, 0);
            this.tblFilters.Controls.Add(this.flowCategory, 1, 0);
            this.tblFilters.Controls.Add(this.flowStatus, 2, 0);
            this.tblFilters.Controls.Add(this.flowButtons, 3, 0);

            this.tblFilters.Controls.Add(this.chkShowArchived, 0, 1);
            this.tblFilters.SetColumnSpan(this.chkShowArchived, 3);

            this.pnlTop.Controls.Add(this.tblFilters);

            // ============================================================
            // pnlGridWrap
            // ============================================================
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

            this.gridComplaints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridComplaints.BackgroundColor = System.Drawing.Color.White;
            this.gridComplaints.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridComplaints.AllowUserToAddRows = false;
            this.gridComplaints.AllowUserToDeleteRows = false;
            this.gridComplaints.ReadOnly = true;
            this.gridComplaints.RowHeadersVisible = false;
            this.gridComplaints.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridComplaints.MultiSelect = false;
            this.gridComplaints.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.pnlGridWrap.Controls.Add(this.gridComplaints);

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

            this.tblPager.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPager.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPager.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));

            this.tblPager.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

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

            this.cmbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPageSize.Width = 90;
            this.cmbPageSize.Margin = new System.Windows.Forms.Padding(0);

            this.flowPageSize.Controls.Add(this.lblPageSize);
            this.flowPageSize.Controls.Add(this.cmbPageSize);

            this.tblNav.AutoSize = true;
            this.tblNav.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblNav.ColumnCount = 5;
            this.tblNav.RowCount = 1;
            this.tblNav.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tblNav.Margin = new System.Windows.Forms.Padding(0);

            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tblNav.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));

            this.tblNav.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));

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
            // FrmComplaints
            // ============================================================
            this.ClientSize = new System.Drawing.Size(1250, 720);
            this.MinimumSize = new System.Drawing.Size(1050, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Complaints";

            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.pnlPager);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.tblFilters.ResumeLayout(false);
            this.tblFilters.PerformLayout();
            this.flowSearch.ResumeLayout(false);
            this.flowSearch.PerformLayout();
            this.flowCategory.ResumeLayout(false);
            this.flowCategory.PerformLayout();
            this.flowStatus.ResumeLayout(false);
            this.flowStatus.PerformLayout();
            this.flowButtons.ResumeLayout(false);
            this.flowButtons.PerformLayout();
            this.pnlGridWrap.ResumeLayout(false);
            this.pnlPager.ResumeLayout(false);
            this.tblPager.ResumeLayout(false);
            this.tblPager.PerformLayout();
            this.flowPageSize.ResumeLayout(false);
            this.flowPageSize.PerformLayout();
            this.tblNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridComplaints)).EndInit();
            this.ResumeLayout(false);
        }
    }
}