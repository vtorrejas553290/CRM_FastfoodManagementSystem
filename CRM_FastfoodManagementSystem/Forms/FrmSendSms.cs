using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CRM.domain.Models;
using CRM.infrastructure;
using CRM.infrastructure.Data;

namespace CRM.winForms.Forms;

public partial class FrmSendSms : Form
{
    private readonly int _customerId;
    private readonly string _customerName;
    private readonly string _customerCode;
    private readonly string _phone;

    private List<OutreachPromotion> _promotions = new();

    public FrmSendSms(
        int customerId,
        string customerName,
        string customerCode,
        string phone,
        string initialBody = "")
    {
        _customerId = customerId;
        _customerName = customerName;
        _customerCode = customerCode;
        _phone = phone;

        InitializeComponent();
        ApplyTheme();

        Load += (_, __) => FitToScreen();
        Load += FrmSendSms_Load;

        cmbPromotion.SelectedIndexChanged += (_, __) => UpdatePreview();
        txtTemplate.TextChanged += (_, __) => UpdatePreview();

        btnSend.Click += (_, __) => DoSend();
        btnCancel.Click += (_, __) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
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

        pnlHeader.BackColor = AppTheme.Surface;
        pnlBody.BackColor = AppTheme.ContentSurface;
        pnlButtons.BackColor = AppTheme.Surface;

        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.ForeColor = AppTheme.TextPrimary;

        foreach (var lbl in new[]
        {
            lblToLabel, lblPromotion, lblTemplate, lblPreview, lblHint
        })
            AppTheme.StyleLabel(lbl);

        lblTo.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleInput(cmbPromotion);
        AppTheme.StyleInput(txtTemplate);
        AppTheme.StyleInput(txtPreview);

        AppTheme.StyleSuccessButton(btnSend);
        AppTheme.StyleNeutralButton(btnCancel);
    }

    private void FrmSendSms_Load(object? sender, EventArgs e)
    {
        lblTo.Text = $"{_customerName} <{_phone}>";

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

        // Seed the template: caller's prefilled text if any, otherwise the default.
        txtTemplate.Text = string.IsNullOrWhiteSpace(_initialBody)
            ? DefaultTemplate()
            : _initialBody;

        UpdatePreview();
    }

    private string _initialBody = "";

    private string DefaultTemplate()
    {
        return "Hi {CustomerName}! We miss you at CRM FastFood. " +
               "Use code {PromoCode} for {DiscountDisplay} off your next order. " +
               "Valid until {EndDate}. {MinPurchase}";
    }

    private OutreachPromotion? GetSelectedPromotion()
    {
        if (cmbPromotion.SelectedValue is not int id || id == 0) return null;
        return _promotions.FirstOrDefault(p => p.PromotionId == id);
    }

    private void UpdatePreview()
    {
        var promo = GetSelectedPromotion();
        txtPreview.Text = PreviewRender(txtTemplate.Text, promo);
    }

    private string PreviewRender(string template, OutreachPromotion? promo)
    {
        string output = template
            .Replace("{CustomerName}", _customerName)
            .Replace("{CustomerCode}", _customerCode);

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

    private void DoSend()
    {
        var message = txtPreview.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(message))
        {
            MessageBox.Show("Please enter a message.", "Missing message",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtTemplate.Focus();
            return;
        }

        if (message.Length > 480)
        {
            var confirm = MessageBox.Show(
                $"Your message is {message.Length} characters and may be split " +
                "into multiple SMS. Continue?",
                "Long message",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;
        }

        try
        {
            var body = Uri.EscapeDataString(message);
            Process.Start(new ProcessStartInfo
            {
                FileName = $"sms:{_phone}?body={body}",
                UseShellExecute = true
            });

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not open SMS composer:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}