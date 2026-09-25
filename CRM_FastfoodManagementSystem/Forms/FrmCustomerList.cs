using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmCustomerList : Form
{
    private readonly bool _canEdit;
    private readonly bool _canRegister;

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

        Load += (_, __) => LoadCustomers();
        btnRefresh.Click += (_, __) => LoadCustomers();
        btnRegisterCustomer.Click += BtnRegisterCustomer_Click;
        chkShowArchived.CheckedChanged += (_, __) => LoadCustomers();
        txtSearch.TextChanged += (_, __) => LoadCustomers();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlTop.BackColor = AppTheme.Surface;

        AppTheme.StyleSuccessButton(btnRegisterCustomer);
        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridCustomers);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;

        btnRegisterCustomer.Visible = _canRegister;
    }

    private void LoadCustomers()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var search = txtSearch.Text.Trim().ToLower();
            var showArchived = chkShowArchived.Checked;

            var query = db.Customers.AsNoTracking();

            // Show only the relevant set: archived when toggled, active otherwise
            if (showArchived)
                query = query.Where(x => !x.IsActive);
            else
                query = query.Where(x => x.IsActive);

            var customers = query
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    x.CustomerName.ToLower().Contains(search) ||
                    x.CustomerCode.ToLower().Contains(search))
                .OrderBy(x => x.CustomerId)
                .Select(x => new
                {
                    x.CustomerId,
                    x.CustomerCode,
                    x.CustomerName,
                    x.ContactNumber,
                    x.EmailAddress,
                    x.Address,
                    Status = x.IsActive ? "Active" : "Archived",
                    x.CreatedAt
                })
                .ToList();

            gridCustomers.DataSource = customers;

            if (_canEdit)
                BuildActionColumns();
            else
                RemoveActionColumns();

            lblStatus.Text = _canEdit
                ? $"{customers.Count} customer(s) — use the row buttons to Edit or Archive/Unarchive."
                : _canRegister
                    ? $"{customers.Count} customer(s) displayed — you can register new customers."
                    : $"{customers.Count} customer(s) displayed (view only).";

            lblStatus.ForeColor = AppTheme.TextSecondary;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BuildActionColumns()
    {
        RemoveActionColumns();

        // Edit always present when _canEdit
        gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEdit", "Edit", "Edit", "warning", 90));

        if (chkShowArchived.Checked)
        {
            // Archived view → Unarchive button
            gridCustomers.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 120));

            AppTheme.ApplyStyleToButtonColumn(gridCustomers, "colUnarchive", "success");
        }
        else
        {
            // Active view → Archive button
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