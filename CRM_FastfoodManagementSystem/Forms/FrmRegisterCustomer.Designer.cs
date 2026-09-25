namespace CRM.winForms.Forms
{
    partial class FrmRegisterCustomer
    {
        private System.ComponentModel.IContainer components = null;

        // Header
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;

        // Personal
        private System.Windows.Forms.Panel pnlPersonal;
        private System.Windows.Forms.Label lblPersonalTitle;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblMiddleName;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblBirthday;
        private System.Windows.Forms.DateTimePicker dtpBirthday;

        // Contact
        private System.Windows.Forms.Panel pnlContact;
        private System.Windows.Forms.Label lblContactTitle;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox txtContact;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;

        // Terms
        private System.Windows.Forms.Panel pnlTerms;
        private System.Windows.Forms.Label lblTermsTitle;
        private System.Windows.Forms.CheckBox chkAcceptTerms;
        private System.Windows.Forms.Button btnViewTerms;

        // Footer
        private System.Windows.Forms.Panel pnlFooter;
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSubtitle = new System.Windows.Forms.Label();

            this.pnlPersonal = new System.Windows.Forms.Panel();
            this.lblPersonalTitle = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblMiddleName = new System.Windows.Forms.Label();
            this.txtMiddleName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblBirthday = new System.Windows.Forms.Label();
            this.dtpBirthday = new System.Windows.Forms.DateTimePicker();

            this.pnlContact = new System.Windows.Forms.Panel();
            this.lblContactTitle = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();

            this.pnlTerms = new System.Windows.Forms.Panel();
            this.lblTermsTitle = new System.Windows.Forms.Label();
            this.chkAcceptTerms = new System.Windows.Forms.CheckBox();
            this.btnViewTerms = new System.Windows.Forms.Button();

            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlPersonal.SuspendLayout();
            this.pnlContact.SuspendLayout();
            this.pnlTerms.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // ============================================================
            // FrmRegisterCustomer
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(760, 860);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Register Customer";

            // ============================================================
            // pnlHeader — Dock Top
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 88;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);

            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 16);
            this.lblHeaderTitle.Text = "Register Customer";

            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(26, 48);
            this.lblHeaderSubtitle.Text = "Enter the customer's details below";

            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);

            // ============================================================
            // pnlPersonal — Dock Top
            // ============================================================
            this.pnlPersonal.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPersonal.Height = 260;
            this.pnlPersonal.BackColor = System.Drawing.Color.White;
            this.pnlPersonal.Padding = new System.Windows.Forms.Padding(24, 20, 24, 20);

            this.lblPersonalTitle.AutoSize = true;
            this.lblPersonalTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblPersonalTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblPersonalTitle.Location = new System.Drawing.Point(24, 16);
            this.lblPersonalTitle.Text = "Personal Information";

            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblCode.Location = new System.Drawing.Point(24, 52);
            this.lblCode.Text = "CUSTOMER CODE";

            this.txtCode.Location = new System.Drawing.Point(24, 72);
            this.txtCode.Size = new System.Drawing.Size(330, 27);

            this.lblBirthday.AutoSize = true;
            this.lblBirthday.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblBirthday.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblBirthday.Location = new System.Drawing.Point(374, 52);
            this.lblBirthday.Text = "BIRTHDAY";

            this.dtpBirthday.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthday.Location = new System.Drawing.Point(374, 72);
            this.dtpBirthday.Size = new System.Drawing.Size(330, 27);

            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblFirstName.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblFirstName.Location = new System.Drawing.Point(24, 120);
            this.lblFirstName.Text = "FIRST NAME";

            this.txtFirstName.Location = new System.Drawing.Point(24, 140);
            this.txtFirstName.Size = new System.Drawing.Size(220, 27);

            this.lblMiddleName.AutoSize = true;
            this.lblMiddleName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblMiddleName.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblMiddleName.Location = new System.Drawing.Point(254, 120);
            this.lblMiddleName.Text = "MIDDLE NAME (OPTIONAL)";

            this.txtMiddleName.Location = new System.Drawing.Point(254, 140);
            this.txtMiddleName.Size = new System.Drawing.Size(220, 27);

            this.lblLastName.AutoSize = true;
            this.lblLastName.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblLastName.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblLastName.Location = new System.Drawing.Point(484, 120);
            this.lblLastName.Text = "LAST NAME";

            this.txtLastName.Location = new System.Drawing.Point(484, 140);
            this.txtLastName.Size = new System.Drawing.Size(220, 27);

            this.pnlPersonal.Controls.Add(this.lblPersonalTitle);
            this.pnlPersonal.Controls.Add(this.lblCode);
            this.pnlPersonal.Controls.Add(this.txtCode);
            this.pnlPersonal.Controls.Add(this.lblBirthday);
            this.pnlPersonal.Controls.Add(this.dtpBirthday);
            this.pnlPersonal.Controls.Add(this.lblFirstName);
            this.pnlPersonal.Controls.Add(this.txtFirstName);
            this.pnlPersonal.Controls.Add(this.lblMiddleName);
            this.pnlPersonal.Controls.Add(this.txtMiddleName);
            this.pnlPersonal.Controls.Add(this.lblLastName);
            this.pnlPersonal.Controls.Add(this.txtLastName);

            // ============================================================
            // pnlContact — Dock Top
            // ============================================================
            this.pnlContact.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContact.Height = 250;
            this.pnlContact.BackColor = System.Drawing.Color.White;

            this.lblContactTitle.AutoSize = true;
            this.lblContactTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblContactTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblContactTitle.Location = new System.Drawing.Point(24, 16);
            this.lblContactTitle.Text = "Contact Information";

            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblContact.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblContact.Location = new System.Drawing.Point(24, 52);
            this.lblContact.Text = "CONTACT NUMBER";

            this.txtContact.Location = new System.Drawing.Point(24, 72);
            this.txtContact.Size = new System.Drawing.Size(330, 27);

            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblEmail.Location = new System.Drawing.Point(374, 52);
            this.lblEmail.Text = "EMAIL ADDRESS";

            this.txtEmail.Location = new System.Drawing.Point(374, 72);
            this.txtEmail.Size = new System.Drawing.Size(330, 27);

            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblAddress.Location = new System.Drawing.Point(24, 116);
            this.lblAddress.Text = "ADDRESS";

            this.txtAddress.Location = new System.Drawing.Point(24, 136);
            this.txtAddress.Multiline = true;
            this.txtAddress.Size = new System.Drawing.Size(680, 88);

            this.pnlContact.Controls.Add(this.lblContactTitle);
            this.pnlContact.Controls.Add(this.lblContact);
            this.pnlContact.Controls.Add(this.txtContact);
            this.pnlContact.Controls.Add(this.lblEmail);
            this.pnlContact.Controls.Add(this.txtEmail);
            this.pnlContact.Controls.Add(this.lblAddress);
            this.pnlContact.Controls.Add(this.txtAddress);

            // ============================================================
            // pnlTerms — Dock Top (added LAST before footer to sit right above it)
            // ============================================================
            this.pnlTerms.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTerms.Height = 130;
            this.pnlTerms.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);

            this.lblTermsTitle.AutoSize = true;
            this.lblTermsTitle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTermsTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblTermsTitle.Location = new System.Drawing.Point(24, 16);
            this.lblTermsTitle.Text = "Terms and Conditions";

            this.chkAcceptTerms.AutoSize = true;
            this.chkAcceptTerms.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkAcceptTerms.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.chkAcceptTerms.Location = new System.Drawing.Point(24, 52);
            this.chkAcceptTerms.Text = "Customer accepted Terms & Conditions";
            this.chkAcceptTerms.UseVisualStyleBackColor = true;

            this.btnViewTerms.Location = new System.Drawing.Point(24, 80);
            this.btnViewTerms.Size = new System.Drawing.Size(240, 34);
            this.btnViewTerms.Text = "View / Print Terms && Conditions";
            this.btnViewTerms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlTerms.Controls.Add(this.lblTermsTitle);
            this.pnlTerms.Controls.Add(this.chkAcceptTerms);
            this.pnlTerms.Controls.Add(this.btnViewTerms);

            // ============================================================
            // pnlFooter — Dock Bottom
            // ============================================================
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 120;
            this.pnlFooter.BackColor = System.Drawing.Color.White;

            this.lblStatus.AutoSize = false;
            this.lblStatus.Location = new System.Drawing.Point(24, 12);
            this.lblStatus.Size = new System.Drawing.Size(712, 32);
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(30, 150, 90);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.btnSave.Location = new System.Drawing.Point(484, 60);
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnCancel.Location = new System.Drawing.Point(616, 60);
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.pnlFooter.Controls.Add(this.lblStatus);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.btnCancel);

            // ============================================================
            // Add to form — ORDER MATTERS for Dock = Top stacking:
            //   * Last added Top panel appears CLOSEST to the top
            //   * First added Top panel appears LOWEST (just above the Bottom-docked panels)
            //
            // So we add them in REVERSE visual order:
            //   1. pnlTerms      (added first  → ends up just above footer)
            //   2. pnlContact    (added next   → above terms)
            //   3. pnlPersonal   (added next   → above contact)
            //   4. pnlHeader     (added last   → very top)
            //
            // And Bottom-docked pnlFooter is added after everything
            // so it stays pinned to the bottom edge.
            // ============================================================
            this.Controls.Add(this.pnlTerms);
            this.Controls.Add(this.pnlContact);
            this.Controls.Add(this.pnlPersonal);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlPersonal.ResumeLayout(false);
            this.pnlPersonal.PerformLayout();
            this.pnlContact.ResumeLayout(false);
            this.pnlContact.PerformLayout();
            this.pnlTerms.ResumeLayout(false);
            this.pnlTerms.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}