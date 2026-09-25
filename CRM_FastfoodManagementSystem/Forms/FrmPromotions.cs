using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmPromotions : Form
{
    public FrmPromotions()
    {
        InitializeComponent();
        ApplyTheme();

        AppTheme.RegisterButtonColumnStyles(gridPromotions,
            ("colEdit", "warning"),
            ("colArchive", "danger"),
            ("colUnarchive", "success"));

        Load += (_, __) => LoadPromotions();
        btnRefresh.Click += (_, __) => LoadPromotions();
        btnAddPromotion.Click += BtnAddPromotion_Click;
        chkShowArchived.CheckedChanged += (_, __) => LoadPromotions();
        txtSearch.TextChanged += (_, __) => LoadPromotions();
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        pnlTop.BackColor = AppTheme.Surface;
        pnlGridWrap.BackColor = AppTheme.ContentSurface;

        AppTheme.StyleInput(txtSearch);
        AppTheme.StyleSecondaryButton(btnRefresh);
        AppTheme.StyleSuccessButton(btnAddPromotion);
        AppTheme.StyleLabel(lblStatus);
        AppTheme.StyleGrid(gridPromotions);

        chkShowArchived.Font = AppTheme.FontBody;
        chkShowArchived.ForeColor = AppTheme.TextPrimary;
    }

    private void LoadPromotions()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            var search = txtSearch.Text.Trim().ToLower();
            var showInactive = chkShowArchived.Checked;
            var now = DateTime.UtcNow;

            var query = db.Promotions.AsNoTracking();

            // Show only the relevant set: archived when toggled, active otherwise
            if (showInactive)
                query = query.Where(x => !x.IsActive);
            else
                query = query.Where(x => x.IsActive);

            var items = query
                .Where(x =>
                    string.IsNullOrWhiteSpace(search) ||
                    x.PromotionName.ToLower().Contains(search) ||
                    x.PromotionCode.ToLower().Contains(search))
                .OrderByDescending(x => x.CreatedAt)
                .ToList()
                .Select(x => new
                {
                    x.PromotionId,
                    x.PromotionCode,
                    x.PromotionName,
                    Discount = x.DiscountType == "Percent"
                        ? $"{x.DiscountValue:N0}%"
                        : $"₱{x.DiscountValue:N2}",
                    MinPurchase = x.MinimumPurchase.HasValue
                        ? $"₱{x.MinimumPurchase:N2}"
                        : "-",
                    Validity = $"{x.StartDate:yyyy-MM-dd} → {x.EndDate:yyyy-MM-dd}",
                    State = !x.IsActive ? "Archived"
                          : (now < x.StartDate ? "Upcoming"
                          : (now > x.EndDate ? "Expired" : "Active"))
                })
                .ToList();

            gridPromotions.DataSource = items;
            BuildActionColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

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