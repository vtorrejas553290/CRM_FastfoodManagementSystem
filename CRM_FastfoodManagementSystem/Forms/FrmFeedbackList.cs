using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmFeedbackList : Form
{
    // Only STAFF can add feedback
    private readonly bool _canAdd;

    // ADMIN and MANAGER can edit status + archive
    private readonly bool _canManage;

    // ADMIN, MANAGER, and STAFF can view
    private readonly bool _canView;

    public FrmFeedbackList()
    {
        _canAdd = UserSession.IsStaff;
        _canManage = UserSession.IsAdmin || UserSession.IsManager;
        _canView = UserSession.IsStaff || UserSession.IsAdmin || UserSession.IsManager;

        InitializeComponent();
        ApplyTheme();

        Load += (_, __) =>
        {
            InitStatusFilter();
            InitStarsFilter();
            LoadFeedback();
        };

        btnRefresh.Click += (_, __) => LoadFeedback();
        btnAdd.Click += BtnAdd_Click;
        txtSearch.TextChanged += (_, __) => LoadFeedback();
        cmbFilterStatus.SelectedIndexChanged += (_, __) => LoadFeedback();
        cmbFilterStars.SelectedIndexChanged += (_, __) => LoadFeedback();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlTop.BackColor = AppTheme.Surface;
        pnlGridWrap.BackColor = AppTheme.ContentSurface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleInput(cmbFilterStatus);
        AppTheme.StyleInput(cmbFilterStars);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleSuccessButton(btnAdd);
        AppTheme.StyleGrid(gridFeedback);

        AppTheme.StyleLabel(lblSearch);
        AppTheme.StyleLabel(lblFilterStatus);
        AppTheme.StyleLabel(lblFilterStars);

        lblMode.Font = AppTheme.FontHeading;
        lblMode.ForeColor = AppTheme.TextPrimary;

        // Mode label
        if (_canAdd)
            lblMode.Text = "Customer Feedback — Staff (Add)";
        else if (_canManage)
            lblMode.Text = "Customer Feedback — Management (Edit / Archive)";
        else
            lblMode.Text = "Customer Feedback — View Only";

        // Only Staff sees Add button
        btnAdd.Visible = _canAdd;

        // Register row-action styling once (for managers/admins)
        if (_canManage)
        {
            AppTheme.RegisterButtonColumnStyles(gridFeedback,
                ("colEditStatus", "warning"),
                ("colArchive", "danger"));
        }
    }

    private void InitStatusFilter()
    {
        cmbFilterStatus.Items.Clear();
        cmbFilterStatus.Items.Add("All");
        cmbFilterStatus.Items.Add("New");
        cmbFilterStatus.Items.Add("Reviewed");
        cmbFilterStatus.Items.Add("Resolved");
        cmbFilterStatus.Items.Add("Archived");
        cmbFilterStatus.SelectedIndex = 0;
    }

    private void InitStarsFilter()
    {
        cmbFilterStars.Items.Clear();
        cmbFilterStars.Items.Add("All");
        cmbFilterStars.Items.Add("5 ★");
        cmbFilterStars.Items.Add("4 ★");
        cmbFilterStars.Items.Add("3 ★");
        cmbFilterStars.Items.Add("2 ★");
        cmbFilterStars.Items.Add("1 ★");
        cmbFilterStars.SelectedIndex = 0;
    }

    private void LoadFeedback()
    {
        try
        {
            if (!_canView)
            {
                gridFeedback.DataSource = new List<object>();
                return;
            }

            using var db = AppServices.CreateTenantContext();

            var search = txtSearch.Text.Trim().ToLower();
            var status = cmbFilterStatus.SelectedItem?.ToString() ?? "All";
            var starsText = cmbFilterStars.SelectedItem?.ToString() ?? "All";

            var query = db.CustomerFeedbacks
                .Include(x => x.Customer)
                .AsNoTracking();

            if (status != "All")
                query = query.Where(x => x.Status == status);

            if (starsText != "All" && starsText.Length > 0)
            {
                var firstChar = starsText[0];
                if (char.IsDigit(firstChar))
                {
                    int stars = firstChar - '0';
                    query = query.Where(x => x.Rating == stars);
                }
            }

            var items = query
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    (x.Customer != null && x.Customer.CustomerName.ToLower().Contains(search)) ||
                    (x.Comments != null && x.Comments.ToLower().Contains(search)))
                .OrderByDescending(x => x.SubmittedAt)
                .Select(x => new
                {
                    x.CustomerFeedbackId,
                    Customer = x.Customer != null ? x.Customer.CustomerName : "(deleted)",
                    x.Rating,
                    Category = x.Category,
                    x.Comments,
                    x.Status,
                    x.SubmittedAt
                })
                .ToList();

            gridFeedback.DataSource = items;

            // Add row-action columns only for admin/manager
            if (_canManage)
                BuildActionColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Adds Edit Status + Archive row actions to the grid.
    /// Only used for Admin and Manager.
    /// </summary>
    private void BuildActionColumns()
    {
        for (int i = gridFeedback.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridFeedback.Columns[i];
            if (col.Name == "colEditStatus" || col.Name == "colArchive")
                gridFeedback.Columns.RemoveAt(i);
        }

        gridFeedback.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEditStatus", "Edit", "Edit Status", "warning", 110));

        gridFeedback.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colArchive", "Archive", "Archive", "danger", 100));

        // Immediate attempt
        AppTheme.ApplyStyleToButtonColumn(gridFeedback, "colEditStatus", "warning");
        AppTheme.ApplyStyleToButtonColumn(gridFeedback, "colArchive", "danger");

        gridFeedback.CellContentClick -= GridFeedback_CellContentClick;
        gridFeedback.CellContentClick += GridFeedback_CellContentClick;
    }

    private void GridFeedback_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridFeedback.Columns[e.ColumnIndex];

        if (column.Name == "colEditStatus")
            HandleEditStatus(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
    }

    private int? GetFeedbackIdAtRow(int rowIndex)
    {
        var cell = gridFeedback.Rows[rowIndex].Cells["CustomerFeedbackId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (!_canAdd)
        {
            MessageBox.Show("Only Staff can add feedback.", "Not allowed");
            return;
        }

        using var dlg = new FrmFeedbackEntry(null);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadFeedback();
        }
    }

    private void HandleEditStatus(int rowIndex)
    {
        if (!_canManage)
        {
            MessageBox.Show("Only Admin and Manager can edit feedback status.", "Not allowed");
            return;
        }

        var id = GetFeedbackIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmFeedbackStatusEditor(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadFeedback();
        }
    }

    private void HandleArchive(int rowIndex)
    {
        if (!_canManage)
        {
            MessageBox.Show("Only Admin and Manager can archive feedback.", "Not allowed");
            return;
        }

        var id = GetFeedbackIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Archive this feedback? It will be marked as 'Archived'.",
            "Confirm Archive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var fb = db.CustomerFeedbacks.FirstOrDefault(x => x.CustomerFeedbackId == id.Value);
            if (fb is null) return;

            fb.Status = "Archived";
            db.SaveChanges();

            ActivityLogger.Log("Archive", "CustomerFeedback", id.Value,
                $"Feedback #{id.Value} archived");

            MessageBox.Show("Feedback archived.", "Success");
            LoadFeedback();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}