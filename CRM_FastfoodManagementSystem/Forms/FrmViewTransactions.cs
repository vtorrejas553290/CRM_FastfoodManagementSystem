using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmViewTransactions : Form
{
    // ---- Pagination state ----
    private List<TxnRow> _allRows = new();
    private int _currentPage = 1;

    /// <summary>
    /// Snapshot row type. Concrete class because anonymous types can't be
    /// stored in a field.
    /// </summary>
    private class TxnRow
    {
        public int TransactionId { get; set; }
        public string OrderCode { get; set; } = "";
        public string Customer { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public decimal Total { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ChangeDue { get; set; }
        public string Reference { get; set; } = "";
        public DateTime PaidAt { get; set; }
    }

    public FrmViewTransactions()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridTransactions,
            ("colView", "primary"));

        Load += (_, __) =>
        {
            InitFilters();
            InitPager();
            LoadTransactions();
        };

        btnRefresh.Click += (_, __) => LoadTransactions();

        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadTransactions();
        };

        cmbMethodFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadTransactions();
        };

        cmbDateFilter.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadTransactions();
        };

        // Pagination events
        cmbPageSize.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            RenderPage();
        };
        btnFirstPage.Click += (_, __) => GoToPage(1);
        btnPrevPage.Click += (_, __) => GoToPage(_currentPage - 1);
        btnNextPage.Click += (_, __) => GoToPage(_currentPage + 1);
        btnLastPage.Click += (_, __) => GoToPage(TotalPages);
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlTop.BackColor = AppTheme.Surface;
        pnlGridWrap.BackColor = AppTheme.ContentSurface;
        pnlPager.BackColor = AppTheme.Surface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleInput(cmbMethodFilter);
        AppTheme.StyleInput(cmbDateFilter);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleGrid(gridTransactions);

        AppTheme.StyleLabel(lblMethodFilter);
        AppTheme.StyleLabel(lblDateFilter);

        lblTotal.Font = AppTheme.FontSubheading;
        lblTotal.ForeColor = AppTheme.SuccessGreen;

        // Pager styling
        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);
    }

    private void InitFilters()
    {
        cmbMethodFilter.Items.Clear();
        cmbMethodFilter.Items.AddRange(new object[] { "All", "Cash", "GCash" });
        cmbMethodFilter.SelectedIndex = 0;

        cmbDateFilter.Items.Clear();
        cmbDateFilter.Items.AddRange(new object[] { "All", "Today", "Last 7 Days", "Last 30 Days" });
        cmbDateFilter.SelectedIndex = 0;
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;   // default 25
    }

    // ============================================================
    //  DATA LOAD
    // ============================================================

    private void LoadTransactions()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var search = txtSearch.Text.Trim().ToLower();
            var method = cmbMethodFilter.SelectedItem?.ToString() ?? "All";
            var dateRange = cmbDateFilter.SelectedItem?.ToString() ?? "All";

            var query = db.Transactions
                .Include(x => x.Order!)
                    .ThenInclude(o => o.Customer)
                .AsNoTracking();

            if (method != "All")
                query = query.Where(x => x.PaymentMethod == method);

            var now = DateTime.UtcNow;

            if (dateRange == "Today")
                query = query.Where(x => x.PaidAt.Date == now.Date);
            else if (dateRange == "Last 7 Days")
                query = query.Where(x => x.PaidAt >= now.AddDays(-7));
            else if (dateRange == "Last 30 Days")
                query = query.Where(x => x.PaidAt >= now.AddDays(-30));

            _allRows = query
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    (x.Order != null && x.Order.OrderCode.ToLower().Contains(search)) ||
                    (x.Order!.Customer != null && x.Order.Customer.CustomerName.ToLower().Contains(search)))
                .OrderByDescending(x => x.PaidAt)
                .Select(x => new TxnRow
                {
                    TransactionId = x.TransactionId,
                    OrderCode = x.Order!.OrderCode,
                    Customer = x.Order.Customer != null ? x.Order.Customer.CustomerName : "(deleted)",
                    PaymentMethod = x.PaymentMethod,
                    Total = x.Order.TotalAmount,
                    AmountPaid = x.AmountPaid,
                    ChangeDue = x.ChangeDue,
                    Reference = x.ReferenceNumber ?? "-",
                    PaidAt = x.PaidAt
                })
                .ToList();

            // Total sales across the full filtered set (not just the page)
            decimal totalSales = _allRows.Sum(x => x.Total);
            lblTotal.Text = $"Total Sales: ₱{totalSales:N2}  ({_allRows.Count} transactions)";

            // Clamp page if filters shrank the set
            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ============================================================
    //  PAGINATION
    // ============================================================

    private int PageSize
    {
        get
        {
            var s = cmbPageSize.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(s) || s == "All") return int.MaxValue;
            return int.TryParse(s, out int n) && n > 0 ? n : 25;
        }
    }

    private int TotalPages
    {
        get
        {
            if (PageSize == int.MaxValue) return 1;
            return Math.Max(1, (int)Math.Ceiling(_allRows.Count / (double)PageSize));
        }
    }

    private void GoToPage(int page)
    {
        int target = Math.Clamp(page, 1, TotalPages);
        if (target == _currentPage) return;

        _currentPage = target;
        RenderPage();
    }

    private void RenderPage()
    {
        int size = PageSize;
        List<TxnRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridTransactions.DataSource = pageRows.Select(x => new
        {
            x.TransactionId,
            x.OrderCode,
            x.Customer,
            x.PaymentMethod,
            x.Total,
            x.AmountPaid,
            x.ChangeDue,
            x.Reference,
            x.PaidAt
        }).ToList();

        BuildActionColumns();

        int total = TotalPages;
        lblPageInfo.Text = $"Page {_currentPage} of {total}";

        int first = _allRows.Count == 0
            ? 0
            : ((_currentPage - 1) * (size == int.MaxValue ? _allRows.Count : size)) + 1;
        int last = size == int.MaxValue
            ? _allRows.Count
            : Math.Min(_currentPage * size, _allRows.Count);
        lblShowing.Text = $"Showing {first}–{last} of {_allRows.Count}";

        bool multiPage = total > 1;
        btnFirstPage.Enabled = multiPage && _currentPage > 1;
        btnPrevPage.Enabled = multiPage && _currentPage > 1;
        btnNextPage.Enabled = multiPage && _currentPage < total;
        btnLastPage.Enabled = multiPage && _currentPage < total;
    }

    // ============================================================
    //  ACTION COLUMNS
    // ============================================================

    private void BuildActionColumns()
    {
        for (int i = gridTransactions.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridTransactions.Columns[i];
            if (col.Name == "colView")
                gridTransactions.Columns.RemoveAt(i);
        }

        gridTransactions.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colView", "View", "View", "primary", 100));

        AppTheme.ApplyStyleToButtonColumn(gridTransactions, "colView", "primary");

        gridTransactions.CellContentClick -= GridTransactions_CellContentClick;
        gridTransactions.CellContentClick += GridTransactions_CellContentClick;
    }

    private void GridTransactions_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridTransactions.Columns[e.ColumnIndex];
        if (column.Name != "colView") return;

        var idCell = gridTransactions.Rows[e.RowIndex].Cells["TransactionId"];
        if (idCell?.Value is null) return;

        int transactionId = Convert.ToInt32(idCell.Value);

        try
        {
            using var db = AppServices.CreateTenantContext();

            var transaction = db.Transactions
                .AsNoTracking()
                .FirstOrDefault(x => x.TransactionId == transactionId);

            if (transaction is null)
            {
                MessageBox.Show("Transaction not found.", "Error");
                return;
            }

            using var dlg = new FrmOrderDetail(transaction.OrderId);
            dlg.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}