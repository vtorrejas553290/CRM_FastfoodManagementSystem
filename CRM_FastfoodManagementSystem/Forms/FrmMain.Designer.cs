namespace CRM.winForms.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.FlowLayoutPanel pnlMenu;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pnlMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();

            // =========================================================
            // pnlSidebar — wider (280) to fit full menu labels
            // =========================================================
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 280;
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(51, 117, 160);
            this.pnlSidebar.Controls.Add(this.pnlMenu);
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Controls.Add(this.lblWelcome);
            this.pnlSidebar.Controls.Add(this.lblBrand);

            // =========================================================
            // lblBrand — brand / logo area
            // =========================================================
            this.lblBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBrand.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblBrand.ForeColor = System.Drawing.Color.White;
            this.lblBrand.Height = 70;
            this.lblBrand.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.lblBrand.Text = "Fastfood MS";
            this.lblBrand.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // =========================================================
            // lblWelcome — signed-in user info
            // =========================================================
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(204, 223, 232);
            this.lblWelcome.Height = 60;
            this.lblWelcome.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.lblWelcome.Text = "Welcome";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // =========================================================
            // btnLogout — pinned to bottom of sidebar
            // =========================================================
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.Height = 55;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Text = "Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;

            // =========================================================
            // pnlMenu — fills the middle of the sidebar
            // =========================================================
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlMenu.WrapContents = false;
            this.pnlMenu.AutoScroll = true;
            this.pnlMenu.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(51, 117, 160);

            // =========================================================
            // pnlTop — page title bar
            // =========================================================
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 60;
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblPageTitle);

            // lblPageTitle
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(0, 41, 68);
            this.lblPageTitle.Location = new System.Drawing.Point(20, 15);
            this.lblPageTitle.Text = "Dashboard";

            // =========================================================
            // pnlContent — hosts in-panel forms
            // =========================================================
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

            // =========================================================
            // FrmMain
            // =========================================================
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlSidebar);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fastfood Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlSidebar.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}