using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmViewTransactions : Form
{
    private readonly ITransactionController _controller = new TransactionController();

    private List<TransactionRow> _allRows = new();
    private int _currentPage = 1;

    private bool _syncingDates = false;

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
            if (_syncingDates) return;

            _syncingDates = true;
            SyncDatePickersFromRange();
            _syncingDates = false;

            _currentPage = 1;
            LoadTransactions();
        };

        dtpFromDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateFilter.SelectedItem?.ToString() != "Custom")
                cmbDateFilter.SelectedItem = "Custom";
            _syncingDates = false;

            _currentPage = 1;
            LoadTransactions();
        };

        dtpToDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateFilter.SelectedItem?.ToString() != "Custom")
                cmbDateFilter.SelectedItem = "Custom";
            _syncingDates = false;

            _currentPage = 1;
            LoadTransactions();
        };

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
        AppTheme.StyleGrid(gridTransactions);

        AppTheme.StyleLabel(lblMethodFilter);
        AppTheme.StyleLabel(lblDateFilter);
        AppTheme.StyleLabel(lblFromDate);
        AppTheme.StyleLabel(lblToDate);

        lblTotal.Font = AppTheme.FontSubheading;
        lblTotal.ForeColor = AppTheme.SuccessGreen;

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
        cmbDateFilter.Items.AddRange(new object[]
        {
            "All", "Today", "Last 7 Days", "Last 30 Days", "Custom"
        });

        _syncingDates = true;
        cmbDateFilter.SelectedIndex = 0;   // "All"
        SyncDatePickersFromRange();
        _syncingDates = false;
    }

    private void SyncDatePickersFromRange()
    {
        var now = DateTime.UtcNow;
        var sel = cmbDateFilter.SelectedItem?.ToString() ?? "All";

        switch (sel)
        {
            case "Today":
                dtpFromDate.Value = now.Date;
                dtpToDate.Value = now.Date;
                break;
            case "Last 7 Days":
                dtpFromDate.Value = now.AddDays(-7).Date;
                dtpToDate.Value = now.Date;
                break;
            case "Last 30 Days":
                dtpFromDate.Value = now.AddDays(-30).Date;
                dtpToDate.Value = now.Date;
                break;
            case "All":
            case "Custom":
                // leave current picker values alone
                break;
        }
    }

    private (DateTime? from, DateTime? to) GetFromTo()
    {
        var sel = cmbDateFilter.SelectedItem?.ToString() ?? "All";

        if (sel == "All")
            return (null, null);

        DateTime from = dtpFromDate.Value.Date;
        DateTime to = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
        return (from, to);
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    private void LoadTransactions()
    {
        try
        {
            var (from, to) = GetFromTo();

            var filter = new TransactionFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                Method = cmbMethodFilter.SelectedItem?.ToString() ?? "All",
                DateRange = cmbDateFilter.SelectedItem?.ToString() ?? "All",
                FromDate = from,
                ToDate = to
            };

            _allRows = _controller.GetTransactions(filter);

            decimal totalSales = _allRows.Sum(x => x.Total);
            lblTotal.Text = $"Total Sales: ₱{totalSales:N2}  ({_allRows.Count} transactions)";

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

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
        List<TransactionRow> pageRows;

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
        StyleColumns();

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

    private void StyleColumns()
    {
        var grid = gridTransactions;

        if (grid.Columns.Contains("TransactionId")) grid.Columns["TransactionId"].Visible = false;

        SetColumnFixed("OrderCode", "Order Code", 130, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("PaymentMethod", "Method", 110, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("Total", "Total", 120, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("AmountPaid", "Paid", 120, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("ChangeDue", "Change", 110, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("PaidAt", "Paid At", 160, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("Customer", "Customer", 100, 150, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFill("Reference", "Reference", 40, 120, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("Total")) grid.Columns["Total"].DefaultCellStyle.Format = "N2";
        if (grid.Columns.Contains("AmountPaid")) grid.Columns["AmountPaid"].DefaultCellStyle.Format = "N2";
        if (grid.Columns.Contains("ChangeDue")) grid.Columns["ChangeDue"].DefaultCellStyle.Format = "N2";
        if (grid.Columns.Contains("PaidAt")) grid.Columns["PaidAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

        foreach (var name in new[] { "colView" })
        {
            if (!grid.Columns.Contains(name)) continue;
            var c = grid.Columns[name];
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Resizable = DataGridViewTriState.False;
            c.DefaultCellStyle.Padding = new Padding(0);
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            c.DefaultCellStyle.ForeColor = AppTheme.TextOnDark;
            c.DefaultCellStyle.SelectionForeColor = AppTheme.TextOnDark;
            c.HeaderCell.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c.MinimumWidth = c.Width;
        }

        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        grid.RowTemplate.Height = 32;
    }

    private void SetColumnFixed(string name, string header, int width, DataGridViewContentAlignment align)
    {
        if (!gridTransactions.Columns.Contains(name)) return;
        var c = gridTransactions.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridTransactions.Columns.Contains(name)) return;
        var c = gridTransactions.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
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