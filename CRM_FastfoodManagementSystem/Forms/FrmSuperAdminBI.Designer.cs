namespace CRM.winForms.Forms
{
    partial class FrmSuperAdminBI
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.TableLayoutPanel tblKpis;

        private System.Windows.Forms.Panel cardTenants;
        private System.Windows.Forms.Panel cardSubs;
        private System.Windows.Forms.Panel cardPlans;
        private System.Windows.Forms.Panel cardRoutes;

        private System.Windows.Forms.TableLayoutPanel tblCharts;
        private System.Windows.Forms.Panel pnlSubsByPlanChart;
        private System.Windows.Forms.Panel pnlPlanFeaturesChart;

        private System.Windows.Forms.Panel pnlSubscriptions;
        private System.Windows.Forms.Label lblSubscriptionsTitle;
        private System.Windows.Forms.DataGridView gridSubscriptions;

        private System.Windows.Forms.Panel pnlPlans;
        private System.Windows.Forms.Label lblPlansTitle;
        private System.Windows.Forms.DataGridView gridPlans;

        private System.Windows.Forms.Panel pnlRouting;
        private System.Windows.Forms.Label lblRoutingTitle;
        private System.Windows.Forms.DataGridView gridRouting;

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
            this.tblKpis = new System.Windows.Forms.TableLayoutPanel();

            this.cardTenants = new System.Windows.Forms.Panel();
            this.cardSubs = new System.Windows.Forms.Panel();
            this.cardPlans = new System.Windows.Forms.Panel();
            this.cardRoutes = new System.Windows.Forms.Panel();

            this.tblCharts = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSubsByPlanChart = new System.Windows.Forms.Panel();
            this.pnlPlanFeaturesChart = new System.Windows.Forms.Panel();

            this.pnlSubscriptions = new System.Windows.Forms.Panel();
            this.lblSubscriptionsTitle = new System.Windows.Forms.Label();
            this.gridSubscriptions = new System.Windows.Forms.DataGridView();

            this.pnlPlans = new System.Windows.Forms.Panel();
            this.lblPlansTitle = new System.Windows.Forms.Label();
            this.gridPlans = new System.Windows.Forms.DataGridView();

            this.pnlRouting = new System.Windows.Forms.Panel();
            this.lblRoutingTitle = new System.Windows.Forms.Label();
            this.gridRouting = new System.Windows.Forms.DataGridView();

            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.tblKpis.SuspendLayout();
            this.tblCharts.SuspendLayout();
            this.pnlSubscriptions.SuspendLayout();
            this.pnlPlans.SuspendLayout();
            this.pnlRouting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubscriptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPlans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRouting)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
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
            this.lblHeaderSubtitle.Text = "Tenants, subscriptions, plans, and database routing";

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

            // pnlBody
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.AutoScroll = true;
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20);

            // ============================================================
            // tblKpis — 4 equal columns, stretches full width
            // ============================================================
            this.tblKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblKpis.Height = 140;
            this.tblKpis.ColumnCount = 4;
            this.tblKpis.RowCount = 1;
            this.tblKpis.BackColor = System.Drawing.Color.Transparent;
            this.tblKpis.Padding = new System.Windows.Forms.Padding(0);
            this.tblKpis.Margin = new System.Windows.Forms.Padding(0);

            for (int i = 0; i < 4; i++)
                this.tblKpis.ColumnStyles.Add(
                    new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));

            this.tblKpis.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            BuildKpiCard(this.cardTenants, "TOTAL TENANTS");
            BuildKpiCard(this.cardSubs, "ACTIVE SUBSCRIPTIONS");
            BuildKpiCard(this.cardPlans, "PLANS");
            BuildKpiCard(this.cardRoutes, "DATABASE ROUTES");

            this.tblKpis.Controls.Add(this.cardTenants, 0, 0);
            this.tblKpis.Controls.Add(this.cardSubs, 1, 0);
            this.tblKpis.Controls.Add(this.cardPlans, 2, 0);
            this.tblKpis.Controls.Add(this.cardRoutes, 3, 0);

            // tblCharts
            this.tblCharts.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblCharts.Height = 340;
            this.tblCharts.ColumnCount = 2;
            this.tblCharts.RowCount = 1;
            this.tblCharts.BackColor = System.Drawing.Color.Transparent;
            this.tblCharts.Padding = new System.Windows.Forms.Padding(0, 12, 0, 12);
            this.tblCharts.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCharts.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            this.pnlSubsByPlanChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubsByPlanChart.BackColor = System.Drawing.Color.White;
            this.pnlSubsByPlanChart.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            this.pnlPlanFeaturesChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlanFeaturesChart.BackColor = System.Drawing.Color.White;
            this.pnlPlanFeaturesChart.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);

            this.tblCharts.Controls.Add(this.pnlSubsByPlanChart, 0, 0);
            this.tblCharts.Controls.Add(this.pnlPlanFeaturesChart, 1, 0);

            // pnlSubscriptions
            this.pnlSubscriptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubscriptions.Height = 220;
            this.pnlSubscriptions.BackColor = System.Drawing.Color.White;
            this.pnlSubscriptions.Padding = new System.Windows.Forms.Padding(16);
            this.pnlSubscriptions.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);

            this.lblSubscriptionsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubscriptionsTitle.Height = 36;
            this.lblSubscriptionsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSubscriptionsTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblSubscriptionsTitle.Text = "Tenant Subscriptions";
            this.lblSubscriptionsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.gridSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSubscriptions.AllowUserToAddRows = false;
            this.gridSubscriptions.AllowUserToDeleteRows = false;
            this.gridSubscriptions.ReadOnly = true;
            this.gridSubscriptions.RowHeadersVisible = false;
            this.gridSubscriptions.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSubscriptions.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridSubscriptions.BackgroundColor = System.Drawing.Color.White;
            this.gridSubscriptions.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlSubscriptions.Controls.Add(this.gridSubscriptions);
            this.pnlSubscriptions.Controls.Add(this.lblSubscriptionsTitle);

            // pnlPlans
            this.pnlPlans.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPlans.Height = 180;
            this.pnlPlans.BackColor = System.Drawing.Color.White;
            this.pnlPlans.Padding = new System.Windows.Forms.Padding(16);
            this.pnlPlans.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);

            this.lblPlansTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPlansTitle.Height = 36;
            this.lblPlansTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPlansTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblPlansTitle.Text = "Plan Feature Matrix";
            this.lblPlansTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.gridPlans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPlans.AllowUserToAddRows = false;
            this.gridPlans.AllowUserToDeleteRows = false;
            this.gridPlans.ReadOnly = true;
            this.gridPlans.RowHeadersVisible = false;
            this.gridPlans.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPlans.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPlans.BackgroundColor = System.Drawing.Color.White;
            this.gridPlans.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlPlans.Controls.Add(this.gridPlans);
            this.pnlPlans.Controls.Add(this.lblPlansTitle);

            // pnlRouting
            this.pnlRouting.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRouting.Height = 220;
            this.pnlRouting.BackColor = System.Drawing.Color.White;
            this.pnlRouting.Padding = new System.Windows.Forms.Padding(16);
            this.pnlRouting.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);

            this.lblRoutingTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRoutingTitle.Height = 36;
            this.lblRoutingTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRoutingTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblRoutingTitle.Text = "Database Routing (Silo Configuration)";
            this.lblRoutingTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.gridRouting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRouting.AllowUserToAddRows = false;
            this.gridRouting.AllowUserToDeleteRows = false;
            this.gridRouting.ReadOnly = true;
            this.gridRouting.RowHeadersVisible = false;
            this.gridRouting.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRouting.AutoSizeColumnsMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRouting.BackgroundColor = System.Drawing.Color.White;
            this.gridRouting.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.pnlRouting.Controls.Add(this.gridRouting);
            this.pnlRouting.Controls.Add(this.lblRoutingTitle);

            this.pnlBody.Controls.Add(this.pnlRouting);
            this.pnlBody.Controls.Add(this.pnlPlans);
            this.pnlBody.Controls.Add(this.pnlSubscriptions);
            this.pnlBody.Controls.Add(this.tblCharts);
            this.pnlBody.Controls.Add(this.tblKpis);

            // lblStatus
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 28;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            // FrmSuperAdminBI
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1400, 980);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(1000, 760);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business Intelligence";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tblKpis.ResumeLayout(false);
            this.tblCharts.ResumeLayout(false);
            this.pnlSubscriptions.ResumeLayout(false);
            this.pnlPlans.ResumeLayout(false);
            this.pnlRouting.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSubscriptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPlans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRouting)).EndInit();
            this.ResumeLayout(false);
        }

        private void BuildKpiCard(System.Windows.Forms.Panel card, string title)
        {
            card.Dock = System.Windows.Forms.DockStyle.Fill;
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
            lblValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblValue.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblValue.Text = "—";

            var lblSub = new System.Windows.Forms.Label();
            lblSub.Name = "lblSub";
            lblSub.AutoSize = false;
            lblSub.Dock = System.Windows.Forms.DockStyle.Bottom;
            lblSub.Height = 20;
            lblSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblSub.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            card.Controls.Add(lblValue);
            card.Controls.Add(lblSub);
            card.Controls.Add(lblTitle);
        }
    }
}