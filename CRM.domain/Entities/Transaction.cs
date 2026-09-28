namespace CRM.domain.Entities;

public class Transaction
{
    public int TransactionId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = "Cash";

    public decimal AmountPaid { get; set; }

    public decimal ChangeDue { get; set; }

    public string? ReferenceNumber { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    // NEW — which branch this transaction belongs to
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public Order? Order { get; set; }
}