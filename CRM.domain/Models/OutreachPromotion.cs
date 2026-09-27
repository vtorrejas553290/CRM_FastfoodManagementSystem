namespace CRM.domain.Models;

public class OutreachPromotion
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; } = "";
    public string PromotionName { get; set; } = "";
    public string DiscountType { get; set; } = "";        // "Percent" or "Fixed"
    public decimal DiscountValue { get; set; }
    public decimal? MinimumPurchase { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Display => $"{PromotionCode} — {PromotionName}";

    public string DiscountDisplay =>
        DiscountType == "Percent"
            ? $"{DiscountValue:N0}%"
            : $"₱{DiscountValue:N2}";
}

public enum OutreachChannel
{
    Sms,
    Email,
    Both
}

public class ContactRecipient
{
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string ContactNumber { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string Retention { get; set; } = "";
    public bool CanSms { get; set; }
    public bool CanEmail { get; set; }
    public bool Selected { get; set; }
}