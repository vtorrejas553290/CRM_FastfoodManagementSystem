using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmOrderManagement : Form
{
    private const int PointsToPesoDivisor = 100;

    private readonly List<OrderItem> _cart = new();
    private List<Product> _allProducts = new();

    private decimal _appliedDiscount = 0m;
    private int? _appliedPromotionId = null;

    private int _customerPointsBalance = 0;
    private int _redeemedPoints = 0;

    public FrmOrderManagement()
    {
        InitializeComponent();
        ApplyTheme();

        Load += FrmOrderManagement_Load;
        btnPay.Click += BtnPay_Click;
        btnApplyPromo.Click += (_, __) => ApplyPromotion();
        btnClearPromo.Click += (_, __) => ClearPromotion();
        btnRedeemPoints.Click += (_, __) => ApplyPointsRedemption();
        btnClearPoints.Click += (_, __) => ClearPointsRedemption();

        cmbCustomer.SelectedIndexChanged += (_, __) => RefreshCustomerPoints();

        cmbCustomer.TextChanged += (_, __) =>
        {
            if (cmbCustomer.SelectedValue is null
                && !string.IsNullOrWhiteSpace(cmbCustomer.Text))
            {
                lblStatus.ForeColor = AppTheme.Warning;
                lblStatus.Text = $"No customer named \"{cmbCustomer.Text}\" — pick one from the list.";
            }
            else
            {
                lblStatus.Text = string.Empty;
            }
        };

        txtProductSearch.TextChanged += (_, __) => RenderProductTiles();
        cmbCategoryFilter.SelectedIndexChanged += (_, __) => RenderProductTiles();
        numAmountPaid.ValueChanged += (_, __) => UpdateChange();
        cmbPaymentMethod.SelectedIndexChanged += (_, __) => UpdatePaymentUi();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);

        pnlHeader.BackColor = AppTheme.Surface;
        lblHeaderTitle.Font = AppTheme.FontHeading;
        lblHeaderTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleInput(cmbCustomer);
        AppTheme.StyleInput(cmbOrderType);
        AppTheme.StyleLabel(lblCustomer);
        AppTheme.StyleLabel(lblOrderType);

        pnlCatalog.BackColor = AppTheme.ContentSurface;
        pnlCatalogHeader.BackColor = AppTheme.Surface;
        lblCatalogTitle.Font = AppTheme.FontSubheading;
        lblCatalogTitle.ForeColor = AppTheme.TextPrimary;
        AppTheme.StyleInput(txtProductSearch);
        AppTheme.StyleInput(cmbCategoryFilter);
        AppTheme.StyleLabel(lblCatalogEmpty);

        pnlCart.BackColor = AppTheme.Surface;
        pnlCartHeader.BackColor = AppTheme.Surface;
        pnlCartFooter.BackColor = AppTheme.Surface;

        lblCartTitle.Font = AppTheme.FontSubheading;
        lblCartTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleGrid(gridCart);

        lblSubTotal.Font = AppTheme.FontBody;
        lblSubTotal.ForeColor = AppTheme.TextSecondary;

        lblDiscount.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
        lblDiscount.ForeColor = AppTheme.SuccessGreen;

        lblTotalLabel.Font = AppTheme.FontSubheading;
        lblTotalLabel.ForeColor = AppTheme.TextPrimary;

        lblTotal.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTotal.ForeColor = AppTheme.SuccessGreen;

        AppTheme.StyleLabel(lblPromotion);
        AppTheme.StyleInput(cmbPromotion);
        AppTheme.StylePrimaryButton(btnApplyPromo);
        AppTheme.StyleSecondaryButton(btnClearPromo);

        lblPointsBalance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPointsBalance.ForeColor = AppTheme.SuccessGreen;
        AppTheme.StyleLabel(lblRedeemPoints);
        AppTheme.StyleInput(numRedeemPoints);
        AppTheme.StylePrimaryButton(btnRedeemPoints);
        AppTheme.StyleSecondaryButton(btnClearPoints);

        AppTheme.StyleLabel(lblAmountPaid);
        AppTheme.StyleInput(numAmountPaid);

        AppTheme.StyleLabel(lblPaymentMethod);
        AppTheme.StyleInput(cmbPaymentMethod);

        AppTheme.StyleLabel(lblReference);
        AppTheme.StyleInput(txtReference);

        AppTheme.StyleLabel(lblChangeLabel);
        lblChange.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblChange.ForeColor = AppTheme.SuccessGreen;

        AppTheme.StyleSuccessButton(btnPay);
        btnPay.Height = 55;

        AppTheme.StyleLabel(lblStatus);
    }

    private void FrmOrderManagement_Load(object? sender, EventArgs e)
    {
        LoadCustomers();
        LoadCategories();
        LoadProducts();
        LoadActivePromotions();

        cmbOrderType.Items.Clear();
        cmbOrderType.Items.AddRange(new object[] { "DineIn", "TakeOut" });
        cmbOrderType.SelectedIndex = 0;

        cmbPaymentMethod.Items.Clear();
        cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "GCash" });
        cmbPaymentMethod.SelectedIndex = 0;

        UpdatePaymentUi();
        RenderProductTiles();
        RefreshCartGrid();
        RefreshCustomerPoints();
        UpdateTotals();
    }

    private void LoadCustomers()
    {
        using var db = AppServices.CreateTenantContext();
        var customers = db.Customers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.CustomerName)
            .Select(x => new { x.CustomerId, x.CustomerName })
            .ToList();

        cmbCustomer.DisplayMember = "CustomerName";
        cmbCustomer.ValueMember = "CustomerId";
        cmbCustomer.DataSource = customers;

        cmbCustomer.SelectedIndex = -1;
    }

    private void LoadCategories()
    {
        using var db = AppServices.CreateTenantContext();
        var cats = db.Categories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.CategoryName)
            .Select(x => new { x.CategoryId, x.CategoryName })
            .ToList();

        var items = new List<object> { new { CategoryId = 0, CategoryName = "All" } };
        items.AddRange(cats);

        cmbCategoryFilter.DataSource = items;
        cmbCategoryFilter.DisplayMember = "CategoryName";
        cmbCategoryFilter.ValueMember = "CategoryId";
    }

    private void LoadProducts()
    {
        using var db = AppServices.CreateTenantContext();
        _allProducts = db.Products
            .Include(x => x.Category)
            .Include(x => x.Inventory)
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.ProductName)
            .ToList();
    }

    private void LoadActivePromotions()
    {
        using var db = AppServices.CreateTenantContext();
        var now = DateTime.UtcNow;

        var promos = db.Promotions
            .AsNoTracking()
            .Where(x => x.IsActive && x.StartDate <= now && x.EndDate >= now)
            .OrderBy(x => x.PromotionName)
            .Select(x => new
            {
                x.PromotionId,
                Display = x.PromotionCode + " — " + x.PromotionName
            })
            .ToList();

        var items = new List<object> { new { PromotionId = 0, Display = "(none)" } };
        items.AddRange(promos);

        cmbPromotion.DataSource = items;
        cmbPromotion.DisplayMember = "Display";
        cmbPromotion.ValueMember = "PromotionId";
    }

    // ============================================================
    //  LOYALTY POINTS
    // ============================================================

    private void RefreshCustomerPoints()
    {
        if (cmbCustomer.SelectedValue is not int customerId || customerId == 0)
        {
            _customerPointsBalance = 0;
            _redeemedPoints = 0;
            lblPointsBalance.Text = "Loyalty Points: —";
            numRedeemPoints.Value = 0;
            UpdateTotals();
            return;
        }

        using var db = AppServices.CreateTenantContext();
        var customer = db.Customers.AsNoTracking().FirstOrDefault(x => x.CustomerId == customerId);
        _customerPointsBalance = customer?.CurrentPoints ?? 0;

        lblPointsBalance.Text = $"Loyalty Points: {_customerPointsBalance:N0}  (= ₱{_customerPointsBalance / (decimal)PointsToPesoDivisor:N2})";

        _redeemedPoints = 0;
        numRedeemPoints.Value = 0;
        UpdateTotals();
    }

    private void ApplyPointsRedemption()
    {
        if (_customerPointsBalance <= 0)
        {
            MessageBox.Show("This customer has no points to redeem.", "No points");
            return;
        }

        int requested = (int)numRedeemPoints.Value;

        if (requested <= 0)
        {
            MessageBox.Show("Enter a positive number of points.", "Invalid");
            return;
        }

        if (requested > _customerPointsBalance)
        {
            MessageBox.Show(
                $"Customer only has {_customerPointsBalance:N0} points.",
                "Not enough points");
            return;
        }

        decimal cartTotal = _cart.Sum(x => x.LineTotal);
        decimal pointsDiscount = requested / (decimal)PointsToPesoDivisor;
        decimal maxUsableDiscount = cartTotal - _appliedDiscount;

        if (pointsDiscount > maxUsableDiscount)
        {
            MessageBox.Show(
                $"Points value (₱{pointsDiscount:N2}) exceeds the remaining amount to pay (₱{maxUsableDiscount:N2}).",
                "Too many points");
            return;
        }

        _redeemedPoints = requested;
        UpdateTotals();

        lblStatus.ForeColor = AppTheme.Success;
        lblStatus.Text = $"Redeeming {requested:N0} points = ₱{pointsDiscount:N2} discount.";
    }

    private void ClearPointsRedemption()
    {
        _redeemedPoints = 0;
        numRedeemPoints.Value = 0;
        UpdateTotals();
        lblStatus.Text = "Points redemption cleared.";
        lblStatus.ForeColor = AppTheme.TextSecondary;
    }

    // ============================================================
    //  PROMOTIONS
    // ============================================================

    private void ApplyPromotion()
    {
        if (cmbPromotion.SelectedValue is not int promoId || promoId == 0)
        {
            _appliedPromotionId = null;
            _appliedDiscount = 0m;
            UpdateTotals();
            return;
        }

        using var db = AppServices.CreateTenantContext();
        var promo = db.Promotions.AsNoTracking().FirstOrDefault(x => x.PromotionId == promoId);
        if (promo is null) return;

        decimal subTotal = _cart.Sum(x => x.LineTotal);

        if (promo.MinimumPurchase.HasValue && subTotal < promo.MinimumPurchase.Value)
        {
            MessageBox.Show(
                $"Minimum purchase for '{promo.PromotionCode}' is ₱{promo.MinimumPurchase:N2}.\n" +
                $"Current subtotal is ₱{subTotal:N2}.",
                "Minimum purchase not met",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        decimal discount = promo.DiscountType == "Percent"
            ? subTotal * (promo.DiscountValue / 100m)
            : promo.DiscountValue;

        if (discount > subTotal) discount = subTotal;

        _appliedPromotionId = promoId;
        _appliedDiscount = discount;

        UpdateTotals();

        lblStatus.ForeColor = AppTheme.Success;
        lblStatus.Text = $"Promo '{promo.PromotionCode}' applied — discount ₱{discount:N2}.";
    }

    private void ClearPromotion()
    {
        _appliedPromotionId = null;
        _appliedDiscount = 0m;
        if (cmbPromotion.Items.Count > 0) cmbPromotion.SelectedIndex = 0;
        UpdateTotals();
        lblStatus.Text = "Promotion cleared.";
        lblStatus.ForeColor = AppTheme.TextSecondary;
    }

    // ============================================================
    //  PRODUCT TILES
    // ============================================================

    private void RenderProductTiles()
    {
        flowProducts.SuspendLayout();
        flowProducts.Controls.Clear();

        var search = txtProductSearch.Text.Trim().ToLower();

        int? selectedCategoryId = null;
        if (cmbCategoryFilter.SelectedValue is int cid && cid > 0)
            selectedCategoryId = cid;

        var filtered = _allProducts
            .Where(p => string.IsNullOrWhiteSpace(search)
                        || p.ProductName.ToLower().Contains(search)
                        || p.ProductCode.ToLower().Contains(search))
            .Where(p => selectedCategoryId is null || p.CategoryId == selectedCategoryId.Value)
            .ToList();

        if (filtered.Count == 0)
        {
            lblCatalogEmpty.Visible = true;
            flowProducts.ResumeLayout();
            return;
        }

        lblCatalogEmpty.Visible = false;

        foreach (var p in filtered)
            flowProducts.Controls.Add(BuildProductTile(p));

        flowProducts.ResumeLayout();
    }

    private Panel BuildProductTile(Product p)
    {
        var tile = new Panel
        {
            Width = 170,
            Height = 110,
            Margin = new Padding(8),
            BackColor = AppTheme.Surface,
            Cursor = Cursors.Hand
        };

        tile.Paint += (s, e) =>
        {
            using var pen = new Pen(AppTheme.Border, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, tile.Width - 1, tile.Height - 1);
        };

        var lblName = new Label
        {
            Text = p.ProductName,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = AppTheme.TextPrimary,
            Location = new Point(12, 12),
            AutoSize = false,
            Size = new Size(146, 24)
        };

        var lblCategory = new Label
        {
            Text = p.Category != null ? p.Category.CategoryName : "—",
            Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
            ForeColor = AppTheme.TextMuted,
            Location = new Point(12, 38),
            AutoSize = true
        };

        var lblPrice = new Label
        {
            Text = $"₱{p.UnitPrice:N2}",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = AppTheme.SuccessGreen,
            Location = new Point(12, 58),
            AutoSize = true
        };

        decimal stock = p.Inventory != null ? p.Inventory.QuantityOnHand : 0;

        var lblStock = new Label
        {
            Text = $"Stock: {stock}",
            Font = new Font("Segoe UI", 8F),
            ForeColor = stock <= 0 ? AppTheme.Danger : AppTheme.TextMuted,
            Location = new Point(12, 86),
            AutoSize = true
        };

        tile.Controls.Add(lblName);
        tile.Controls.Add(lblCategory);
        tile.Controls.Add(lblPrice);
        tile.Controls.Add(lblStock);

        tile.MouseEnter += (_, __) => tile.BackColor = AppTheme.Primary90;
        tile.MouseLeave += (_, __) => tile.BackColor = AppTheme.Surface;

        tile.Click += (_, __) => AddProductToCart(p);
        lblName.Click += (_, __) => AddProductToCart(p);
        lblCategory.Click += (_, __) => AddProductToCart(p);
        lblPrice.Click += (_, __) => AddProductToCart(p);
        lblStock.Click += (_, __) => AddProductToCart(p);

        return tile;
    }

    private void AddProductToCart(Product p)
    {
        decimal stock = p.Inventory != null ? p.Inventory.QuantityOnHand : 0;

        if (stock <= 0)
        {
            MessageBox.Show($"'{p.ProductName}' is out of stock.", "Out of stock");
            return;
        }

        int inCart = _cart.FirstOrDefault(x => x.ProductId == p.ProductId)?.Quantity ?? 0;
        if (inCart + 1 > stock)
        {
            MessageBox.Show($"Not enough stock. Available: {stock}", "Insufficient stock");
            return;
        }

        var existing = _cart.FirstOrDefault(x => x.ProductId == p.ProductId);

        if (existing is not null)
        {
            existing.Quantity += 1;
            existing.LineTotal = existing.Quantity * existing.UnitPrice;
        }
        else
        {
            _cart.Add(new OrderItem
            {
                ProductId = p.ProductId,
                Quantity = 1,
                UnitPrice = p.UnitPrice,
                LineTotal = p.UnitPrice
            });
        }

        RefreshCartGrid();
        UpdateTotals();
    }

    // ============================================================
    //  CART
    // ============================================================

    private void RefreshCartGrid()
    {
        gridCart.DataSource = null;
        gridCart.Columns.Clear();

        var table = new System.Data.DataTable();
        table.Columns.Add("ProductId", typeof(int));
        table.Columns.Add("Item", typeof(string));
        table.Columns.Add("Qty", typeof(int));
        table.Columns.Add("Price", typeof(string));
        table.Columns.Add("Total", typeof(string));

        foreach (var item in _cart)
        {
            var p = _allProducts.FirstOrDefault(pp => pp.ProductId == item.ProductId);
            table.Rows.Add(
                item.ProductId,
                p != null ? p.ProductName : "(unknown)",
                item.Quantity,
                $"₱{item.UnitPrice:N2}",
                $"₱{item.LineTotal:N2}");
        }

        gridCart.DataSource = table;

        if (gridCart.Columns.Contains("ProductId"))
            gridCart.Columns["ProductId"].Visible = false;

        if (gridCart.Columns.Contains("Item"))
        {
            gridCart.Columns["Item"].HeaderText = "Item";
            gridCart.Columns["Item"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridCart.Columns["Item"].FillWeight = 50;
            gridCart.Columns["Item"].MinimumWidth = 160;
        }

        if (gridCart.Columns.Contains("Qty"))
        {
            gridCart.Columns["Qty"].HeaderText = "Qty";
            gridCart.Columns["Qty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridCart.Columns["Qty"].Width = 50;
            gridCart.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        if (gridCart.Columns.Contains("Price"))
        {
            gridCart.Columns["Price"].HeaderText = "Price";
            gridCart.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridCart.Columns["Price"].Width = 80;
            gridCart.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        if (gridCart.Columns.Contains("Total"))
        {
            gridCart.Columns["Total"].HeaderText = "Total";
            gridCart.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridCart.Columns["Total"].Width = 90;
            gridCart.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        for (int i = gridCart.Columns.Count - 1; i >= 0; i--)
        {
            var c = gridCart.Columns[i];
            if (c is DataGridViewButtonColumn)
                gridCart.Columns.RemoveAt(i);
        }

        var colMinus = new DataGridViewButtonColumn
        {
            Name = "colMinus",
            HeaderText = "",
            Text = "−",
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat,
            Width = 40,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            Resizable = DataGridViewTriState.False,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            DefaultCellStyle = AppTheme.GridButtonStyle("danger")
        };
        gridCart.Columns.Add(colMinus);

        var colX = new DataGridViewButtonColumn
        {
            Name = "colRemove",
            HeaderText = "",
            Text = "X",
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat,
            Width = 40,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            Resizable = DataGridViewTriState.False,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            DefaultCellStyle = AppTheme.GridButtonStyle("danger")
        };
        gridCart.Columns.Add(colX);

        gridCart.CellContentClick -= GridCart_CellContentClick;
        gridCart.CellContentClick += GridCart_CellContentClick;

        AppTheme.ApplyStyleToButtonColumn(gridCart, "colMinus", "danger");
        AppTheme.ApplyStyleToButtonColumn(gridCart, "colRemove", "danger");
    }

    private void GridCart_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var col = gridCart.Columns[e.ColumnIndex];
        if (col.Name != "colMinus" && col.Name != "colRemove") return;

        var productIdCell = gridCart.Rows[e.RowIndex].Cells["ProductId"];
        if (productIdCell?.Value is null) return;

        int productId = Convert.ToInt32(productIdCell.Value);
        var item = _cart.FirstOrDefault(x => x.ProductId == productId);
        if (item is null) return;

        if (col.Name == "colRemove")
        {
            _cart.Remove(item);
        }
        else
        {
            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
                item.LineTotal = item.Quantity * item.UnitPrice;
            }
            else
            {
                _cart.Remove(item);
            }
        }

        RefreshCartGrid();
        UpdateTotals();
    }

    // ============================================================
    //  TOTALS
    // ============================================================

    private void UpdateTotals()
    {
        decimal subTotal = _cart.Sum(x => x.LineTotal);
        decimal pointsDiscount = _redeemedPoints / (decimal)PointsToPesoDivisor;
        decimal totalDiscount = _appliedDiscount + pointsDiscount;
        decimal total = subTotal - totalDiscount;

        if (total < 0) total = 0;

        lblSubTotal.Text = $"SubTotal: ₱{subTotal:N2}";

        if (totalDiscount > 0)
        {
            lblDiscount.Text = $"Discount: -₱{totalDiscount:N2}";
            lblDiscount.Visible = true;
        }
        else
        {
            lblDiscount.Visible = false;
        }

        lblTotal.Text = $"₱{total:N2}";
        UpdateChange();
    }

    private void UpdatePaymentUi()
    {
        var method = cmbPaymentMethod.SelectedItem?.ToString() ?? "Cash";
        bool isGcash = method == "GCash";

        lblReference.Visible = isGcash;
        txtReference.Visible = isGcash;
    }

    private void UpdateChange()
    {
        decimal subTotal = _cart.Sum(x => x.LineTotal);
        decimal pointsDiscount = _redeemedPoints / (decimal)PointsToPesoDivisor;
        decimal total = subTotal - _appliedDiscount - pointsDiscount;
        if (total < 0) total = 0;

        decimal paid = numAmountPaid.Value;
        decimal change = paid - total;

        if (change < 0)
        {
            lblChange.Text = $"Short ₱{Math.Abs(change):N2}";
            lblChange.ForeColor = AppTheme.Danger;
        }
        else
        {
            lblChange.Text = $"₱{change:N2}";
            lblChange.ForeColor = AppTheme.SuccessGreen;
        }
    }

    // ============================================================
    //  RESET (called after every successful pay)
    // ============================================================

    private void ResetOrder()
    {
        _cart.Clear();
        _appliedDiscount = 0m;
        _appliedPromotionId = null;
        _redeemedPoints = 0;
        numRedeemPoints.Value = 0;
        numAmountPaid.Value = 0;
        txtReference.Clear();
        cmbPaymentMethod.SelectedIndex = 0;
        if (cmbPromotion.Items.Count > 0) cmbPromotion.SelectedIndex = 0;
        lblStatus.Text = string.Empty;
        RefreshCartGrid();
        UpdateTotals();
        LoadProducts();
        LoadActivePromotions();
        RenderProductTiles();
        RefreshCustomerPoints();
    }

    // ============================================================
    //  PAY
    // ============================================================

    private void BtnPay_Click(object? sender, EventArgs e)
    {
        lblStatus.ForeColor = AppTheme.Error;
        lblStatus.Text = string.Empty;

        if (_cart.Count == 0)
        {
            lblStatus.Text = "Cart is empty. Click a product tile to add items.";
            return;
        }

        if (cmbCustomer.SelectedValue is not int customerId || customerId == 0)
        {
            lblStatus.Text = "Please select a customer from the dropdown.";
            return;
        }

        decimal subTotal = _cart.Sum(x => x.LineTotal);
        decimal pointsDiscount = _redeemedPoints / (decimal)PointsToPesoDivisor;
        decimal totalDiscount = _appliedDiscount + pointsDiscount;
        decimal total = subTotal - totalDiscount;
        if (total < 0) total = 0;

        decimal paid = numAmountPaid.Value;
        string method = cmbPaymentMethod.SelectedItem?.ToString() ?? "Cash";

        if (paid < total)
        {
            lblStatus.Text = "Amount paid cannot be less than the total.";
            return;
        }

        if (method == "GCash" && string.IsNullOrWhiteSpace(txtReference.Text))
        {
            lblStatus.Text = "GCash Reference Number is required.";
            return;
        }

        int pointsEarned = (int)Math.Floor(total);

        string customerName = cmbCustomer.Text;
        string orderType = cmbOrderType.SelectedItem?.ToString() ?? "DineIn";
        string reference = txtReference.Text.Trim();
        decimal change = paid - total;
        int redeemedPoints = _redeemedPoints;

        using var confirmDlg = new FrmOrderConfirmDialog(
            customerName,
            orderType,
            _cart.ToList(),
            total,
            method,
            paid,
            change,
            reference);

        if (confirmDlg.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            using var db = AppServices.CreateTenantContext();

            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer is null)
            {
                lblStatus.Text = "Customer no longer exists.";
                return;
            }

            var order = new Order
            {
                OrderCode = $"ORD-{DateTime.Now:yyyyMMddHHmmss}",
                CustomerId = customerId,
                StaffUserId = UserSession.UserId,
                OrderType = orderType,
                SubTotal = subTotal,
                DiscountAmount = totalDiscount,
                PointsRedeemed = redeemedPoints,
                PointsEarned = pointsEarned,
                TotalAmount = total,
                Status = "Paid",
                OrderDate = DateTime.UtcNow
            };

            db.Orders.Add(order);
            db.SaveChanges();

            foreach (var item in _cart)
            {
                item.OrderId = order.OrderId;
                db.OrderItems.Add(item);

                var inv = db.Inventories.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (inv is not null)
                {
                    inv.QuantityOnHand -= item.Quantity;
                    inv.LastUpdatedAt = DateTime.UtcNow;
                }
            }

            if (_appliedPromotionId.HasValue && _appliedDiscount > 0)
            {
                db.PromotionRedemptions.Add(new PromotionRedemption
                {
                    PromotionId = _appliedPromotionId.Value,
                    CustomerId = customerId,
                    OrderId = order.OrderId,
                    DiscountApplied = _appliedDiscount,
                    RedeemedAt = DateTime.UtcNow
                });
            }

            if (redeemedPoints > 0)
            {
                db.CustomerPoints.Add(new CustomerPoint
                {
                    CustomerId = customerId,
                    TransactionType = "Redeemed",
                    Points = -redeemedPoints,
                    OrderId = order.OrderId,
                    Notes = $"Redeemed on {order.OrderCode}",
                    PerformedByUserId = UserSession.UserId,
                    PerformedAt = DateTime.UtcNow
                });
            }

            if (pointsEarned > 0)
            {
                db.CustomerPoints.Add(new CustomerPoint
                {
                    CustomerId = customerId,
                    TransactionType = "Earned",
                    Points = pointsEarned,
                    OrderId = order.OrderId,
                    Notes = $"Earned from {order.OrderCode}",
                    PerformedByUserId = UserSession.UserId,
                    PerformedAt = DateTime.UtcNow
                });
            }

            customer.CurrentPoints += pointsEarned - redeemedPoints;

            db.Transactions.Add(new Transaction
            {
                OrderId = order.OrderId,
                PaymentMethod = method,
                AmountPaid = paid,
                ChangeDue = change,
                ReferenceNumber = method == "GCash" ? reference : null,
                PaidAt = DateTime.UtcNow
            });

            db.SaveChanges();

            ActivityLogger.Log("Order", "Order", order.OrderId,
                $"Order {order.OrderCode} created — ₱{total:N2}");

            ActivityLogger.Log("Payment", "Transaction", order.OrderId,
                $"{method} payment of ₱{paid:N2} for {order.OrderCode}");

            if (redeemedPoints > 0)
            {
                ActivityLogger.Log("Points", "Customer", customerId,
                    $"Redeemed {redeemedPoints} points on {order.OrderCode}");
            }

            if (pointsEarned > 0)
            {
                ActivityLogger.Log("Points", "Customer", customerId,
                    $"Earned {pointsEarned} points on {order.OrderCode}");
            }

            // Show the receipt dialog before resetting the form.
            var receiptItems = _cart
                .Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    LineTotal = x.LineTotal,
                    Product = _allProducts.FirstOrDefault(p => p.ProductId == x.ProductId)
                })
                .ToList();

            using var receipt = new FrmReceipt(
                orderCode: order.OrderCode,
                customerName: customerName,
                orderType: orderType,
                paymentMethod: method,
                reference: reference,
                subTotal: subTotal,
                discount: totalDiscount,
                total: total,
                paid: paid,
                change: change,
                pointsRedeemed: redeemedPoints,
                pointsEarned: pointsEarned,
                newPointsBalance: customer.CurrentPoints,
                items: receiptItems,
                paidAt: order.OrderDate);

            receipt.ShowDialog(this);

            ResetOrder();
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }
}