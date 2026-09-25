namespace CRM.winForms.Forms
{
    partial class FrmManagerDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.FlowLayoutPanel flowCards;

        private System.Windows.Forms.Panel cardTodaySales;
        private System.Windows.Forms.Panel cardTodayOrders;
        private System.Windows.Forms.Panel cardLowStock;
        private System.Windows.Forms.Panel cardOpenFeedback;
        private System.Windows.Forms.Panel cardActivePromos;
        private System.Windows.Forms.Panel cardPendingRetention;

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

            this.cardTodaySales = new System.Windows.Forms.Panel();
            this.cardTodayOrders = new System.Windows.Forms.Panel();
            this.cardLowStock = new System.Windows.Forms.Panel();
            this.cardOpenFeedback = new System.Windows.Forms.Panel();
            this.cardActivePromos = new System.Windows.Forms.Panel();
            this.cardPendingRetention = new System.Windows.Forms.Panel();

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
            // pnlHeader
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 96;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 18, 24, 18);

            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 18);
            this.lblHeaderTitle.Text = "Manager Dashboard";

            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(26, 50);
            this.lblHeaderSubtitle.Text = "Today's operations at a glance";

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
            // pnlBody
            // ============================================================
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.AutoScroll = true;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20);

            // ---- KPI row ----
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowCards.Height = 130;
            this.flowCards.WrapContents = false;
            this.flowCards.AutoScroll = false;
            this.flowCards.BackColor = System.Drawing.Color.Transparent;

            BuildKpiCard(this.cardTodaySales, "TODAY'S SALES");
            BuildKpiCard(this.cardTodayOrders, "TODAY'S ORDERS");
            BuildKpiCard(this.cardLowStock, "LOW STOCK ITEMS");
            BuildKpiCard(this.cardOpenFeedback, "OPEN FEEDBACK");
            BuildKpiCard(this.cardActivePromos, "ACTIVE PROMOTIONS");
            BuildKpiCard(this.cardPendingRetention, "PENDING RETENTION");

            this.flowCards.Controls.Add(this.cardTodaySales);
            this.flowCards.Controls.Add(this.cardTodayOrders);
            this.flowCards.Controls.Add(this.cardLowStock);
            this.flowCards.Controls.Add(this.cardOpenFeedback);
            this.flowCards.Controls.Add(this.cardActivePromos);
            this.flowCards.Controls.Add(this.cardPendingRetention);

            // ---- Recent orders ----
            this.pnlRecentOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRecentOrders.Height = 420;
            this.pnlRecentOrders.BackColor = System.Drawing.Color.White;
            this.pnlRecentOrders.Padding = new System.Windows.Forms.Padding(16);

            this.lblRecentOrders.AutoSize = true;
            this.lblRecentOrders.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecentOrders.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblRecentOrders.Location = new System.Drawing.Point(16, 12);
            this.lblRecentOrders.Text = "Recent Orders";

            this.gridRecentOrders.Location = new System.Drawing.Point(16, 46);
            this.gridRecentOrders.Size = new System.Drawing.Size(1300, 350);
            this.gridRecentOrders.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.pnlRecentOrders.Controls.Add(this.lblRecentOrders);
            this.pnlRecentOrders.Controls.Add(this.gridRecentOrders);

            this.pnlBody.Controls.Add(this.pnlRecentOrders);
            this.pnlBody.Controls.Add(this.flowCards);

            // ---- Status ----
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 28;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            // ============================================================
            // FrmManagerDashboard
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manager Dashboard";

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
            card.Width = 210;
            card.Height = 110;
            card.Margin = new System.Windows.Forms.Padding(8);
            card.BackColor = System.Drawing.Color.White;

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

            var lblSub = new System.Windows.Forms.Label();
            lblSub.Name = "lblSub";
            lblSub.AutoSize = false;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblSub.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            lblSub.Location = new System.Drawing.Point(16, 86);
            lblSub.Size = new System.Drawing.Size(180, 18);

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblSub);
        }
    }
}