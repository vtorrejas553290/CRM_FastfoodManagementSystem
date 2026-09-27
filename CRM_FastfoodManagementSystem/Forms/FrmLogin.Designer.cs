namespace CRM.winForms.Forms
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblCompanyCode;
        private System.Windows.Forms.TextBox txtCompanyCode;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblCompanyCode = new System.Windows.Forms.Label();
            this.txtCompanyCode = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // =========================================================
            // pnlHeader — unchanged
            // =========================================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(51, 117, 160);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 130;
            this.pnlHeader.Controls.Add(this.lblAppName);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // lblAppName — unchanged
            this.lblAppName.AutoSize = false;
            this.lblAppName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Height = 60;
            this.lblAppName.Padding = new System.Windows.Forms.Padding(0, 18, 0, 0);
            this.lblAppName.Text = "Fastfood Management System";
            this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblSubtitle — unchanged
            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(204, 223, 232);
            this.lblSubtitle.Height = 40;
            this.lblSubtitle.Text = "Customer Relationship Management";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // =========================================================
            // NEW — lblCompanyCode + txtCompanyCode
            // =========================================================
            this.lblCompanyCode.AutoSize = true;
            this.lblCompanyCode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCompanyCode.Location = new System.Drawing.Point(60, 165);
            this.lblCompanyCode.Text = "Company Code";

            this.txtCompanyCode.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCompanyCode.Location = new System.Drawing.Point(60, 190);
            this.txtCompanyCode.Size = new System.Drawing.Size(380, 30);

            // =========================================================
            // lblUser — shifted down 70
            // =========================================================
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUser.Location = new System.Drawing.Point(60, 235);
            this.lblUser.Text = "Username";

            // txtUsername — shifted down 70
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUsername.Location = new System.Drawing.Point(60, 260);
            this.txtUsername.Size = new System.Drawing.Size(380, 30);

            // lblPass — shifted down 70
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPass.Location = new System.Drawing.Point(60, 305);
            this.lblPass.Text = "Password";

            // txtPassword — shifted down 70
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.Location = new System.Drawing.Point(60, 330);
            this.txtPassword.Size = new System.Drawing.Size(380, 30);
            this.txtPassword.UseSystemPasswordChar = true;

            // btnLogin — shifted down 70
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Location = new System.Drawing.Point(60, 390);
            this.btnLogin.Size = new System.Drawing.Size(185, 42);
            this.btnLogin.Text = "Sign In";
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;

            // btnExit — shifted down 70
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.Location = new System.Drawing.Point(255, 390);
            this.btnExit.Size = new System.Drawing.Size(185, 42);
            this.btnExit.Text = "Exit";
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;

            // lblError — shifted down 70
            this.lblError.AutoSize = false;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.lblError.Location = new System.Drawing.Point(60, 450);
            this.lblError.Size = new System.Drawing.Size(380, 40);

            // =========================================================
            // FrmLogin — height grows by 70
            // =========================================================
            this.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.ClientSize = new System.Drawing.Size(500, 520);
            this.Controls.Add(this.lblCompanyCode);
            this.Controls.Add(this.txtCompanyCode);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fastfood Management System — Login";
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}