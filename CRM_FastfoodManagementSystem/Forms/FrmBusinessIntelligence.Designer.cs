namespace CRM.winForms.Forms
{
    partial class FrmBusinessIntelligence
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cmbDateRange;
        private System.Windows.Forms.Label lblDateRange;

        // Body
        private System.Windows.Forms.Panel pnlBody;

        // KPI grids
        private System.Windows.Forms.TableLayoutPanel tblKpiRow1;
        private System.Windows.Forms.TableLayoutPanel tblKpiRow2;
        private System.Windows.Forms.TableLayoutPanel tblKpiRow3;   // NEW
        private System.Windows.Forms.Panel cardRevenue;
        private System.Windows.Forms.Panel cardTodaySales;
        private System.Windows.Forms.Panel cardOrders;
        private System.Windows.Forms.Panel cardAvgOrder;
        private System.Windows.Forms.Panel cardCustomers;
        private System.Windows.Forms.Panel cardPoints;
        private System.Windows.Forms.Panel cardLowStock;
        private System.Windows.Forms.Panel cardOpenFeedback;
        private System.Windows.Forms.Panel cardAvgRating;
        private System.Windows.Forms.Panel cardPromos;

        // NEW: Retention KPI cards
        private System.Windows.Forms.Panel cardAtRisk;
        private System.Windows.Forms.Panel cardDormant;
        private System.Windows.Forms.Panel cardNeverOrdered;
        private System.Windows.Forms.Panel cardPointsLiability;

        // Insights
        private System.Windows.Forms.Panel pnlInsights;
        private System.Windows.Forms.Label lblInsightsTitle;
        private System.Windows.Forms.FlowLayoutPanel flowInsights;

        // Charts
        private System.Windows.Forms.TableLayoutPanel tblChartsRow1;
        private System.Windows.Forms.TableLayoutPanel tblChartsRow2;
        private System.Windows.Forms.TableLayoutPanel tblChartsRow3;   // NEW
        private System.Windows.Forms.Panel pnlSalesChart;
        private System.Windows.Forms.Panel pnlTopProductsChart;
        private System.Windows.Forms.Panel pnlRatingChart;
        private System.Windows.Forms.Panel pnlPaymentChart;
        private System.Windows.Forms.Panel pnlRetentionSplitChart;      // NEW
        private System.Windows.Forms.Panel pnlRetentionRecencyChart;   // NEW

        // Tables
        private System.Windows.Forms.TableLayoutPanel tblTables;
        private System.Windows.Forms.TableLayoutPanel tblRetentionRow;  // NEW
        private System.Windows.Forms.Panel pnlRecentOrders;
        private System.Windows.Forms.Panel pnlTopCustomers;
        private System.Windows.Forms.Panel pnlAtRiskList;               // NEW
        private System.Windows.Forms.Label lblRecentOrders;
        private System.Windows.Forms.Label lblTopCustomers;
        private System.Windows.Forms.Label lblAtRiskTitle;              // NEW
        private System.Windows.Forms.DataGridView gridRecentOrders;
        private System.Windows.Forms.DataGridView gridTopCustomers;
        private System.Windows.Forms.DataGridView gridAtRisk;           // NEW

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
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();
            this.lblDateRange = new System.Windows.Forms.Label();
            this.cmbDateRange = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();

            this.pnlBody = new System.Windows.Forms.Panel();

            this.tblKpiRow1 = new System.Windows.Forms.TableLayoutPanel();
            this.tblKpiRow2 = new System.Windows.Forms.TableLayoutPanel();
            this.tblKpiRow3 = new System.Windows.Forms.TableLayoutPanel();

            this.cardRevenue = new System.Windows.Forms.Panel();
            this.cardTodaySales = new System.Windows.Forms.Panel();
            this.cardOrders = new System.Windows.Forms.Panel();
            this.cardAvgOrder = new System.Windows.Forms.Panel();
            this.cardCustomers = new System.Windows.Forms.Panel();
            this.cardPoints = new System.Windows.Forms.Panel();

            this.cardLowStock = new System.Windows.Forms.Panel();
            this.cardOpenFeedback = new System.Windows.Forms.Panel();
            this.cardAvgRating = new System.Windows.Forms.Panel();
            this.cardPromos = new System.Windows.Forms.Panel();

            // NEW
            this.cardAtRisk = new System.Windows.Forms.Panel();
            this.cardDormant = new System.Windows.Forms.Panel();
            this.cardNeverOrdered = new System.Windows.Forms.Panel();
            this.cardPointsLiability = new System.Windows.Forms.Panel();

            this.pnlInsights = new System.Windows.Forms.Panel();
            this.lblInsightsTitle = new System.Windows.Forms.Label();
            this.flowInsights = new System.Windows.Forms.FlowLayoutPanel();

            this.tblChartsRow1 = new System.Windows.Forms.TableLayoutPanel();
            this.tblChartsRow2 = new System.Windows.Forms.TableLayoutPanel();
            this.tblChartsRow3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSalesChart = new System.Windows.Forms.Panel();
            this.pnlTopProductsChart = new System.Windows.Forms.Panel();
            this.pnlRatingChart = new System.Windows.Forms.Panel();
            this.pnlPaymentChart = new System.Windows.Forms.Panel();
            this.pnlRetentionSplitChart = new System.Windows.Forms.Panel();
            this.pnlRetentionRecencyChart = new System.Windows.Forms.Panel();

            this.tblTables = new System.Windows.Forms.TableLayoutPanel();
            this.tblRetentionRow = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRecentOrders = new System.Windows.Forms.Panel();
            this.pnlTopCustomers = new System.Windows.Forms.Panel();
            this.pnlAtRiskList = new System.Windows.Forms.Panel();
            this.lblRecentOrders = new System.Windows.Forms.Label();
            this.lblTopCustomers = new System.Windows.Forms.Label();
            this.lblAtRiskTitle = new System.Windows.Forms.Label();
            this.gridRecentOrders = new System.Windows.Forms.DataGridView();
            this.gridTopCustomers = new System.Windows.Forms.DataGridView();
            this.gridAtRisk = new System.Windows.Forms.DataGridView();

            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlInsights.SuspendLayout();
            this.pnlRecentOrders.SuspendLayout();
            this.pnlTopCustomers.SuspendLayout();
            this.pnlAtRiskList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAtRisk)).BeginInit();
            this.SuspendLayout();

            // pnlHeader (unchanged)
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 96;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);

            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 18);
            this.lblHeaderTitle.Text = "Business Intelligence";

            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(26, 50);
            this.lblHeaderSubtitle.Text = "Live KPIs, analytics, and auto-generated insights";

            this.lblDateRange.AutoSize = true;
            this.lblDateRange.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDateRange.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblDateRange.Location = new System.Drawing.Point(370, 22);
            this.lblDateRange.Text = "DATE RANGE";

            this.cmbDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDateRange.Location = new System.Drawing.Point(370, 42);
            this.cmbDateRange.Size = new System.Drawing.Size(180, 29);

            this.btnRefresh.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.Location = new System.Drawing.Point(1150, 40);
            this.btnRefresh.Size = new System.Drawing.Size(130, 34);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.lblDateRange);
            this.pnlHeader.Controls.Add(this.cmbDateRange);
            this.pnlHeader.Controls.Add(this.btnRefresh);

            // pnlBody
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.AutoScroll = true;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20);

            // KPI ROW 1 — 6 cards
            this.tblKpiRow1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblKpiRow1.Height = 130;
            this.tblKpiRow1.ColumnCount = 6;
            this.tblKpiRow1.RowCount = 1;
            this.tblKpiRow1.BackColor = System.Drawing.Color.Transparent;
            for (int i = 0; i < 6; i++)
                this.tblKpiRow1.ColumnStyles.Add(
                    new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / 6));

            BuildKpiCard(this.cardRevenue, "TOTAL REVENUE");
            BuildKpiCard(this.cardTodaySales, "TODAY'S SALES");
            BuildKpiCard(this.cardOrders, "TOTAL ORDERS");
            BuildKpiCard(this.cardAvgOrder, "AVG ORDER VALUE");
            BuildKpiCard(this.cardCustomers, "ACTIVE CUSTOMERS");
            BuildKpiCard(this.cardPoints, "POINTS IN CIRCULATION");

            this.tblKpiRow1.Controls.Add(this.cardRevenue, 0, 0);
            this.tblKpiRow1.Controls.Add(this.cardTodaySales, 1, 0);
            this.tblKpiRow1.Controls.Add(this.cardOrders, 2, 0);
            this.tblKpiRow1.Controls.Add(this.cardAvgOrder, 3, 0);
            this.tblKpiRow1.Controls.Add(this.cardCustomers, 4, 0);
            this.tblKpiRow1.Controls.Add(this.cardPoints, 5, 0);

            // KPI ROW 2 — 4 cards
            this.tblKpiRow2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblKpiRow2.Height = 130;
            this.tblKpiRow2.ColumnCount = 4;
            this.tblKpiRow2.RowCount = 1;
            this.tblKpiRow2.BackColor = System.Drawing.Color.Transparent;
            for (int i = 0; i < 4; i++)
                this.tblKpiRow2.ColumnStyles.Add(
                    new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));

            BuildKpiCard(this.cardLowStock, "LOW STOCK ITEMS");
            BuildKpiCard(this.cardOpenFeedback, "OPEN FEEDBACK");
            BuildKpiCard(this.cardAvgRating, "AVG RATING");
            BuildKpiCard(this.cardPromos, "ACTIVE PROMOTIONS");

            this.tblKpiRow2.Controls.Add(this.cardLowStock, 0, 0);
            this.tblKpiRow2.Controls.Add(this.cardOpenFeedback, 1, 0);
            this.tblKpiRow2.Controls.Add(this.cardAvgRating, 2, 0);
            this.tblKpiRow2.Controls.Add(this.cardPromos, 3, 0);

            // NEW: KPI ROW 3 — 4 retention cards
            this.tblKpiRow3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblKpiRow3.Height = 130;
            this.tblKpiRow3.ColumnCount = 4;
            this.tblKpiRow3.RowCount = 1;
            this.tblKpiRow3.BackColor = System.Drawing.Color.Transparent;
            for (int i = 0; i < 4; i++)
                this.tblKpiRow3.ColumnStyles.Add(
                    new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));

            BuildKpiCard(this.cardAtRisk, "AT RISK CUSTOMERS");
            BuildKpiCard(this.cardDormant, "DORMANT CUSTOMERS");
            BuildKpiCard(this.cardNeverOrdered, "NEVER ORDERED");
            BuildKpiCard(this.cardPointsLiability, "POINTS LIABILITY");

            this.tblKpiRow3.Controls.Add(this.cardAtRisk, 0, 0);
            this.tblKpiRow3.Controls.Add(this.cardDormant, 1, 0);
            this.tblKpiRow3.Controls.Add(this.cardNeverOrdered, 2, 0);
            this.tblKpiRow3.Controls.Add(this.cardPointsLiability, 3, 0);

            // Insights panel (unchanged)
            this.pnlInsights.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInsights.Height = 480;
            this.pnlInsights.BackColor = System.Drawing.Color.White;
            this.pnlInsights.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);

            this.lblInsightsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInsightsTitle.Height = 40;
            this.lblInsightsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblInsightsTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblInsightsTitle.Text = "💡  Key Insights";
            this.lblInsightsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInsightsTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);

            this.flowInsights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowInsights.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowInsights.WrapContents = false;
            this.flowInsights.AutoScroll = true;
            this.flowInsights.BackColor = System.Drawing.Color.White;
            this.flowInsights.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);

            this.pnlInsights.Controls.Add(this.flowInsights);
            this.pnlInsights.Controls.Add(this.lblInsightsTitle);

            // Charts row 1 (unchanged)
            this.tblChartsRow1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblChartsRow1.Height = 340;
            this.tblChartsRow1.ColumnCount = 2;
            this.tblChartsRow1.RowCount = 1;
            this.tblChartsRow1.BackColor = System.Drawing.Color.Transparent;
            this.tblChartsRow1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tblChartsRow1.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblChartsRow1.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            this.pnlSalesChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSalesChart.BackColor = System.Drawing.Color.White;
            this.pnlSalesChart.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.pnlTopProductsChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopProductsChart.BackColor = System.Drawing.Color.White;
            this.pnlTopProductsChart.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);

            this.tblChartsRow1.Controls.Add(this.pnlSalesChart, 0, 0);
            this.tblChartsRow1.Controls.Add(this.pnlTopProductsChart, 1, 0);

            // Charts row 2 (unchanged)
            this.tblChartsRow2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblChartsRow2.Height = 340;
            this.tblChartsRow2.ColumnCount = 2;
            this.tblChartsRow2.RowCount = 1;
            this.tblChartsRow2.BackColor = System.Drawing.Color.Transparent;
            this.tblChartsRow2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tblChartsRow2.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblChartsRow2.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            this.pnlRatingChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRatingChart.BackColor = System.Drawing.Color.White;
            this.pnlRatingChart.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.pnlPaymentChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPaymentChart.BackColor = System.Drawing.Color.White;
            this.pnlPaymentChart.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);

            this.tblChartsRow2.Controls.Add(this.pnlRatingChart, 0, 0);
            this.tblChartsRow2.Controls.Add(this.pnlPaymentChart, 1, 0);

            // NEW: Charts row 3 — retention
            this.tblChartsRow3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblChartsRow3.Height = 340;
            this.tblChartsRow3.ColumnCount = 2;
            this.tblChartsRow3.RowCount = 1;
            this.tblChartsRow3.BackColor = System.Drawing.Color.Transparent;
            this.tblChartsRow3.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tblChartsRow3.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblChartsRow3.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            this.pnlRetentionSplitChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRetentionSplitChart.BackColor = System.Drawing.Color.White;
            this.pnlRetentionSplitChart.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.pnlRetentionRecencyChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRetentionRecencyChart.BackColor = System.Drawing.Color.White;
            this.pnlRetentionRecencyChart.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);

            this.tblChartsRow3.Controls.Add(this.pnlRetentionSplitChart, 0, 0);
            this.tblChartsRow3.Controls.Add(this.pnlRetentionRecencyChart, 1, 0);

            // Tables (unchanged)
            this.tblTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblTables.Height = 420;
            this.tblTables.ColumnCount = 2;
            this.tblTables.RowCount = 1;
            this.tblTables.BackColor = System.Drawing.Color.Transparent;
            this.tblTables.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tblTables.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTables.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            this.pnlRecentOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecentOrders.BackColor = System.Drawing.Color.White;
            this.pnlRecentOrders.Padding = new System.Windows.Forms.Padding(16);
            this.pnlRecentOrders.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.lblRecentOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecentOrders.Height = 36;
            this.lblRecentOrders.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRecentOrders.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblRecentOrders.Text = "Recent Orders";
            this.lblRecentOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.gridRecentOrders.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlRecentOrders.Controls.Add(this.gridRecentOrders);
            this.pnlRecentOrders.Controls.Add(this.lblRecentOrders);

            this.pnlTopCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopCustomers.BackColor = System.Drawing.Color.White;
            this.pnlTopCustomers.Padding = new System.Windows.Forms.Padding(16);
            this.pnlTopCustomers.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);

            this.lblTopCustomers.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopCustomers.Height = 36;
            this.lblTopCustomers.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTopCustomers.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblTopCustomers.Text = "Top Customers by Spend";
            this.lblTopCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.gridTopCustomers.Dock = System.Windows.Forms.DockStyle.Fill;

            this.pnlTopCustomers.Controls.Add(this.gridTopCustomers);
            this.pnlTopCustomers.Controls.Add(this.lblTopCustomers);

            this.tblTables.Controls.Add(this.pnlRecentOrders, 0, 0);
            this.tblTables.Controls.Add(this.pnlTopCustomers, 1, 0);

            // NEW: Retention at-risk table row (full width)
            this.tblRetentionRow.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblRetentionRow.Height = 380;
            this.tblRetentionRow.ColumnCount = 1;
            this.tblRetentionRow.RowCount = 1;
            this.tblRetentionRow.BackColor = System.Drawing.Color.Transparent;
            this.tblRetentionRow.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tblRetentionRow.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            this.pnlAtRiskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAtRiskList.BackColor = System.Drawing.Color.White;
            this.pnlAtRiskList.Padding = new System.Windows.Forms.Padding(16);
            this.pnlAtRiskList.Margin = new System.Windows.Forms.Padding(0);

            this.lblAtRiskTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAtRiskTitle.Height = 36;
            this.lblAtRiskTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAtRiskTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblAtRiskTitle.Text = "⚠️  At-Risk & Dormant Customers — Follow Up Now";
            this.lblAtRiskTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.gridAtRisk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAtRisk.AllowUserToAddRows = false;
            this.gridAtRisk.AllowUserToDeleteRows = false;
            this.gridAtRisk.ReadOnly = true;
            this.gridAtRisk.RowHeadersVisible = false;
            this.gridAtRisk.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAtRisk.MultiSelect = false;
            this.gridAtRisk.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.pnlAtRiskList.Controls.Add(this.gridAtRisk);
            this.pnlAtRiskList.Controls.Add(this.lblAtRiskTitle);

            this.tblRetentionRow.Controls.Add(this.pnlAtRiskList, 0, 0);

            // lblStatus (unchanged)
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 28;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            // Add sections to body (last added = top)
            this.pnlBody.Controls.Add(this.tblRetentionRow);     // bottom of scroll
            this.pnlBody.Controls.Add(this.tblTables);
            this.pnlBody.Controls.Add(this.tblChartsRow3);       // NEW
            this.pnlBody.Controls.Add(this.tblChartsRow2);
            this.pnlBody.Controls.Add(this.tblChartsRow1);
            this.pnlBody.Controls.Add(this.pnlInsights);
            this.pnlBody.Controls.Add(this.tblKpiRow3);          // NEW
            this.pnlBody.Controls.Add(this.tblKpiRow2);
            this.pnlBody.Controls.Add(this.tblKpiRow1);          // top of scroll

            // FrmBusinessIntelligence
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1500, 950);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business Intelligence";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlInsights.ResumeLayout(false);
            this.pnlRecentOrders.ResumeLayout(false);
            this.pnlTopCustomers.ResumeLayout(false);
            this.pnlAtRiskList.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAtRisk)).EndInit();
            this.ResumeLayout(false);
        }

        private void BuildKpiCard(System.Windows.Forms.Panel card, string title)
        {
            card.Dock = System.Windows.Forms.DockStyle.Fill;
            card.Margin = new System.Windows.Forms.Padding(6);
            card.BackColor = System.Drawing.Color.White;
            card.Padding = new System.Windows.Forms.Padding(16);

            var lblTitle = new System.Windows.Forms.Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            lblTitle.Location = new System.Drawing.Point(16, 16);
            lblTitle.Text = title;

            var lblValue = new System.Windows.Forms.Label();
            lblValue.Name = "lblValue";
            lblValue.AutoSize = false;
            lblValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblValue.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            lblValue.Location = new System.Drawing.Point(16, 46);
            lblValue.Size = new System.Drawing.Size(180, 40);
            lblValue.Text = "—";
            lblValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            var lblSub = new System.Windows.Forms.Label();
            lblSub.Name = "lblSub";
            lblSub.AutoSize = false;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblSub.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            lblSub.Location = new System.Drawing.Point(16, 86);
            lblSub.Size = new System.Drawing.Size(180, 18);
            lblSub.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblSub);
        }
    }
}