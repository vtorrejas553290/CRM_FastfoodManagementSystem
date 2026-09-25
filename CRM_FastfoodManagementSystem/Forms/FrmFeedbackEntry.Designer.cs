namespace CRM.winForms.Forms
{
    partial class FrmFeedbackEntry
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.NumericUpDown numRating;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblStatusField;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblRating = new System.Windows.Forms.Label();
            this.numRating = new System.Windows.Forms.NumericUpDown();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblStatusField = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numRating)).BeginInit();
            this.SuspendLayout();

            // =========================================================
            // Row 1 — Customer
            // =========================================================
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(20, 22);
            this.lblCustomer.Size = new System.Drawing.Size(100, 17);
            this.lblCustomer.Text = "Customer:";

            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.Location = new System.Drawing.Point(160, 19);
            this.cmbCustomer.Size = new System.Drawing.Size(310, 25);

            // =========================================================
            // Row 2 — Rating
            // =========================================================
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new System.Drawing.Point(20, 62);
            this.lblRating.Size = new System.Drawing.Size(100, 17);
            this.lblRating.Text = "Rating (1-5):";

            this.numRating.Location = new System.Drawing.Point(160, 59);
            this.numRating.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numRating.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numRating.Size = new System.Drawing.Size(80, 25);
            this.numRating.Value = new decimal(new int[] { 3, 0, 0, 0 });

            // =========================================================
            // Row 3 — Category
            // =========================================================
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(20, 102);
            this.lblCategory.Size = new System.Drawing.Size(100, 17);
            this.lblCategory.Text = "Category:";

            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Location = new System.Drawing.Point(160, 99);
            this.cmbCategory.Size = new System.Drawing.Size(310, 25);

            // =========================================================
            // Row 4 — Status  (was overlapping)
            // =========================================================
            this.lblStatusField.AutoSize = true;
            this.lblStatusField.Location = new System.Drawing.Point(20, 142);
            this.lblStatusField.Size = new System.Drawing.Size(100, 17);
            this.lblStatusField.Text = "Status:";

            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Location = new System.Drawing.Point(160, 139);
            this.cmbStatus.Size = new System.Drawing.Size(310, 25);

            // =========================================================
            // Row 5 — Comments
            // =========================================================
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(20, 182);
            this.lblComments.Size = new System.Drawing.Size(100, 17);
            this.lblComments.Text = "Comments:";

            this.txtComments.Location = new System.Drawing.Point(160, 179);
            this.txtComments.Multiline = true;
            this.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComments.Size = new System.Drawing.Size(310, 130);

            // =========================================================
            // Row 6 — Buttons
            // =========================================================
            this.btnSave.Location = new System.Drawing.Point(160, 325);
            this.btnSave.Size = new System.Drawing.Size(140, 38);
            this.btnSave.Text = "Save";

            this.btnCancel.Location = new System.Drawing.Point(310, 325);
            this.btnCancel.Size = new System.Drawing.Size(140, 38);
            this.btnCancel.Text = "Cancel";

            // =========================================================
            // Row 7 — Status message
            // =========================================================
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblStatus.Location = new System.Drawing.Point(20, 380);
            this.lblStatus.MaximumSize = new System.Drawing.Size(450, 60);
            this.lblStatus.Size = new System.Drawing.Size(450, 60);

            // =========================================================
            // FrmFeedbackEntry
            // =========================================================
            this.ClientSize = new System.Drawing.Size(510, 460);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.numRating);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblStatusField);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblComments);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Feedback";
            ((System.ComponentModel.ISupportInitialize)(this.numRating)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}