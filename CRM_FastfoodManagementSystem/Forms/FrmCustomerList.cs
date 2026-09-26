using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmCustomerList : Form
{
    private readonly ICustomerController _controller = new CustomerController();

    private readonly bool _canEdit;
    private readonly bool _canRegister;

    private List<CustomerRow> _allRows = new();
    private int _currentPage = 1;

    public FrmCustomerList() : this(canEdit: true, canRegister: true) { }

    public FrmCustomerList(bool canEdit)
        : this(canEdit, canRegister: canEdit) { }

    public FrmCustomerList(bool canEdit, bool canRegister)
    {
        _canEdit = canEdit;
        _canRegister = canRegister;

        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridCustomers,
            ("colEdit", "warning"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) =>
        {
            InitPager();
            LoadCustomers();
        };

        btnRegisterCustomer.Click += BtnRegisterCustomer_Click;
        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadCustomers();
        };
        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadCustomers();
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
        pnlPager.BackColor = AppTheme.Surface;

        AppTheme.StyleSuccessButton(btnRegisterCustomer);
        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridCustomers);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);

        btnRegisterCustomer.Visible = _canRegister;
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    private void LoadCustomers()
    {
        try
        {
            var filter = new CustomerFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                ShowArchived = chkShowArchived.Checked
            };

            _allRows = _controller.GetCustomers(filter);

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            int count = _allRows.Count;
            lblStatus.Text = _canEdit
                ? $"{count} customer(s) — use the row buttons to Edit or Archive/Unarchive."
                : _canRegister
                    ? $"{count} customer(s) displayed — you can register new customers."
                    : $"{count} customer(s) displayed (view only).";

            lblStatus.ForeColor = AppTheme.TextSecondary;
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
        List<CustomerRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridCustomers.DataSource = pageRows;

        if (_canEdit)
            BuildActionColumns();
        else
            RemoveActionColumns();

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

    // ============================================================
    //  ACTION COLUMNS
    // ============================================================

    private void BuildActionColumns()
    {
        RemoveActionColumns();

        gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEdit", "Edit", "Edit", "warning", 90));

        if (chkShowArchived.Checked)
        {
            gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 120));

            AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colUnarchive", "success");
        }
        else
        {
            gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 100));

            AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colEdit", "warning");

        gridCustomers.CellContentClick -= GridCustomers_CellContentClick;
        gridCustomers.CellContentClick += GridCustomers_CellContentClick;
    }

    private void RemoveActionColumns()
    {
        for (int i = gridCustomers.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridCustomers.Columns[i];
            if (col.Name == "colEdit" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridCustomers.Columns.RemoveAt(i);
        }
    }

    private void StyleColumns()
    {
        var grid = gridCustomers;

        if (grid.Columns.Contains("CustomerId")) grid.Columns["CustomerId"].Visible = false;

        SetColumnFixed("CustomerCode", "Code", 110, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("ContactNumber", "Contact", 140, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("EmailAddress", "Email", 200, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Status", "Status", 100, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("CreatedAt", "Created", 130, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("CustomerName", "Customer", 60, 160, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFill("Address", "Address", 40, 180, DataGridViewContentAlignment.MiddleLeft);

        if (grid.Columns.Contains("CreatedAt"))
            grid.Columns["CreatedAt"].DefaultCellStyle.Format = "yyyy-MM-dd";

        foreach (var name in new[] { "colEdit", "colArchive", "colUnarchive" })
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

        grid.CellFormatting -= Grid_CellFormatting;
        grid.CellFormatting += Grid_CellFormatting;
    }

    private void SetColumnFixed(string name, string header, int width, DataGridViewContentAlignment align)
    {
        if (!gridCustomers.Columns.Contains(name)) return;
        var c = gridCustomers.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridCustomers.Columns.Contains(name)) return;
        var c = gridCustomers.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridCustomers.Rows.Count) return;
        var row = gridCustomers.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridCustomers.Columns[e.ColumnIndex].Name != "Status") return;

        var status = row.Cells["Status"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = status switch
        {
            "Active" => Color.FromArgb(22, 130, 60),
            "Archived" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    private void GridCustomers_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (!_canEdit) return;
        if (e.RowIndex < 0) return;

        var column = gridCustomers.Columns[e.ColumnIndex];

        if (column.Name == "colEdit")
            HandleEdit(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
        else if (column.Name == "colUnarchive")
            HandleUnarchive(e.RowIndex);
    }

    private int? GetCustomerIdAtRow(int rowIndex)
    {
        var cell = gridCustomers.Rows[rowIndex].Cells["CustomerId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void HandleEdit(int rowIndex)
    {
        var id = GetCustomerIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmRegisterCustomer(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadCustomers();

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Customer updated.";
        }
    }

    private void HandleArchive(int rowIndex)
    {
        var id = GetCustomerIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Archive this customer? They will no longer appear in active lists.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == id.Value);
            if (customer is null) return;

            customer.IsActive = false;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "Customer", id.Value,
                $"Customer '{customer.CustomerName}' archived");

            lblStatus.ForeColor = AppTheme.Danger;
            lblStatus.Text = "Customer archived.";
            LoadCustomers();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void HandleUnarchive(int rowIndex)
    {
        var id = GetCustomerIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Unarchive this customer? They will become active again.",
            "Confirm Unarchive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var customer = db.Customers.FirstOrDefault(x => x.CustomerId == id.Value);
            if (customer is null) return;

            customer.IsActive = true;
            db.SaveChanges();

            ActivityLogger.Log("Unarchive", "Customer", id.Value,
                $"Customer '{customer.CustomerName}' unarchived");

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Customer unarchived.";
            LoadCustomers();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void BtnRegisterCustomer_Click(object? sender, EventArgs e)
    {
        using var dlg = new FrmRegisterCustomer();

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadCustomers();

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Customer registered successfully.";
        }
    }
}