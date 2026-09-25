namespace CRM.winForms.Forms
{
    #nullable disable
    partial class FrmComplaintEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.ComboBox cmbOrder;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblSeverity;
        private System.Windows.Forms.ComboBox cmbSeverity;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;

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
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.cmbOrder = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblSeverity = new System.Windows.Forms.Label();
            this.cmbSeverity = new System.Windows.Forms.ComboBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 84;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 12);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(24, 16);
            this.lblTitle.Text = "File Complaint";

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            this.lblSubtitle.Location = new System.Drawing.Point(26, 50);
            this.lblSubtitle.Text = "Log a customer complaint";

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // pnlBody
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);
            this.pnlBody.AutoScroll = true;

            int y = 12;

            // Customer
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomer.Location = new System.Drawing.Point(24, y);
            this.lblCustomer.Text = "CUSTOMER *";
            y += 22;

            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.Location = new System.Drawing.Point(24, y);
            this.cmbCustomer.Size = new System.Drawing.Size(560, 27);
            this.cmbCustomer.DropDownHeight = 300;
            y += 38;

            // Order
            this.lblOrder.AutoSize = true;
            this.lblOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOrder.Location = new System.Drawing.Point(24, y);
            this.lblOrder.Text = "RELATED ORDER (OPTIONAL)";
            y += 22;

            this.cmbOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrder.Location = new System.Drawing.Point(24, y);
            this.cmbOrder.Size = new System.Drawing.Size(560, 27);
            this.cmbOrder.DropDownHeight = 300;
            y += 38;

            // Category + Severity side by side
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCategory.Location = new System.Drawing.Point(24, y);
            this.lblCategory.Text = "CATEGORY *";

            this.lblSeverity.AutoSize = true;
            this.lblSeverity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSeverity.Location = new System.Drawing.Point(320, y);
            this.lblSeverity.Text = "SEVERITY *";
            y += 22;

            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Location = new System.Drawing.Point(24, y);
            this.cmbCategory.Size = new System.Drawing.Size(280, 27);

            this.cmbSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSeverity.Location = new System.Drawing.Point(320, y);
            this.cmbSeverity.Size = new System.Drawing.Size(264, 27);
            y += 38;

            // Subject
            this.lblSubject.AutoSize = true;
            this.lblSubject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubject.Location = new System.Drawing.Point(24, y);
            this.lblSubject.Text = "SUBJECT *";
            y += 22;

            this.txtSubject.Location = new System.Drawing.Point(24, y);
            this.txtSubject.Size = new System.Drawing.Size(560, 25);
            this.txtSubject.MaxLength = 200;
            y += 38;

            // Description
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(24, y);
            this.lblDescription.Text = "DESCRIPTION *";
            y += 22;

            this.txtDescription.Location = new System.Drawing.Point(24, y);
            this.txtDescription.Size = new System.Drawing.Size(560, 160);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.MaxLength = 2000;

            this.pnlBody.Controls.Add(this.lblCustomer);
            this.pnlBody.Controls.Add(this.cmbCustomer);
            this.pnlBody.Controls.Add(this.lblOrder);
            this.pnlBody.Controls.Add(this.cmbOrder);
            this.pnlBody.Controls.Add(this.lblCategory);
            this.pnlBody.Controls.Add(this.cmbCategory);
            this.pnlBody.Controls.Add(this.lblSeverity);
            this.pnlBody.Controls.Add(this.cmbSeverity);
            this.pnlBody.Controls.Add(this.lblSubject);
            this.pnlBody.Controls.Add(this.txtSubject);
            this.pnlBody.Controls.Add(this.lblDescription);
            this.pnlBody.Controls.Add(this.txtDescription);

            // pnlButtons
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 60;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);

            this.btnSave.Location = new System.Drawing.Point(364, 12);
            this.btnSave.Size = new System.Drawing.Size(120, 34);
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnCancel.Location = new System.Drawing.Point(494, 12);
            this.btnCancel.Size = new System.Drawing.Size(90, 34);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnCancel);

            // FrmComplaintEditor
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 620);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Complaint";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}