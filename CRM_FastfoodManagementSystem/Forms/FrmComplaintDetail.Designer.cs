namespace CRM.winForms.Forms
{
    #nullable disable
    partial class FrmComplaintDetail
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblMeta;

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblCustomerLabel;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblOrderLabel;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.Label lblCategoryLabel;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblSeverityLabel;
        private System.Windows.Forms.Label lblSeverity;
        private System.Windows.Forms.Label lblSubjectLabel;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblDescriptionLabel;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblAssignedLabel;
        private System.Windows.Forms.ComboBox cmbAssignedTo;
        private System.Windows.Forms.Label lblResolutionLabel;
        private System.Windows.Forms.TextBox txtResolution;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
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
            this.lblCode = new System.Windows.Forms.Label();
            this.lblMeta = new System.Windows.Forms.Label();

            this.pnlBody = new System.Windows.Forms.Panel();
            this.lblCustomerLabel = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblOrderLabel = new System.Windows.Forms.Label();
            this.lblOrder = new System.Windows.Forms.Label();
            this.lblCategoryLabel = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblSeverityLabel = new System.Windows.Forms.Label();
            this.lblSeverity = new System.Windows.Forms.Label();
            this.lblSubjectLabel = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblDescriptionLabel = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblAssignedLabel = new System.Windows.Forms.Label();
            this.cmbAssignedTo = new System.Windows.Forms.ComboBox();
            this.lblResolutionLabel = new System.Windows.Forms.Label();
            this.txtResolution = new System.Windows.Forms.TextBox();

            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 84;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 12);

            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCode.Location = new System.Drawing.Point(24, 16);
            this.lblCode.Text = "Complaint";

            this.lblMeta.AutoSize = true;
            this.lblMeta.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Italic);
            this.lblMeta.Location = new System.Drawing.Point(26, 50);
            this.lblMeta.Text = "Details";

            this.pnlHeader.Controls.Add(this.lblCode);
            this.pnlHeader.Controls.Add(this.lblMeta);

            // pnlBody
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);
            this.pnlBody.AutoScroll = true;

            int y = 12;
            int labelX = 24;
            int valueX = 140;
            int rowH = 26;

            // Customer
            this.lblCustomerLabel.AutoSize = true;
            this.lblCustomerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblCustomerLabel.Text = "Customer:";

            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCustomer.Location = new System.Drawing.Point(valueX, y);
            this.lblCustomer.Text = "—";
            y += rowH;

            // Order
            this.lblOrderLabel.AutoSize = true;
            this.lblOrderLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOrderLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblOrderLabel.Text = "Order:";

            this.lblOrder.AutoSize = true;
            this.lblOrder.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblOrder.Location = new System.Drawing.Point(valueX, y);
            this.lblOrder.Text = "—";
            y += rowH;

            // Category
            this.lblCategoryLabel.AutoSize = true;
            this.lblCategoryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCategoryLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblCategoryLabel.Text = "Category:";

            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCategory.Location = new System.Drawing.Point(valueX, y);
            this.lblCategory.Text = "—";
            y += rowH;

            // Severity
            this.lblSeverityLabel.AutoSize = true;
            this.lblSeverityLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSeverityLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblSeverityLabel.Text = "Severity:";

            this.lblSeverity.AutoSize = true;
            this.lblSeverity.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSeverity.Location = new System.Drawing.Point(valueX, y);
            this.lblSeverity.Text = "—";
            y += rowH + 4;

            // Subject
            this.lblSubjectLabel.AutoSize = true;
            this.lblSubjectLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubjectLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblSubjectLabel.Text = "Subject:";

            this.lblSubject.AutoSize = true;
            this.lblSubject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubject.Location = new System.Drawing.Point(valueX, y);
            this.lblSubject.MaximumSize = new System.Drawing.Size(440, 0);
            this.lblSubject.Text = "—";
            y += 32;

            // Description
            this.lblDescriptionLabel.AutoSize = true;
            this.lblDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescriptionLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblDescriptionLabel.Text = "Description:";

            this.txtDescription.Location = new System.Drawing.Point(valueX, y);
            this.txtDescription.Size = new System.Drawing.Size(440, 100);
            this.txtDescription.Multiline = true;
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.BackColor = System.Drawing.Color.White;
            y += 112;

            // Status
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblStatusLabel.Text = "Status:";

            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Location = new System.Drawing.Point(valueX, y - 3);
            this.cmbStatus.Size = new System.Drawing.Size(200, 27);
            y += rowH + 8;

            // Assigned
            this.lblAssignedLabel.AutoSize = true;
            this.lblAssignedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAssignedLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblAssignedLabel.Text = "Assigned To:";

            this.cmbAssignedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignedTo.Location = new System.Drawing.Point(valueX, y - 3);
            this.cmbAssignedTo.Size = new System.Drawing.Size(280, 27);
            y += rowH + 8;

            // Resolution
            this.lblResolutionLabel.AutoSize = true;
            this.lblResolutionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblResolutionLabel.Location = new System.Drawing.Point(labelX, y);
            this.lblResolutionLabel.Text = "Resolution:";

            this.txtResolution.Location = new System.Drawing.Point(valueX, y);
            this.txtResolution.Size = new System.Drawing.Size(440, 100);
            this.txtResolution.Multiline = true;
            this.txtResolution.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResolution.MaxLength = 2000;

            this.pnlBody.Controls.Add(this.lblCustomerLabel);
            this.pnlBody.Controls.Add(this.lblCustomer);
            this.pnlBody.Controls.Add(this.lblOrderLabel);
            this.pnlBody.Controls.Add(this.lblOrder);
            this.pnlBody.Controls.Add(this.lblCategoryLabel);
            this.pnlBody.Controls.Add(this.lblCategory);
            this.pnlBody.Controls.Add(this.lblSeverityLabel);
            this.pnlBody.Controls.Add(this.lblSeverity);
            this.pnlBody.Controls.Add(this.lblSubjectLabel);
            this.pnlBody.Controls.Add(this.lblSubject);
            this.pnlBody.Controls.Add(this.lblDescriptionLabel);
            this.pnlBody.Controls.Add(this.txtDescription);
            this.pnlBody.Controls.Add(this.lblStatusLabel);
            this.pnlBody.Controls.Add(this.cmbStatus);
            this.pnlBody.Controls.Add(this.lblAssignedLabel);
            this.pnlBody.Controls.Add(this.cmbAssignedTo);
            this.pnlBody.Controls.Add(this.lblResolutionLabel);
            this.pnlBody.Controls.Add(this.txtResolution);

            // pnlButtons
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 60;
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);

            this.btnEdit.Location = new System.Drawing.Point(200, 12);
            this.btnEdit.Size = new System.Drawing.Size(110, 34);
            this.btnEdit.Text = "Edit";
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnSave.Location = new System.Drawing.Point(320, 12);
            this.btnSave.Size = new System.Drawing.Size(150, 34);
            this.btnSave.Text = "Save Changes";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnClose.Location = new System.Drawing.Point(480, 12);
            this.btnClose.Size = new System.Drawing.Size(90, 34);
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlButtons.Controls.Add(this.btnEdit);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnClose);

            // FrmComplaintDetail
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 760);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Complaint";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}