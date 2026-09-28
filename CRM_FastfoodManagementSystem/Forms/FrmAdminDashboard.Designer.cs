namespace CRM.winForms.Forms
{
    partial class FrmAdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.FlowLayoutPanel flowCards;

        private System.Windows.Forms.Panel cardTotalCustomers;
        private System.Windows.Forms.Panel cardTotalRevenue;
        private System.Windows.Forms.Panel cardActiveUsers;
        private System.Windows.Forms.Panel cardTotalProducts;
        private System.Windows.Forms.Panel cardAvgOrderValue;
        private System.Windows.Forms.Panel cardNewCustomersMonth;
        private System.Windows.Forms.Panel cardNewOrdersWeek;

        private System.Windows.Forms.Panel pnlRecentOrders;
        private System.Windows.Forms.Label lblRecentOrders;
        private System.Windows.Forms.DataGridView gridRecentOrders;

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
            this.btnRefresh = new System.Windows.Forms.Button();

            this.pnlBody = new System.Windows.Forms.Panel();
            this.flowCards = new System.Windows.Forms.FlowLayoutPanel();

            this.cardTotalCustomers = new System.Windows.Forms.Panel();
            this.cardTotalRevenue = new System.Windows.Forms.Panel();
            this.cardActiveUsers = new System.Windows.Forms.Panel();
            this.cardTotalProducts = new System.Windows.Forms.Panel();
            this.cardAvgOrderValue = new System.Windows.Forms.Panel();
            this.cardNewCustomersMonth = new System.Windows.Forms.Panel();
            this.cardNewOrdersWeek = new System.Windows.Forms.Panel();

            this.pnlRecentOrders = new System.Windows.Forms.Panel();
            this.lblRecentOrders = new System.Windows.Forms.Label();
            this.gridRecentOrders = new System.Windows.Forms.DataGridView();

            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlRecentOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentOrders)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // pnlHeader — hidden. The sidebar top bar already shows the
            // page title, so this header would be redundant. We keep the
            // Refresh button accessible by re-parenting it to the body.
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 96;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);
            this.pnlHeader.Visible = false;

            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 18);
            this.lblHeaderTitle.Text = "Dashboard";

            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(26, 50);
            this.lblHeaderSubtitle.Text = "Business overview at a glance";

            this.btnRefresh.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.Location = new System.Drawing.Point(1150, 38);
            this.btnRefresh.Size = new System.Drawing.Size(130, 34);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.btnRefresh);

            // ============================================================
            // pnlBody — fills the whole form
            // ============================================================
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.AutoScroll = false;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20);

            // ============================================================
            // flowCards — two rows of KPI cards, wraps horizontally
            // ============================================================
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowCards.Height = 260;
            this.flowCards.WrapContents = true;
            this.flowCards.AutoScroll = false;
            this.flowCards.BackColor = System.Drawing.Color.Transparent;
            this.flowCards.Padding = new System.Windows.Forms.Padding(0);
            this.flowCards.Margin = new System.Windows.Forms.Padding(0);

            BuildKpiCard(this.cardTotalCustomers, "TOTAL CUSTOMERS");
            BuildKpiCard(this.cardTotalRevenue, "TOTAL REVENUE");
            BuildKpiCard(this.cardActiveUsers, "ACTIVE USERS");
            BuildKpiCard(this.cardTotalProducts, "TOTAL PRODUCTS");
            BuildKpiCard(this.cardAvgOrderValue, "AVERAGE ORDER VALUE");
            BuildKpiCard(this.cardNewCustomersMonth, "NEW CUSTOMERS THIS MONTH");
            BuildKpiCard(this.cardNewOrdersWeek, "NEW ORDERS THIS WEEK");

            this.flowCards.Controls.Add(this.cardTotalCustomers);
            this.flowCards.Controls.Add(this.cardTotalRevenue);
            this.flowCards.Controls.Add(this.cardActiveUsers);
            this.flowCards.Controls.Add(this.cardTotalProducts);
            this.flowCards.Controls.Add(this.cardAvgOrderValue);
            this.flowCards.Controls.Add(this.cardNewCustomersMonth);
            this.flowCards.Controls.Add(this.cardNewOrdersWeek);

            // ============================================================
            // pnlRecentOrders — fills the space below the cards
            // ============================================================
            this.pnlRecentOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecentOrders.BackColor = System.Drawing.Color.White;
            this.pnlRecentOrders.Padding = new System.Windows.Forms.Padding(16);
            this.pnlRecentOrders.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);

            this.lblRecentOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecentOrders.Height = 30;
            this.lblRecentOrders.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecentOrders.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblRecentOrders.Text = "Recent Orders";

            this.gridRecentOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRecentOrders.AllowUserToAddRows = false;
            this.gridRecentOrders.AllowUserToDeleteRows = false;
            this.gridRecentOrders.ReadOnly = true;
            this.gridRecentOrders.RowHeadersVisible = false;
            this.gridRecentOrders.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRecentOrders.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRecentOrders.BackgroundColor = System.Drawing.Color.White;
            this.gridRecentOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlRecentOrders.Controls.Add(this.gridRecentOrders);
            this.pnlRecentOrders.Controls.Add(this.lblRecentOrders);

            // ============================================================
            // Status bar at the bottom
            // ============================================================
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 28;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            // ============================================================
            // Compose form
            // The recent-orders panel fills the body; the flowCards
            // panel is docked to the top of the body, so it sits above.
            // ============================================================
            this.pnlBody.Controls.Add(this.pnlRecentOrders);
            this.pnlBody.Controls.Add(this.flowCards);

            // ============================================================
            // FrmAdminDashboard
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlRecentOrders.ResumeLayout(false);
            this.pnlRecentOrders.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentOrders)).EndInit();
            this.ResumeLayout(false);
        }

        private void BuildKpiCard(System.Windows.Forms.Panel card, string title)
        {
            card.Width = 240;
            card.Height = 110;
            card.Margin = new System.Windows.Forms.Padding(6);
            card.BackColor = System.Drawing.Color.White;
            card.Padding = new System.Windows.Forms.Padding(16);

            var lblTitle = new System.Windows.Forms.Label();
            lblTitle.AutoSize = false;
            lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTitle.Height = 20;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            lblTitle.Text = title;

            var lblValue = new System.Windows.Forms.Label();
            lblValue.Name = "lblValue";
            lblValue.AutoSize = false;
            lblValue.Dock = System.Windows.Forms.DockStyle.Fill;
            lblValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblValue.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblValue.Text = "—";

            var lblSub = new System.Windows.Forms.Label();
            lblSub.Name = "lblSub";
            lblSub.AutoSize = false;
            lblSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblSub.Height = 18;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblSub.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            card.Controls.Add(lblValue);
            card.Controls.Add(lblSub);
            card.Controls.Add(lblTitle);
        }
    }
}