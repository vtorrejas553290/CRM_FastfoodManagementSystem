namespace CRM.domain.Entities;

public class CustomerPoint
{
    public int CustomerPointId { get; set; }

    public int CustomerId { get; set; }

    // "Earned" | "Redeemed" | "Bonus" | "Correction"
    public string TransactionType { get; set; } = "Earned";

    // Positive for earned/bonus; negative for redeemed/correction
    public int Points { get; set; }

    public int? OrderId { get; set; }

    public string? Notes { get; set; }

    public int? PerformedByUserId { get; set; }

    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public Customer? Customer { get; set; }

    public Order? Order { get; set; }
}