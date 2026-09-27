using CRM.domain.Models;
using CRM.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmBulkContact : Form
{
    private readonly List<ContactRecipient> _recipients;
    private List<OutreachPromotion> _promotions = new();

    public FrmBulkContact(List<ContactRecipient> recipients)
    {
        _recipients = recipients;
        InitializeComponent();
        ApplyTheme();

        Load += FrmBulkContact_Load;
        Load += (_, __) => FitToScreen();

        rbSms.CheckedChanged += (_, __) => UpdateChannelVisibility();
        rbEmail.CheckedChanged += (_, __) => UpdateChannelVisibility();
        rbBoth.CheckedChanged += (_, __) => UpdateChannelVisibility();

        cmbPromotion.SelectedIndexChanged += (_, __) => UpdatePreview();
        txtTemplate.TextChanged += (_, __) => UpdatePreview();

        btnSend.Click += (_, __) => DoSend();
        btnClose.Click += (_, __) => Close();

        gridRecipients.CellValueChanged += (_, __) => UpdateRecipientCount();
        gridRecipients.CurrentCellDirtyStateChanged += (_, __) =>
        {
            if (gridRecipients.IsCurrentCellDirty)
                gridRecipients.CommitEdit(DataGridViewDataErrorContexts.Commit);
        };
    }

    private void FitToScreen()
    {
        var wa = Screen.FromControl(this).WorkingArea;

        int maxW = wa.Width - 60;
        int maxH = wa.Height - 60;

        int minW = Math.Min(MinimumSize.Width, maxW);
        int minH = Math.Min(MinimumSize.Height, maxH);

        int w = Math.Clamp(Width, minW, maxW);
        int h = Math.Clamp(Height, minH, maxH);

        Size = new Size(w, h);

        StartPosition = FormStartPosition.Manual;
        Location = new Point(
            wa.Left + (wa.Width - Width) / 2,
            wa.Top + (wa.Height - Height) / 2);
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);

        tblRoot.BackColor = AppTheme.ContentSurface;
        pnlHeader.BackColor = AppTheme.Surface;
        tblTopRow.BackColor = AppTheme.Surface;
        tblMessages.BackColor = AppTheme.ContentSurface;
        tblRecipients.BackColor = AppTheme.ContentSurface;
        flowFooter.BackColor = AppTheme.Surface;

        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTitle.ForeColor = AppTheme.TextPrimary;
        lblSubtitle.ForeColor = AppTheme.TextSecondary;

        foreach (var lbl in new[]
        {
            lblChannel, lblPromotion, lblTemplate, lblPreview, lblRecipients, lblSummary
        })
            AppTheme.StyleLabel(lbl);

        AppTheme.StyleInput(cmbPromotion);
        AppTheme.StyleInput(txtTemplate);
        AppTheme.StyleInput(txtPreview);
        AppTheme.StyleGrid(gridRecipients);

        AppTheme.StyleSuccessButton(btnSend);
        AppTheme.StyleNeutralButton(btnClose);

        rbSms.ForeColor = AppTheme.TextPrimary;
        rbEmail.ForeColor = AppTheme.TextPrimary;
        rbBoth.ForeColor = AppTheme.TextPrimary;
    }

    private void FrmBulkContact_Load(object? sender, EventArgs e)
    {
        rbSms.Checked = true;
        UpdateChannelVisibility();

        using var db = AppServices.CreateTenantContext();
        var now = DateTime.UtcNow;

        _promotions = db.Promotions
            .AsNoTracking()
            .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
            .OrderBy(p => p.PromotionName)
            .Select(p => new OutreachPromotion
            {
                PromotionId = p.PromotionId,
                PromotionCode = p.PromotionCode,
                PromotionName = p.PromotionName,
                DiscountType = p.DiscountType,
                DiscountValue = p.DiscountValue,
                MinimumPurchase = p.MinimumPurchase,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            })
            .ToList();

        var promoItems = new List<object>
        {
            new { PromotionId = 0, Display = "(no promotion)" }
        };
        promoItems.AddRange(_promotions.Select(p => new { p.PromotionId, p.Display }));

        cmbPromotion.DataSource = promoItems;
        cmbPromotion.DisplayMember = "Display";
        cmbPromotion.ValueMember = "PromotionId";
        cmbPromotion.SelectedIndex = 0;

        gridRecipients.DataSource = _recipients;
        StyleRecipientGrid();
        UpdateRecipientCount();

        txtTemplate.Text = DefaultTemplate();
        UpdatePreview();
    }

    private string DefaultTemplate()
    {
        return "Hi {CustomerName}! We miss you at CRM FastFood. " +
               "Use code {PromoCode} for {DiscountDisplay} off your next order. " +
               "Valid until {EndDate}. {MinPurchase}";
    }

    private void StyleRecipientGrid()
    {
        var grid = gridRecipients;

        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grid.ScrollBars = ScrollBars.Both;

        void Hide(string name)
        {
            if (grid.Columns.Contains(name)) grid.Columns[name].Visible = false;
        }

        void Fixed(string name, string header, int width, DataGridViewContentAlignment align)
        {
            if (!grid.Columns.Contains(name)) return;
            var c = grid.Columns[name];
            c.HeaderText = header;
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            c.Width = width;
            c.MinimumWidth = width;
            c.DefaultCellStyle.Alignment = align;
        }

        Hide("CustomerId");
        Hide("CanSms");
        Hide("CanEmail");

        Fixed("CustomerCode", "Code", 90, DataGridViewContentAlignment.MiddleLeft);
        Fixed("ContactNumber", "Phone", 120, DataGridViewContentAlignment.MiddleLeft);
        Fixed("Retention", "Retention", 90, DataGridViewContentAlignment.MiddleCenter);
        Fixed("Selected", "Send", 60, DataGridViewContentAlignment.MiddleCenter);

        if (grid.Columns.Contains("CustomerName"))
        {
            var c = grid.Columns["CustomerName"];
            c.HeaderText = "Customer";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c.FillWeight = 60;
            c.MinimumWidth = 120;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        if (grid.Columns.Contains("EmailAddress"))
        {
            var c = grid.Columns["EmailAddress"];
            c.HeaderText = "Email";
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            c.FillWeight = 40;
            c.MinimumWidth = 140;
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        grid.ReadOnly = false;
        foreach (DataGridViewColumn col in grid.Columns)
            col.ReadOnly = col.Name != "Selected";
    }

    private void UpdateChannelVisibility()
    {
        UpdatePreview();
        UpdateRecipientCount();
    }

    private OutreachPromotion? GetSelectedPromotion()
    {
        if (cmbPromotion.SelectedValue is not int id || id == 0) return null;
        return _promotions.FirstOrDefault(p => p.PromotionId == id);
    }

    private OutreachChannel GetSelectedChannel()
    {
        if (rbEmail.Checked) return OutreachChannel.Email;
        if (rbBoth.Checked) return OutreachChannel.Both;
        return OutreachChannel.Sms;
    }

    private void UpdatePreview()
    {
        var promo = GetSelectedPromotion();
        var first = _recipients.FirstOrDefault(r => r.Selected);

        string rendered;
        if (first is null)
        {
            rendered = "(no recipients selected)";
        }
        else
        {
            rendered = PreviewRender(txtTemplate.Text, first, promo);
        }

        txtPreview.Text = rendered;
    }

    private static string PreviewRender(
        string template, ContactRecipient r, OutreachPromotion? promo)
    {
        string output = template
            .Replace("{CustomerName}", r.CustomerName)
            .Replace("{CustomerCode}", r.CustomerCode)
            .Replace("{Retention}", r.Retention);

        if (promo != null)
        {
            output = output
                .Replace("{PromoCode}", promo.PromotionCode)
                .Replace("{PromoName}", promo.PromotionName)
                .Replace("{DiscountDisplay}", promo.DiscountDisplay)
                .Replace("{EndDate}", promo.EndDate.ToString("yyyy-MM-dd"))
                .Replace("{MinPurchase}",
                    promo.MinimumPurchase.HasValue
                        ? $"Min purchase ₱{promo.MinimumPurchase.Value:N2}."
                        : "");
        }
        else
        {
            output = output
                .Replace("{PromoCode}", "")
                .Replace("{PromoName}", "")
                .Replace("{DiscountDisplay}", "")
                .Replace("{EndDate}", "")
                .Replace("{MinPurchase}", "");
        }

        return output.Trim();
    }

    private void UpdateRecipientCount()
    {
        int selected = _recipients.Count(r => r.Selected);
        int smsable = _recipients.Count(r => r.Selected && r.CanSms);
        int emailable = _recipients.Count(r => r.Selected && r.CanEmail);

        lblSummary.Text =
            $"{selected} selected · {smsable} with phone · {emailable} with email";

        btnSend.Enabled = selected > 0;
    }

    private void DoSend()
    {
        var channel = GetSelectedChannel();
        var promo = GetSelectedPromotion();

        if (string.IsNullOrWhiteSpace(txtTemplate.Text))
        {
            MessageBox.Show("Enter a message template.", "Missing template");
            return;
        }

        int selected = _recipients.Count(r => r.Selected);
        if (selected == 0)
        {
            MessageBox.Show("No recipients selected.", "Nothing to send");
            return;
        }

        var confirm = MessageBox.Show(
            $"Send to {selected} customer(s) via {channel}?\n\n" +
            $"This will write a contact-log entry for each recipient. " +
            $"Actual SMS/email delivery is not enabled.",
            "Confirm Bulk Send",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            var result = OutreachService.Send(
                _recipients,
                channel,
                txtTemplate.Text,
                promo,
                UserSession.UserId,
                UserSession.Username ?? "system");

            MessageBox.Show(
                $"Sent: {result.SentCount}\n" +
                $"Skipped: {result.SkippedCount}" +
                (result.SkippedReasons.Count > 0
                    ? "\n\n" + string.Join("\n", result.SkippedReasons.Take(10))
                    : ""),
                "Bulk Contact Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}