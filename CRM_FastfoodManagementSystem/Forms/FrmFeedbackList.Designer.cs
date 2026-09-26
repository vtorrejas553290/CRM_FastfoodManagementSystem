namespace CRM.winForms.Forms
{
    partial class FrmFeedbackList
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Label lblFilterStars;
        private System.Windows.Forms.ComboBox cmbFilterStars;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gridFeedback;
        private System.Windows.Forms.Label lblTotal;

        // Header layout
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.FlowLayoutPanel flowSearch;
        private System.Windows.Forms.FlowLayoutPanel flowStatus;
        private System.Windows.Forms.FlowLayoutPanel flowStars;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;

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
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterStars = new System.Windows.Forms.Label();
            this.cmbFilterStars = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gridFeedback = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();

            this.tblHeader = new System.Windows.Forms.TableLayoutPanel();
            this.flowSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.flowStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.flowStars = new System.Windows.Forms.FlowLayoutPanel();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();

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
            this.tblHeader.SuspendLayout();
            this.flowSearch.SuspendLayout();
            this.flowStatus.SuspendLayout();
            this.flowStars.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.pnlPager.SuspendLayout();
            this.tblPager.SuspendLayout();
            this.flowPageSize.SuspendLayout();
            this.tblNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFeedback)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlTop
            // ============================================================
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 96;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 8);

            // tblHeader — 4 columns:
            //   col 0: Search group        (AutoSize)
            //   col 1: Status group        (AutoSize)
            //   col 2: Stars group         (AutoSize)
            //   col 3: Add button, right   (Percent 100, button anchored right)
            this.tblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblHeader.Height = 40;
            this.tblHeader.ColumnCount = 4;
            this.tblHeader.RowCount = 1;
            this.tblHeader.Padding = new System.Windows.Forms.Padding(0);
            this.tblHeader.Margin = new System.Windows.Forms.Padding(0);

            this.tblHeader.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblHeader.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblHeader.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblHeader.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.tblHeader.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));

            // ---- flowSearch: "Search:" + txtSearch (widened) ----
            this.flowSearch.AutoSize = true;
            this.flowSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowSearch.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowSearch.WrapContents = false;
            this.flowSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowSearch.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.flowSearch.Padding = new System.Windows.Forms.Padding(0);

            this.lblSearch.AutoSize = true;
            this.lblSearch.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblSearch.Text = "Search:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.txtSearch.Width = 420;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0);

            this.flowSearch.Controls.Add(this.lblSearch);
            this.flowSearch.Controls.Add(this.txtSearch);

            // ---- flowStatus ----
            this.flowStatus.AutoSize = true;
            this.flowStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowStatus.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowStatus.WrapContents = false;
            this.flowStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowStatus.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.flowStatus.Padding = new System.Windows.Forms.Padding(0);

            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblFilterStatus.Text = "Status:";
            this.lblFilterStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.Width = 130;
            this.cmbFilterStatus.Margin = new System.Windows.Forms.Padding(0);

            this.flowStatus.Controls.Add(this.lblFilterStatus);
            this.flowStatus.Controls.Add(this.cmbFilterStatus);

            // ---- flowStars ----
            this.flowStars.AutoSize = true;
            this.flowStars.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowStars.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowStars.WrapContents = false;
            this.flowStars.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowStars.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.flowStars.Padding = new System.Windows.Forms.Padding(0);

            this.lblFilterStars.AutoSize = true;
            this.lblFilterStars.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            this.lblFilterStars.Text = "Stars:";
            this.lblFilterStars.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.cmbFilterStars.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStars.Width = 120;
            this.cmbFilterStars.Margin = new System.Windows.Forms.Padding(0);

            this.flowStars.Controls.Add(this.lblFilterStars);
            this.flowStars.Controls.Add(this.cmbFilterStars);

            // ---- flowButtons: Add Feedback, right-aligned ----
            this.flowButtons.AutoSize = true;
            this.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowButtons.WrapContents = false;
            this.flowButtons.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flowButtons.Padding = new System.Windows.Forms.Padding(0);

            this.btnAdd.Width = 150;
            this.btnAdd.Height = 32;
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd.Text = "Add Feedback";

            this.flowButtons.Controls.Add(this.btnAdd);

            this.tblHeader.Controls.Add(this.flowSearch, 0, 0);
            this.tblHeader.Controls.Add(this.flowStatus, 1, 0);
            this.tblHeader.Controls.Add(this.flowStars, 2, 0);
            this.tblHeader.Controls.Add(this.flowButtons, 3, 0);

            // lblTotal
            this.lblTotal.AutoSize = false;
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotal.Height = 28;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTotal.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblTotal.Text = "Total Entries: 0";

            this.pnlTop.Controls.Add(this.lblTotal);
            this.pnlTop.Controls.Add(this.tblHeader);

            // ============================================================
            // pnlGridWrap
            // ============================================================
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.pnlGridWrap.Controls.Add(this.gridFeedback);

            this.gridFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFeedback.BackgroundColor = System.Drawing.Color.White;
            this.gridFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridFeedback.AllowUserToAddRows = false;
            this.gridFeedback.AllowUserToDeleteRows = false;
            this.gridFeedback.ReadOnly = true;
            this.gridFeedback.RowHeadersVisible = false;
            this.gridFeedback.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFeedback.MultiSelect = false;
            this.gridFeedback.AutoSizeColumnsMode =
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
            // FrmFeedbackList
            // ============================================================
            this.ClientSize = new System.Drawing.Size(1150, 720);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Feedback";
            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.pnlPager);
            this.Controls.Add(this.pnlTop);

            this.pnlTop.ResumeLayout(false);
            this.tblHeader.ResumeLayout(false);
            this.tblHeader.PerformLayout();
            this.flowSearch.ResumeLayout(false);
            this.flowSearch.PerformLayout();
            this.flowStatus.ResumeLayout(false);
            this.flowStatus.PerformLayout();
            this.flowStars.ResumeLayout(false);
            this.flowStars.PerformLayout();
            this.flowButtons.ResumeLayout(false);
            this.flowButtons.PerformLayout();
            this.pnlGridWrap.ResumeLayout(false);
            this.pnlPager.ResumeLayout(false);
            this.tblPager.ResumeLayout(false);
            this.tblPager.PerformLayout();
            this.flowPageSize.ResumeLayout(false);
            this.flowPageSize.PerformLayout();
            this.tblNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFeedback)).EndInit();
            this.ResumeLayout(false);
        }
    }
}