namespace CRM.winForms.Forms
{
    partial class FrmOrderManagement
    {
        private System.ComponentModel.IContainer components = null;

        // HEADER
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSubtitle;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblOrderType;
        private System.Windows.Forms.ComboBox cmbOrderType;
        private System.Windows.Forms.Button btnNewOrder;

        // CATALOG
        private System.Windows.Forms.Panel pnlCatalog;
        private System.Windows.Forms.Panel pnlCatalogHeader;
        private System.Windows.Forms.Label lblCatalogTitle;
        private System.Windows.Forms.Label lblCatalogSubtitle;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.FlowLayoutPanel flowProducts;
        private System.Windows.Forms.Label lblCatalogEmpty;

        // CART
        private System.Windows.Forms.Panel pnlCart;
        private System.Windows.Forms.Panel pnlCartHeader;
        private System.Windows.Forms.Label lblCartTitle;
        private System.Windows.Forms.Label lblCartItems;
        private System.Windows.Forms.DataGridView gridCart;

        // CART FOOTER
        private System.Windows.Forms.Panel pnlCartFooter;

        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblTotalLabel;
        private System.Windows.Forms.Label lblTotal;

        private System.Windows.Forms.Label lblPromotion;
        private System.Windows.Forms.ComboBox cmbPromotion;
        private System.Windows.Forms.Button btnApplyPromo;
        private System.Windows.Forms.Button btnClearPromo;

        private System.Windows.Forms.Label lblPointsBalance;
        private System.Windows.Forms.Label lblRedeemPoints;
        private System.Windows.Forms.NumericUpDown numRedeemPoints;
        private System.Windows.Forms.Button btnRedeemPoints;
        private System.Windows.Forms.Button btnClearPoints;

        private System.Windows.Forms.Label lblPaymentSection;
        private System.Windows.Forms.Label lblAmountPaid;
        private System.Windows.Forms.NumericUpDown numAmountPaid;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblChangeLabel;
        private System.Windows.Forms.Label lblChange;

        private System.Windows.Forms.Button btnPay;
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
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblOrderType = new System.Windows.Forms.Label();
            this.cmbOrderType = new System.Windows.Forms.ComboBox();
            this.btnNewOrder = new System.Windows.Forms.Button();

            this.pnlCatalog = new System.Windows.Forms.Panel();
            this.pnlCatalogHeader = new System.Windows.Forms.Panel();
            this.lblCatalogTitle = new System.Windows.Forms.Label();
            this.lblCatalogSubtitle = new System.Windows.Forms.Label();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            this.flowProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCatalogEmpty = new System.Windows.Forms.Label();

            this.pnlCart = new System.Windows.Forms.Panel();
            this.pnlCartHeader = new System.Windows.Forms.Panel();
            this.lblCartTitle = new System.Windows.Forms.Label();
            this.lblCartItems = new System.Windows.Forms.Label();
            this.gridCart = new System.Windows.Forms.DataGridView();
            this.pnlCartFooter = new System.Windows.Forms.Panel();

            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();

            this.lblPromotion = new System.Windows.Forms.Label();
            this.cmbPromotion = new System.Windows.Forms.ComboBox();
            this.btnApplyPromo = new System.Windows.Forms.Button();
            this.btnClearPromo = new System.Windows.Forms.Button();

            this.lblPointsBalance = new System.Windows.Forms.Label();
            this.lblRedeemPoints = new System.Windows.Forms.Label();
            this.numRedeemPoints = new System.Windows.Forms.NumericUpDown();
            this.btnRedeemPoints = new System.Windows.Forms.Button();
            this.btnClearPoints = new System.Windows.Forms.Button();

            this.lblPaymentSection = new System.Windows.Forms.Label();
            this.lblAmountPaid = new System.Windows.Forms.Label();
            this.numAmountPaid = new System.Windows.Forms.NumericUpDown();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblChangeLabel = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();

