namespace CRM.domain.Models;

/// <summary>
/// Flattened row for the View Transactions grid.
/// Property names match the anonymous-type projection that was previously
/// bound to gridTransactions, so grid column bindings are unchanged.
/// </summary>
public class TransactionRow
{
    public int TransactionId { get; set; }
    public string OrderCode { get; set; } = "";
    public string Customer { get; set; } = "";
    public string PaymentMethod { get; set; } = "";
    public decimal Total { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeDue { get; set; }
    public string Reference { get; set; } = "";
    public DateTime PaidAt { get; set; }
}