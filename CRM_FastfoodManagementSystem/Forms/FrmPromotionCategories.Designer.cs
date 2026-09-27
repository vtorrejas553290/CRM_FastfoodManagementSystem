namespace CRM.winForms.Forms
{
    partial class FrmPromotionCategories
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.DataGridView gridCategories;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;

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
            this.gridCategories = new System.Windows.Forms.DataGridView();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCategories)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 64;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 16, 20, 12);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 18);
            this.lblTitle.Text = "Promotion Categories";

            this.pnlHeader.Controls.Add(this.lblTitle);

            // pnlBody
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);

            this.gridCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCategories.BackgroundColor = System.Drawing.Color.White;
            this.gridCategories.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridCategories.RowHeadersVisible = false;
            this.gridCategories.AllowUserToAddRows = false;
            this.gridCategories.AllowUserToDeleteRows = false;
            this.gridCategories.ReadOnly = true;
            this.gridCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCategories.MultiSelect = false;
            this.gridCategories.RowTemplate.Height = 34;

            this.pnlBody.Controls.Add(this.gridCategories);

            // pnlButtons
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 64;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);

            this.btnAdd.Width = 160;
            this.btnAdd.Height = 40;
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.btnAdd.Location = new System.Drawing.Point(20, 12);
            this.btnAdd.Text = "Add Category";
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;

            this.btnClose.Width = 100;
            this.btnClose.Height = 40;
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnAdd);
            this.pnlButtons.Controls.Add(this.btnClose);

            this.pnlButtons.Resize += (_, __) =>
            {
                this.btnClose.Location = new System.Drawing.Point(
                    this.pnlButtons.Width - this.btnClose.Width - 20, 12);
            };

            // FrmPromotionCategories
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(720, 520);
            this.MinimumSize = new System.Drawing.Size(620, 460);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Promotion Categories";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;

            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCategories)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}