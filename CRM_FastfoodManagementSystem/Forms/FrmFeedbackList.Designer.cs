namespace CRM.winForms.Forms
{
    partial class FrmFeedbackList
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlGridWrap;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Label lblFilterStars;
        private System.Windows.Forms.ComboBox cmbFilterStars;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gridFeedback;

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
            this.lblMode = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterStars = new System.Windows.Forms.Label();
            this.cmbFilterStars = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.gridFeedback = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.pnlGridWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFeedback)).BeginInit();
            this.SuspendLayout();

            // pnlTop
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 150;
            this.pnlTop.BackColor = System.Drawing.Color.White;

            // lblMode
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMode.Location = new System.Drawing.Point(20, 14);
            this.lblMode.Size = new System.Drawing.Size(500, 32);
            this.lblMode.Text = "Mode";

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 65);
            this.lblSearch.Size = new System.Drawing.Size(70, 17);
            this.lblSearch.Text = "Search:";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(85, 62);
            this.txtSearch.Size = new System.Drawing.Size(280, 25);
            this.txtSearch.PlaceholderText = "Search by customer name or comment...";

            // lblFilterStatus
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(385, 65);
            this.lblFilterStatus.Size = new System.Drawing.Size(60, 17);
            this.lblFilterStatus.Text = "Status:";

            // cmbFilterStatus
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.Location = new System.Drawing.Point(445, 62);
            this.cmbFilterStatus.Size = new System.Drawing.Size(140, 25);

            // lblFilterStars
            this.lblFilterStars.AutoSize = true;
            this.lblFilterStars.Location = new System.Drawing.Point(605, 65);
            this.lblFilterStars.Size = new System.Drawing.Size(60, 17);
            this.lblFilterStars.Text = "Stars:";

            // cmbFilterStars
            this.cmbFilterStars.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStars.Location = new System.Drawing.Point(660, 62);
            this.cmbFilterStars.Size = new System.Drawing.Size(140, 25);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(20, 100);
            this.btnRefresh.Size = new System.Drawing.Size(110, 34);
            this.btnRefresh.Text = "Refresh";

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(140, 100);
            this.btnAdd.Size = new System.Drawing.Size(160, 34);
            this.btnAdd.Text = "Add Feedback";

            // pnlGridWrap
            this.pnlGridWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridWrap.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.pnlGridWrap.BackColor = System.Drawing.Color.White;

            // gridFeedback
            this.gridFeedback.Dock = System.Windows.Forms.DockStyle.Fill;

            // FrmFeedbackList
            this.ClientSize = new System.Drawing.Size(1050, 680);
            this.Controls.Add(this.pnlGridWrap);
            this.Controls.Add(this.pnlTop);
            this.pnlTop.Controls.Add(this.lblMode);
            this.pnlTop.Controls.Add(this.lblSearch);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblFilterStatus);
            this.pnlTop.Controls.Add(this.cmbFilterStatus);
            this.pnlTop.Controls.Add(this.lblFilterStars);
            this.pnlTop.Controls.Add(this.cmbFilterStars);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.btnAdd);
            this.pnlGridWrap.Controls.Add(this.gridFeedback);
            this.MinimumSize = new System.Drawing.Size(950, 550);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Feedback";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlGridWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFeedback)).EndInit();
            this.ResumeLayout(false);
        }
    }
}