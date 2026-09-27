using CRM.domain.Entities;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms.Forms;

public partial class FrmPromotionCategories : Form
{
    public FrmPromotionCategories()
    {
        InitializeComponent();
        ApplyTheme();

        Load += (_, __) => LoadCategories();
        btnAdd.Click += (_, __) => AddOrEdit(null);
        btnClose.Click += (_, __) => Close();
        gridCategories.CellContentClick += GridCategories_CellContentClick;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleGrid(gridCategories);
        AppTheme.StyleSuccessButton(btnAdd);
        AppTheme.StyleNeutralButton(btnClose);
    }

    private void LoadCategories()
    {
        using var db = AppServices.CreateTenantContext();

        var rows = db.PromotionCategories
            .AsNoTracking()
            .OrderBy(c => c.CategoryName)
            .Select(c => new
            {
                c.PromotionCategoryId,
                c.CategoryName,
                c.Description,
                PromotionsUsing = c.Promotions.Count(),
                Status = c.IsActive ? "Active" : "Archived"
            })
            .ToList();

        gridCategories.DataSource = rows;

        if (gridCategories.Columns.Contains("PromotionCategoryId"))
            gridCategories.Columns["PromotionCategoryId"].Visible = false;

        if (gridCategories.Columns.Contains("CategoryName"))
        {
            gridCategories.Columns["CategoryName"].HeaderText = "Category";
            gridCategories.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridCategories.Columns["CategoryName"].FillWeight = 40;
        }

        if (gridCategories.Columns.Contains("Description"))
        {
            gridCategories.Columns["Description"].HeaderText = "Description";
            gridCategories.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridCategories.Columns["Description"].FillWeight = 60;
        }

        if (gridCategories.Columns.Contains("PromotionsUsing"))
        {
            gridCategories.Columns["PromotionsUsing"].HeaderText = "In Use";
            gridCategories.Columns["PromotionsUsing"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridCategories.Columns["PromotionsUsing"].Width = 70;
            gridCategories.Columns["PromotionsUsing"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        if (gridCategories.Columns.Contains("Status"))
        {
            gridCategories.Columns["Status"].HeaderText = "Status";
            gridCategories.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            gridCategories.Columns["Status"].Width = 90;
            gridCategories.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        for (int i = gridCategories.Columns.Count - 1; i >= 0; i--)
        {
            if (gridCategories.Columns[i] is DataGridViewButtonColumn)
                gridCategories.Columns.RemoveAt(i);
        }

        var colEdit = new DataGridViewButtonColumn
        {
            Name = "colEdit",
            HeaderText = "",
            Text = "Edit",
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat,
            Width = 70,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            Resizable = DataGridViewTriState.False,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            DefaultCellStyle = AppTheme.GridButtonStyle("warning")
        };
        gridCategories.Columns.Add(colEdit);

        var colToggle = new DataGridViewButtonColumn
        {
            Name = "colToggle",
            HeaderText = "",
            Text = "Archive",
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat,
            Width = 90,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            Resizable = DataGridViewTriState.False,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            DefaultCellStyle = AppTheme.GridButtonStyle("danger")
        };
        gridCategories.Columns.Add(colToggle);

        AppTheme.ApplyStyleToButtonColumn(gridCategories, "colEdit", "warning");
        AppTheme.ApplyStyleToButtonColumn(gridCategories, "colToggle", "danger");
    }

    private void GridCategories_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var col = gridCategories.Columns[e.ColumnIndex];
        var idCell = gridCategories.Rows[e.RowIndex].Cells["PromotionCategoryId"];
        if (idCell?.Value is null) return;
        int id = Convert.ToInt32(idCell.Value);

        if (col.Name == "colEdit")
            AddOrEdit(id);
        else if (col.Name == "colToggle")
            ToggleActive(id);
    }

    private void AddOrEdit(int? categoryId)
    {
        using var dlg = new FrmPromotionCategoryEditor(categoryId);
        if (dlg.ShowDialog(this) == DialogResult.OK)
            LoadCategories();
    }

    private void ToggleActive(int categoryId)
    {
        using var db = AppServices.CreateTenantContext();
        var cat = db.PromotionCategories.FirstOrDefault(c => c.PromotionCategoryId == categoryId);
        if (cat is null) return;

        cat.IsActive = !cat.IsActive;
        db.SaveChanges();

        ActivityLogger.Log(cat.IsActive ? "Unarchive" : "Archive",
            "PromotionCategory", categoryId,
            $"Category '{cat.CategoryName}' {(cat.IsActive ? "reactivated" : "archived")}");

        LoadCategories();
    }
}