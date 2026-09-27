namespace CRM.winForms.Forms
{
    partial class FrmBranchEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;

        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox txtContact;

        private System.Windows.Forms.Label lblError;

        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;

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
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();

            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();

            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();

            this.lblContact = new System.Windows.Forms.Label();
            this.txtContact = new System.Windows.Forms.TextBox();

            this.lblError = new System.Windows.Forms.Label();

            this.pnlFooter = new System.Windows.Forms.Panel();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // pnlHeader
            // ============================================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(51, 117, 160);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 72;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);

            // lblTitle
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Height = 42;
            this.lblTitle.Padding = new System.Windows.Forms.Padding(22, 14, 0, 0);
            this.lblTitle.Text = "Add Branch";

            // lblSubtitle
            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(204, 223, 232);
            this.lblSubtitle.Height = 26;
            this.lblSubtitle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.lblSubtitle.Text = "Manage branch information";

            // ============================================================
            // Field labels + inputs
            // ============================================================
            int left = 32;
            int labelW = 160;
            int inputLeft = left + labelW + 12;
            int inputW = 420;
            int rowH = 40;

            // Branch Code
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCode.Location = new System.Drawing.Point(left, 112);
            this.lblCode.Text = "Branch Code *";

            this.txtCode.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCode.Location = new System.Drawing.Point(inputLeft, 108);
            this.txtCode.Size = new System.Drawing.Size(inputW, 30);
            this.txtCode.MaxLength = 50;

            // Branch Name
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.Location = new System.Drawing.Point(left, 112 + rowH);
            this.lblName.Text = "Branch Name *";

            this.txtName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtName.Location = new System.Drawing.Point(inputLeft, 108 + rowH);
            this.txtName.Size = new System.Drawing.Size(inputW, 30);
            this.txtName.MaxLength = 200;

            // Address (multiline)
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAddress.Location = new System.Drawing.Point(left, 112 + rowH * 2);
            this.lblAddress.Text = "Address";

            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAddress.Location = new System.Drawing.Point(inputLeft, 108 + rowH * 2);
            this.txtAddress.Multiline = true;
            this.txtAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAddress.Size = new System.Drawing.Size(inputW, 72);
            this.txtAddress.MaxLength = 500;

            // Contact Number
            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblContact.Location = new System.Drawing.Point(left, 112 + rowH * 2 + 96);
            this.lblContact.Text = "Contact Number";

            this.txtContact.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtContact.Location = new System.Drawing.Point(inputLeft, 108 + rowH * 2 + 96);
            this.txtContact.Size = new System.Drawing.Size(inputW, 30);
            this.txtContact.MaxLength = 50;

            // ============================================================
            // lblError
            // ============================================================
            this.lblError.AutoSize = false;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.lblError.Location = new System.Drawing.Point(left, 112 + rowH * 2 + 96 + 44);
            this.lblError.Size = new System.Drawing.Size(560, 40);

            // ============================================================
            // pnlFooter
            // ============================================================
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 68;
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);

            this.flowButtons.AutoSize = true;
            this.flowButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowButtons.WrapContents = false;
            this.flowButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowButtons.Padding = new System.Windows.Forms.Padding(0);
            this.flowButtons.Margin = new System.Windows.Forms.Padding(0);

            this.btnSave.Width = 140;
            this.btnSave.Height = 40;
            this.btnSave.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;

            this.btnCancel.Width = 140;
            this.btnCancel.Height = 40;
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;

            this.flowButtons.Controls.Add(this.btnSave);
            this.flowButtons.Controls.Add(this.btnCancel);

            this.pnlFooter.Controls.Add(this.flowButtons);

            // ============================================================
            // FrmBranchEditor
            // ============================================================
            this.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.ClientSize = new System.Drawing.Size(620, 480);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtContact);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Branch";

            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.flowButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}