namespace CRM.winForms.Forms
{
    partial class FrmReports
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label lblDateRange;
        private System.Windows.Forms.ComboBox cmbDateRange;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.DataGridView gridReport;
        private System.Windows.Forms.Label lblStatus;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.cmbDateRange = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.gridReport = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();

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

            this.pnlHeader.SuspendLayout();
            this.pnlPager.SuspendLayout();
            this.tblPager.SuspendLayout();
            this.flowPageSize.SuspendLayout();
            this.tblNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlHeader — unchanged
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 96;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(24, 18);
            this.lblTitle.Text = "Reports";

            this.lblReportType.AutoSize = true;
            this.lblReportType.Location = new System.Drawing.Point(26, 54);
            this.lblReportType.Text = "REPORT TYPE";

            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReportType.Location = new System.Drawing.Point(130, 50);
            this.cmbReportType.Size = new System.Drawing.Size(200, 29);

            this.lblDateRange.AutoSize = true;
            this.lblDateRange.Location = new System.Drawing.Point(350, 54);
            this.lblDateRange.Text = "DATE RANGE";

            this.cmbDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDateRange.Location = new System.Drawing.Point(450, 50);
            this.cmbDateRange.Size = new System.Drawing.Size(160, 29);

            this.btnGenerate.Location = new System.Drawing.Point(630, 48);
            this.btnGenerate.Size = new System.Drawing.Size(120, 32);
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnExportPdf.Location = new System.Drawing.Point(760, 48);
            this.btnExportPdf.Size = new System.Drawing.Size(140, 32);
            this.btnExportPdf.Text = "Export PDF";
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblReportType);
            this.pnlHeader.Controls.Add(this.cmbReportType);
            this.pnlHeader.Controls.Add(this.lblDateRange);
            this.pnlHeader.Controls.Add(this.cmbDateRange);
            this.pnlHeader.Controls.Add(this.btnGenerate);
            this.pnlHeader.Controls.Add(this.btnExportPdf);

            // ============================================================
            // gridReport — fills the middle
            // ============================================================
            this.gridReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReport.AllowUserToAddRows = false;
            this.gridReport.AllowUserToDeleteRows = false;
            this.gridReport.ReadOnly = true;
            this.gridReport.RowHeadersVisible = false;
            this.gridReport.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReport.MultiSelect = false;
            this.gridReport.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReport.BackgroundColor = System.Drawing.Color.White;
            this.gridReport.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // ============================================================
            // pnlPager — pagination bar (redesigned, matches FrmActivityLogs)
            // ============================================================
            this.pnlPager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPager.Height = 52;
            this.pnlPager.BackColor = System.Drawing.Color.White;
            this.pnlPager.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);

            // tblPager: [page size group] [nav cluster] [showing label (right)]
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

            // ---- flowPageSize: "Rows per page:" + combo ----
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

            // ---- tblNav: [ « ] [ ‹ ] [ Page X of Y ] [ › ] [ » ] ----
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

            // btnFirstPage «
            this.btnFirstPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnFirstPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnFirstPage.Text = "«";
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // btnPrevPage ‹
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

            // btnNextPage ›
            this.btnNextPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnNextPage.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnNextPage.Text = "›";
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // btnLastPage »
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

            // ---- lblShowing ----
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
            this.lblStatus.Height = 28;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ============================================================
            // FrmReports
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 760);
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reports";
            this.Controls.Add(this.gridReport);
            this.Controls.Add(this.pnlPager);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlPager.ResumeLayout(false);
            this.tblPager.ResumeLayout(false);
            this.tblPager.PerformLayout();
            this.flowPageSize.ResumeLayout(false);
            this.flowPageSize.PerformLayout();
            this.tblNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).EndInit();
            this.ResumeLayout(false);
        }
    }
}