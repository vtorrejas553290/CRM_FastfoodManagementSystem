namespace CRM.domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    // Kept for backward compatibility — auto-computed from First + Middle + Last
    public string CustomerName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string? ContactNumber { get; set; }

    public string? EmailAddress { get; set; }

    public string? Address { get; set; }

    public DateTime? Birthday { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Loyalty points balance
    public int CurrentPoints { get; set; } = 0;

    public ICollection<CustomerFeedback> Feedbacks { get; set; } = new List<CustomerFeedback>();
    public ICollection<TermsAcceptance> TermsAcceptances { get; set; } = new List<TermsAcceptance>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<PromotionRedemption> Redemptions { get; set; } = new List<PromotionRedemption>();
    public ICollection<CustomerPoint> PointTransactions { get; set; } = new List<CustomerPoint>();
}