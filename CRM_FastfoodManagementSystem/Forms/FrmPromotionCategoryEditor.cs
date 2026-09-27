using CRM.domain.Entities;
using CRM.infrastructure;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace CRM.winForms.Forms;

public partial class FrmPromotionCategoryEditor : Form
{
    private readonly int? _categoryId;

    public FrmPromotionCategoryEditor(int? categoryId)
    {
        _categoryId = categoryId;
        InitializeComponent();
        ApplyTheme();

        Load += (_, __) => LoadIfEditing();
        btnSave.Click += (_, __) => Save();
        btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblTitle.ForeColor = AppTheme.TextPrimary;

        foreach (var lbl in new[] { lblName, lblDescription, lblHint })
            AppTheme.StyleLabel(lbl);

        AppTheme.StyleInput(txtName);
        AppTheme.StyleInput(txtDescription);

        AppTheme.StyleSuccessButton(btnSave);
        AppTheme.StyleNeutralButton(btnCancel);
    }

    private void LoadIfEditing()
    {
        if (_categoryId is null)
        {
            lblTitle.Text = "New Promotion Category";
            lblHint.Text =
                "Tip: name the category 'New Comers' for a new-customer welcome offer, " +
                "or 'Win Them Back' for a win-back offer.";
            return;
        }

        lblTitle.Text = "Edit Promotion Category";

        using var db = AppServices.CreateTenantContext();
        var cat = db.PromotionCategories.FirstOrDefault(c => c.PromotionCategoryId == _categoryId.Value);
        if (cat is null)
        {
            MessageBox.Show("Category not found.", "Error");
            Close();
            return;
        }

        txtName.Text = cat.CategoryName;
        txtDescription.Text = cat.Description ?? "";
    }

    private void Save()
    {
        var name = txtName.Text.Trim();
        var desc = txtDescription.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Enter a category name.", "Required");
            txtName.Focus();
            return;
        }

        try
        {
            using var db = AppServices.CreateTenantContext();

            bool duplicate = db.PromotionCategories
                .Any(c => c.CategoryName == name
                       && c.PromotionCategoryId != (_categoryId ?? 0));

            if (duplicate)
            {
                MessageBox.Show("A category with that name already exists.", "Duplicate");
                return;
            }

            PromotionCategory cat;
            if (_categoryId is null)
            {
                cat = new PromotionCategory
                {
                    CategoryName = name,
                    Description = string.IsNullOrWhiteSpace(desc) ? null : desc,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                db.PromotionCategories.Add(cat);
            }
            else
            {
                cat = db.PromotionCategories.First(c => c.PromotionCategoryId == _categoryId.Value);
                cat.CategoryName = name;
                cat.Description = string.IsNullOrWhiteSpace(desc) ? null : desc;
            }

            db.SaveChanges();

            ActivityLogger.Log(_categoryId is null ? "Create" : "Update",
                "PromotionCategory", cat.PromotionCategoryId,
                $"Category '{cat.CategoryName}' saved");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}