            this.btnPay = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlCatalog.SuspendLayout();
            this.pnlCatalogHeader.SuspendLayout();
            this.pnlCart.SuspendLayout();
            this.pnlCartHeader.SuspendLayout();
            this.pnlCartFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountPaid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRedeemPoints)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // FrmOrderManagement
            // ============================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(1500, 900);
            this.MinimumSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Management";

            // ============================================================
            // HEADER
            // ============================================================
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 96;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);

            // Title block (left)
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblHeaderTitle.Location = new System.Drawing.Point(24, 14);
            this.lblHeaderTitle.Text = "New Order";

            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(26, 46);
            this.lblHeaderSubtitle.Text = "Create and process a customer order";

            // Customer (label above input)
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblCustomer.Location = new System.Drawing.Point(370, 14);
            this.lblCustomer.Text = "CUSTOMER";

            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCustomer.Location = new System.Drawing.Point(370, 34);
            this.cmbCustomer.Size = new System.Drawing.Size(320, 29);

            // Order Type
            this.lblOrderType.AutoSize = true;
            this.lblOrderType.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblOrderType.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblOrderType.Location = new System.Drawing.Point(710, 14);
            this.lblOrderType.Text = "ORDER TYPE";

            this.cmbOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrderType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbOrderType.Location = new System.Drawing.Point(710, 34);
            this.cmbOrderType.Size = new System.Drawing.Size(160, 29);

            // New Order button (anchored right)
            this.btnNewOrder.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;
            this.btnNewOrder.Location = new System.Drawing.Point(1150, 32);
            this.btnNewOrder.Size = new System.Drawing.Size(150, 36);
            this.btnNewOrder.Text = "+  New Order";
            this.btnNewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrder.FlatAppearance.BorderSize = 0;

            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.lblCustomer);
            this.pnlHeader.Controls.Add(this.cmbCustomer);
            this.pnlHeader.Controls.Add(this.lblOrderType);
            this.pnlHeader.Controls.Add(this.cmbOrderType);
            this.pnlHeader.Controls.Add(this.btnNewOrder);

            // ============================================================
            // CART (right side)
            // ============================================================
            this.pnlCart.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlCart.Width = 540;
            this.pnlCart.BackColor = System.Drawing.Color.White;
            this.pnlCart.Padding = new System.Windows.Forms.Padding(20);

            // Cart header
            this.pnlCartHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCartHeader.Height = 60;
            this.pnlCartHeader.BackColor = System.Drawing.Color.White;

            this.lblCartTitle.AutoSize = true;
            this.lblCartTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblCartTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblCartTitle.Location = new System.Drawing.Point(0, 4);
            this.lblCartTitle.Text = "Current Order";

            this.lblCartItems.AutoSize = true;
            this.lblCartItems.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCartItems.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblCartItems.Location = new System.Drawing.Point(1, 32);
            this.lblCartItems.Text = "0 items";

            this.pnlCartHeader.Controls.Add(this.lblCartTitle);
            this.pnlCartHeader.Controls.Add(this.lblCartItems);

            // Cart grid
            this.gridCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCart.BackgroundColor = System.Drawing.Color.White;
            this.gridCart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridCart.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridCart.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridCart.EnableHeadersVisualStyles = false;
            this.gridCart.RowHeadersVisible = false;
            this.gridCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCart.MultiSelect = false;
            this.gridCart.AllowUserToAddRows = false;
            this.gridCart.AllowUserToDeleteRows = false;
            this.gridCart.AllowUserToResizeRows = false;
            this.gridCart.RowTemplate.Height = 42;

            // ============================================================
            // CART FOOTER — aligned using X positions and consistent Y rhythm
            // ============================================================
            this.pnlCartFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCartFooter.Height = 570;
            this.pnlCartFooter.BackColor = System.Drawing.Color.White;

            // ------------------------------------------------------------
            // Totals section
            // ------------------------------------------------------------
            // Subtotal (left label / right value)
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            this.lblSubTotal.Location = new System.Drawing.Point(0, 10);
            this.lblSubTotal.Text = "SubTotal: ₱0.00";

            // Discount
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDiscount.ForeColor = System.Drawing.Color.FromArgb(30, 150, 90);
            this.lblDiscount.Location = new System.Drawing.Point(0, 34);
            this.lblDiscount.Text = "Discount: -₱0.00";
            this.lblDiscount.Visible = false;

            // TOTAL label + value
            this.lblTotalLabel.AutoSize = true;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblTotalLabel.Location = new System.Drawing.Point(0, 66);
            this.lblTotalLabel.Text = "TOTAL";

            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(13, 91, 209);
            this.lblTotal.Location = new System.Drawing.Point(340, 56);
            this.lblTotal.Text = "₱0.00";

            // Divider
            var div1 = new System.Windows.Forms.Panel();
            div1.BackColor = System.Drawing.Color.FromArgb(230, 234, 240);
            div1.Location = new System.Drawing.Point(0, 108);
            div1.Size = new System.Drawing.Size(460, 1);

            // ------------------------------------------------------------
            // Promotion section
            // ------------------------------------------------------------
            this.lblPromotion.AutoSize = true;
            this.lblPromotion.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblPromotion.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblPromotion.Location = new System.Drawing.Point(0, 122);
            this.lblPromotion.Text = "PROMOTION";

            this.cmbPromotion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPromotion.Location = new System.Drawing.Point(0, 144);
            this.cmbPromotion.Size = new System.Drawing.Size(280, 29);

            this.btnApplyPromo.Location = new System.Drawing.Point(290, 143);
            this.btnApplyPromo.Size = new System.Drawing.Size(80, 31);
            this.btnApplyPromo.Text = "Apply";
            this.btnApplyPromo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyPromo.FlatAppearance.BorderSize = 0;

            this.btnClearPromo.Location = new System.Drawing.Point(376, 143);
            this.btnClearPromo.Size = new System.Drawing.Size(80, 31);
            this.btnClearPromo.Text = "Clear";
            this.btnClearPromo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // Divider
            var div2 = new System.Windows.Forms.Panel();
            div2.BackColor = System.Drawing.Color.FromArgb(230, 234, 240);
            div2.Location = new System.Drawing.Point(0, 188);
            div2.Size = new System.Drawing.Size(460, 1);

            // ------------------------------------------------------------
            // Loyalty section
            // ------------------------------------------------------------
            this.lblPointsBalance.AutoSize = true;
            this.lblPointsBalance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPointsBalance.ForeColor = System.Drawing.Color.FromArgb(30, 150, 90);
            this.lblPointsBalance.Location = new System.Drawing.Point(0, 202);
            this.lblPointsBalance.Text = "Loyalty Points: —";

            this.lblRedeemPoints.AutoSize = true;
            this.lblRedeemPoints.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblRedeemPoints.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            this.lblRedeemPoints.Location = new System.Drawing.Point(0, 228);
            this.lblRedeemPoints.Text = "Redeem Points";

            this.numRedeemPoints.Location = new System.Drawing.Point(0, 248);
            this.numRedeemPoints.Size = new System.Drawing.Size(110, 29);
            this.numRedeemPoints.Maximum = 1000000;
            this.numRedeemPoints.DecimalPlaces = 0;

            this.btnRedeemPoints.Location = new System.Drawing.Point(120, 247);
            this.btnRedeemPoints.Size = new System.Drawing.Size(70, 31);
            this.btnRedeemPoints.Text = "Use";
            this.btnRedeemPoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedeemPoints.FlatAppearance.BorderSize = 0;

            this.btnClearPoints.Location = new System.Drawing.Point(200, 247);
            this.btnClearPoints.Size = new System.Drawing.Size(70, 31);
            this.btnClearPoints.Text = "Clear";
            this.btnClearPoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // Divider
            var div3 = new System.Windows.Forms.Panel();
            div3.BackColor = System.Drawing.Color.FromArgb(230, 234, 240);
            div3.Location = new System.Drawing.Point(0, 292);
            div3.Size = new System.Drawing.Size(460, 1);

            // ------------------------------------------------------------
            // Payment section
            // ------------------------------------------------------------
            this.lblPaymentSection.AutoSize = true;
            this.lblPaymentSection.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentSection.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblPaymentSection.Location = new System.Drawing.Point(0, 304);
            this.lblPaymentSection.Text = "Payment";

            // Amount Paid
            this.lblAmountPaid.AutoSize = true;
            this.lblAmountPaid.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblAmountPaid.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblAmountPaid.Location = new System.Drawing.Point(0, 336);
            this.lblAmountPaid.Text = "AMOUNT PAID";

            this.numAmountPaid.DecimalPlaces = 2;
            this.numAmountPaid.Location = new System.Drawing.Point(0, 356);
            this.numAmountPaid.Size = new System.Drawing.Size(460, 34);
            this.numAmountPaid.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numAmountPaid.ThousandsSeparator = true;
            this.numAmountPaid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            // Payment Method (left half)
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblPaymentMethod.Location = new System.Drawing.Point(0, 400);
            this.lblPaymentMethod.Text = "PAYMENT METHOD";

            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPaymentMethod.Location = new System.Drawing.Point(0, 420);
            this.cmbPaymentMethod.Size = new System.Drawing.Size(225, 29);

            // GCash Reference (right half)
            this.lblReference.AutoSize = true;
            this.lblReference.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblReference.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblReference.Location = new System.Drawing.Point(235, 400);
            this.lblReference.Text = "GCASH REFERENCE";

            this.txtReference.Location = new System.Drawing.Point(235, 420);
            this.txtReference.Size = new System.Drawing.Size(225, 29);

            // Change
            this.lblChangeLabel.AutoSize = true;
            this.lblChangeLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblChangeLabel.ForeColor = System.Drawing.Color.FromArgb(90, 100, 115);
            this.lblChangeLabel.Location = new System.Drawing.Point(0, 462);
            this.lblChangeLabel.Text = "CHANGE";

            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblChange.ForeColor = System.Drawing.Color.FromArgb(30, 150, 90);
            this.lblChange.Location = new System.Drawing.Point(340, 458);
            this.lblChange.Text = "₱0.00";

            // Pay button (full-width)
            this.btnPay.Location = new System.Drawing.Point(0, 492);
            this.btnPay.Size = new System.Drawing.Size(460, 48);
            this.btnPay.Text = "PAY ORDER";
            this.btnPay.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.FlatAppearance.BorderSize = 0;

            // Status
            this.lblStatus.AutoSize = false;
            this.lblStatus.Location = new System.Drawing.Point(0, 548);
            this.lblStatus.Size = new System.Drawing.Size(460, 20);
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);

            // Add to footer
            this.pnlCartFooter.Controls.Add(this.lblSubTotal);
            this.pnlCartFooter.Controls.Add(this.lblDiscount);
            this.pnlCartFooter.Controls.Add(this.lblTotalLabel);
            this.pnlCartFooter.Controls.Add(this.lblTotal);
            this.pnlCartFooter.Controls.Add(div1);
            this.pnlCartFooter.Controls.Add(this.lblPromotion);
            this.pnlCartFooter.Controls.Add(this.cmbPromotion);
            this.pnlCartFooter.Controls.Add(this.btnApplyPromo);
            this.pnlCartFooter.Controls.Add(this.btnClearPromo);
            this.pnlCartFooter.Controls.Add(div2);
            this.pnlCartFooter.Controls.Add(this.lblPointsBalance);
            this.pnlCartFooter.Controls.Add(this.lblRedeemPoints);
            this.pnlCartFooter.Controls.Add(this.numRedeemPoints);
            this.pnlCartFooter.Controls.Add(this.btnRedeemPoints);
            this.pnlCartFooter.Controls.Add(this.btnClearPoints);
            this.pnlCartFooter.Controls.Add(div3);
            this.pnlCartFooter.Controls.Add(this.lblPaymentSection);
            this.pnlCartFooter.Controls.Add(this.lblAmountPaid);
            this.pnlCartFooter.Controls.Add(this.numAmountPaid);
            this.pnlCartFooter.Controls.Add(this.lblPaymentMethod);
            this.pnlCartFooter.Controls.Add(this.cmbPaymentMethod);
            this.pnlCartFooter.Controls.Add(this.lblReference);
            this.pnlCartFooter.Controls.Add(this.txtReference);
            this.pnlCartFooter.Controls.Add(this.lblChangeLabel);
            this.pnlCartFooter.Controls.Add(this.lblChange);
            this.pnlCartFooter.Controls.Add(this.btnPay);
            this.pnlCartFooter.Controls.Add(this.lblStatus);

            // Add cart pieces
            this.pnlCart.Controls.Add(this.gridCart);
            this.pnlCart.Controls.Add(this.pnlCartFooter);
            this.pnlCart.Controls.Add(this.pnlCartHeader);

            // ============================================================
            // CATALOG (left side)
            // ============================================================
            this.pnlCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCatalog.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlCatalog.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);

            // Catalog header
            this.pnlCatalogHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCatalogHeader.Height = 96;
            this.pnlCatalogHeader.BackColor = System.Drawing.Color.White;
            this.pnlCatalogHeader.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);

            this.lblCatalogTitle.AutoSize = true;
            this.lblCatalogTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblCatalogTitle.ForeColor = System.Drawing.Color.FromArgb(25, 35, 50);
            this.lblCatalogTitle.Location = new System.Drawing.Point(20, 14);
            this.lblCatalogTitle.Text = "Products";

            this.lblCatalogSubtitle.AutoSize = true;
            this.lblCatalogSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCatalogSubtitle.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblCatalogSubtitle.Location = new System.Drawing.Point(21, 42);
            this.lblCatalogSubtitle.Text = "Select a product to add it to the order";

            this.txtProductSearch.Location = new System.Drawing.Point(320, 28);
            this.txtProductSearch.Size = new System.Drawing.Size(280, 29);
            this.txtProductSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProductSearch.PlaceholderText = "Search products...";

            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategoryFilter.Location = new System.Drawing.Point(615, 28);
            this.cmbCategoryFilter.Size = new System.Drawing.Size(190, 29);

            this.lblCatalogEmpty.AutoSize = true;
            this.lblCatalogEmpty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCatalogEmpty.ForeColor = System.Drawing.Color.FromArgb(110, 120, 135);
            this.lblCatalogEmpty.Location = new System.Drawing.Point(20, 72);
            this.lblCatalogEmpty.Text = "No products match the filter.";
            this.lblCatalogEmpty.Visible = false;

            this.pnlCatalogHeader.Controls.Add(this.lblCatalogTitle);
            this.pnlCatalogHeader.Controls.Add(this.lblCatalogSubtitle);
            this.pnlCatalogHeader.Controls.Add(this.txtProductSearch);
            this.pnlCatalogHeader.Controls.Add(this.cmbCategoryFilter);
            this.pnlCatalogHeader.Controls.Add(this.lblCatalogEmpty);

            // Product tiles
            this.flowProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowProducts.AutoScroll = true;
            this.flowProducts.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowProducts.WrapContents = true;
            this.flowProducts.Padding = new System.Windows.Forms.Padding(16);
            this.flowProducts.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            this.pnlCatalog.Controls.Add(this.flowProducts);
            this.pnlCatalog.Controls.Add(this.pnlCatalogHeader);

            // ============================================================
            // Add to form
            // ============================================================
            this.Controls.Add(this.pnlCatalog);
            this.Controls.Add(this.pnlCart);
            this.Controls.Add(this.pnlHeader);

            // ============================================================
            // Resume
            // ============================================================
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCatalogHeader.ResumeLayout(false);
            this.pnlCatalogHeader.PerformLayout();
            this.pnlCatalog.ResumeLayout(false);
            this.pnlCartHeader.ResumeLayout(false);
            this.pnlCartHeader.PerformLayout();
            this.pnlCartFooter.ResumeLayout(false);
            this.pnlCartFooter.PerformLayout();
            this.pnlCart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountPaid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRedeemPoints)).EndInit();
            this.ResumeLayout(false);
        }
    }
}