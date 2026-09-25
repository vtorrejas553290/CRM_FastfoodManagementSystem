namespace CRM.domain.Entities;

public class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionCode { get; set; } = string.Empty;

    public string PromotionName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string DiscountType { get; set; } = "Percent";   // "Percent" or "Fixed"

    public decimal DiscountValue { get; set; }

    public decimal? MinimumPurchase { get; set; }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(30);

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PromotionRedemption> Redemptions { get; set; } = new List<PromotionRedemption>();
}