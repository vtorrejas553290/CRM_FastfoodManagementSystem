namespace CRM.domain.Entities;

public class RetentionOffer
{
    public int RetentionOfferId { get; set; }

    public int CustomerId { get; set; }

    // Optional — may be a free-text offer without a specific promotion
    public int? PromotionId { get; set; }

    public string OfferText { get; set; } = string.Empty;

    // Pending / Sent / Redeemed / Expired
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public int? CreatedByUserId { get; set; }

    // Navigation
    public Customer? Customer { get; set; }
    public Promotion? Promotion { get; set; }
    public User? CreatedByUser { get; set; }
}