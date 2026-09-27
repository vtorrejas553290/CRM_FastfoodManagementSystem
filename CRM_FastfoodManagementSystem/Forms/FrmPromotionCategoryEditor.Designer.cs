namespace CRM.winForms.Forms
{
    partial class FrmPromotionCategoryEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblHint;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

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

            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 16, 20, 12);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Text = "Promotion Category";

            this.pnlHeader.Controls.Add(this.lblTitle);

            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);

            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(20, 20);
            this.lblName.Text = "NAME";

            this.txtName.Location = new System.Drawing.Point(20, 42);
            this.txtName.Size = new System.Drawing.Size(520, 29);
            this.txtName.MaxLength = 100;

            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(20, 84);
            this.lblDescription.Text = "DESCRIPTION";

            this.txtDescription.Location = new System.Drawing.Point(20, 106);
            this.txtDescription.Size = new System.Drawing.Size(520, 80);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.MaxLength = 500;

            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblHint.Location = new System.Drawing.Point(20, 200);
            this.lblHint.MaximumSize = new System.Drawing.Size(520, 0);
            this.lblHint.Text = "";

            this.pnlBody.Controls.Add(this.lblName);
            this.pnlBody.Controls.Add(this.txtName);
            this.pnlBody.Controls.Add(this.lblDescription);
            this.pnlBody.Controls.Add(this.txtDescription);
            this.pnlBody.Controls.Add(this.lblHint);

            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 64;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);

            this.btnSave.Width = 140;
            this.btnSave.Height = 40;
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;

            this.btnCancel.Width = 100;
            this.btnCancel.Height = 40;
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnCancel);

            this.pnlButtons.Resize += (_, __) =>
            {
                this.btnCancel.Location = new System.Drawing.Point(
                    this.pnlButtons.Width - this.btnCancel.Width - 20, 12);
                this.btnSave.Location = new System.Drawing.Point(
                    this.btnCancel.Left - this.btnSave.Width - 12, 12);
            };

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(600, 380);
            this.MinimumSize = new System.Drawing.Size(520, 340);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Promotion Category";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;

            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}