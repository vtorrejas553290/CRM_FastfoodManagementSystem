namespace CRM.domain.Entities;

public class PromotionRedemption
{
    public int PromotionRedemptionId { get; set; }

    public int PromotionId { get; set; }

    public int CustomerId { get; set; }

    public int OrderId { get; set; }

    public decimal DiscountApplied { get; set; }

    public DateTime RedeemedAt { get; set; } = DateTime.UtcNow;

    public Promotion? Promotion { get; set; }

    public Customer? Customer { get; set; }

    public Order? Order { get; set; }
}