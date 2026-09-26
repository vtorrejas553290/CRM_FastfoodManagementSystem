using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmPromotions : Form
{
    private readonly IPromotionController _controller = new PromotionController();

    private List<PromotionRow> _allRows = new();
    private int _currentPage = 1;

    public FrmPromotions()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridPromotions,
            ("colEdit", "warning"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) =>
        {
            InitPager();
            LoadPromotions();
        };

        btnAddPromotion.Click += BtnAddPromotion_Click;
        chkShowArchived.CheckedChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadPromotions();
        };
        txtSearch.TextChanged += (_, __) =>
        {
            _currentPage = 1;
            LoadPromotions();
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
        AppTheme.StyleSuccessButton(btnAddPromotion);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridPromotions);

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
    }

    private void InitPager()
    {
        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;
    }

    private void LoadPromotions()
    {
        try
        {
            var filter = new PromotionFilter
            {
                Search = txtSearch.Text.Trim().ToLower(),
                ShowInactive = chkShowArchived.Checked,
                Now = DateTime.UtcNow
            };

            _allRows = _controller.GetPromotions(filter);

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
        List<PromotionRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        gridPromotions.DataSource = pageRows;
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

    // ============================================================
    //  ACTION COLUMNS
    // ============================================================

    private void BuildActionColumns()
    {
        for (int i = gridPromotions.Columns.Count - 1; i >= 0; i--)
        {
            var col = gridPromotions.Columns[i];
            if (col.Name == "colEdit" || col.Name == "colArchive" || col.Name == "colUnarchive")
                gridPromotions.Columns.RemoveAt(i);
        }

        gridPromotions.Columns.Add(AppTheme.CreateGridButtonColumn(
            "colEdit", "Edit", "Edit", "warning", 90));

        if (chkShowArchived.Checked)
        {
            gridPromotions.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colUnarchive", "Unarchive", "Unarchive", "success", 110));

            AppTheme.ApplyStyleToButtonColumn(gridPromotions, "colUnarchive", "success");
        }
        else
        {
            gridPromotions.Columns.Add(AppTheme.CreateGridButtonColumn(
                "colArchive", "Archive", "Archive", "danger", 100));

            AppTheme.ApplyStyleToButtonColumn(gridPromotions, "colArchive", "danger");
        }

        AppTheme.ApplyStyleToButtonColumn(gridPromotions, "colEdit", "warning");

        gridPromotions.CellContentClick -= GridPromotions_CellContentClick;
        gridPromotions.CellContentClick += GridPromotions_CellContentClick;
    }

    private void StyleColumns()
    {
        var grid = gridPromotions;

        if (grid.Columns.Contains("PromotionId")) grid.Columns["PromotionId"].Visible = false;

        SetColumnFixed("PromotionCode", "Code", 120, DataGridViewContentAlignment.MiddleLeft);
        SetColumnFixed("Discount", "Discount", 110, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("MinPurchase", "Min Purchase", 130, DataGridViewContentAlignment.MiddleRight);
        SetColumnFixed("Validity", "Validity", 200, DataGridViewContentAlignment.MiddleCenter);
        SetColumnFixed("State", "State", 110, DataGridViewContentAlignment.MiddleCenter);

        SetColumnFill("PromotionName", "Name", 100, 180, DataGridViewContentAlignment.MiddleLeft);

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
        if (!gridPromotions.Columns.Contains(name)) return;
        var c = gridPromotions.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c.Width = width;
        c.MinimumWidth = width;
        c.DefaultCellStyle.Alignment = align;
    }

    private void SetColumnFill(string name, string header, int fillWeight, int minWidth, DataGridViewContentAlignment align)
    {
        if (!gridPromotions.Columns.Contains(name)) return;
        var c = gridPromotions.Columns[name];
        c.HeaderText = header;
        c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        c.FillWeight = fillWeight;
        c.MinimumWidth = minWidth;
        c.DefaultCellStyle.Alignment = align;
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= gridPromotions.Rows.Count) return;
        var row = gridPromotions.Rows[e.RowIndex];
        if (row.DataBoundItem is null) return;
        if (gridPromotions.Columns[e.ColumnIndex].Name != "State") return;

        var state = row.Cells["State"]?.Value?.ToString() ?? "";
        e.CellStyle.ForeColor = state switch
        {
            "Active" => Color.FromArgb(22, 130, 60),
            "Upcoming" => Color.FromArgb(30, 100, 200),
            "Expired" => Color.FromArgb(190, 40, 40),
            "Archived" => Color.FromArgb(120, 120, 120),
            _ => AppTheme.TextPrimary
        };
        e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    private void GridPromotions_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var column = gridPromotions.Columns[e.ColumnIndex];

        if (column.Name == "colEdit")
            HandleEdit(e.RowIndex);
        else if (column.Name == "colArchive")
            HandleArchive(e.RowIndex);
        else if (column.Name == "colUnarchive")
            HandleUnarchive(e.RowIndex);
    }

    private int? GetPromotionIdAtRow(int rowIndex)
    {
        var cell = gridPromotions.Rows[rowIndex].Cells["PromotionId"];
        if (cell?.Value is null) return null;
        return Convert.ToInt32(cell.Value);
    }

    private void BtnAddPromotion_Click(object? sender, EventArgs e)
    {
        using var dlg = new FrmPromotionEditor(null);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Create", "Promotion", null, "New promotion created");
            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "Promotion added.";
            LoadPromotions();
        }
    }

    private void HandleEdit(int rowIndex)
    {
        var id = GetPromotionIdAtRow(rowIndex);
        if (id is null) return;

        using var dlg = new FrmPromotionEditor(id.Value);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            ActivityLogger.Log("Update", "Promotion", id.Value,
                $"Promotion #{id.Value} updated");
            lblStatus.ForeColor = AppTheme.Success;
            lblStatus.Text = "Promotion updated.";
            LoadPromotions();
        }
    }

    private void HandleArchive(int rowIndex)
    {
        var id = GetPromotionIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Deactivate this promotion? It will no longer be applied to new orders.",
            "Confirm",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var promo = db.Promotions.FirstOrDefault(x => x.PromotionId == id.Value);
            if (promo is null) return;

            promo.IsActive = false;
            db.SaveChanges();

            ActivityLogger.Log("Archive", "Promotion", id.Value,
                $"Promotion '{promo.PromotionCode}' archived");

            lblStatus.ForeColor = AppTheme.Danger;
            lblStatus.Text = "Promotion archived.";
            LoadPromotions();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void HandleUnarchive(int rowIndex)
    {
        var id = GetPromotionIdAtRow(rowIndex);
        if (id is null) return;

        var confirm = MessageBox.Show(
            "Reactivate this promotion? It will be applied to eligible orders again.",
            "Confirm Unarchive",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            using var db = AppServices.CreateTenantContext();
            var promo = db.Promotions.FirstOrDefault(x => x.PromotionId == id.Value);
            if (promo is null) return;

            promo.IsActive = true;
            db.SaveChanges();

            ActivityLogger.Log("Unarchive", "Promotion", id.Value,
                $"Promotion '{promo.PromotionCode}' unarchived");

            lblStatus.ForeColor = AppTheme.SuccessGreen;
            lblStatus.Text = "Promotion unarchived.";
            LoadPromotions();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}