namespace CRM.winForms.Forms
{
    partial class FrmPromotions
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkShowArchived;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAddPromotion;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView gridPromotions;

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
            this.chkShowArchived = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddPromotion = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.gridPromotions = new System.Windows.Forms.DataGridView();

            this.pnlTop.SuspendLayout();
            this.pnlGridWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPromotions)).BeginInit();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 65;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 14, 16, 14);

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(16, 20);
            this.txtSearch.Size = new System.Drawing.Size(320, 25);
            this.txtSearch.PlaceholderText = "Search by code or name...";

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(346, 18);
            this.btnRefresh.Size = new System.Drawing.Size(100, 32);
            this.btnRefresh.Text = "Refresh";

            // btnAddPromotion
            this.btnAddPromotion.Location = new System.Drawing.Point(456, 18);
            this.btnAddPromotion.Size = new System.Drawing.Size(160, 32);
            this.btnAddPromotion.Text = "Add Promotion";

            // chkShowArchived
            this.chkShowArchived.Location = new System.Drawing.Point(630, 22);
            this.chkShowArchived.Size = new System.Drawing.Size(160, 25);
            this.chkShowArchived.Text = "Show inactive";
            this.chkShowArchived.UseVisualStyleBackColor = true;

            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.btnAddPromotion);
            this.pnlTop.Controls.Add(this.chkShowArchived);

            // lblStatus
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 32;
            this.lblStatus.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // gridPromotions
            this.gridPromotions.Dock = System.Windows.Forms.DockStyle.Fill;

            // pnlGridWrap
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(16);
            this.pnlGridWrap.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.pnlGridWrap.Controls.Add(this.gridPromotions);

            // FrmPromotions
            this.ClientSize = new System.Drawing.Size(1150, 650);
            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(950, 550);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Promotions";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlGridWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPromotions)).EndInit();
            this.ResumeLayout(false);
        }
    }
}