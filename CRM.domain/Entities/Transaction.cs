namespace CRM.domain.Entities;

public class Transaction
{
    public int TransactionId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = "Cash";   // "Cash" or "GCash"

    public decimal AmountPaid { get; set; }

    public decimal ChangeDue { get; set; }

    public string? ReferenceNumber { get; set; }          // for GCash

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    public Order? Order { get; set; }
}