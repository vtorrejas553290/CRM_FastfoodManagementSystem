namespace CRM.domain.Entities;

public class TermsAcceptance
{
    public int TermsAcceptanceId { get; set; }

    public int TermsAndConditionId { get; set; }

    public int CustomerId { get; set; }

    public DateTime AcceptedAt { get; set; } = DateTime.UtcNow;

    public string? IpAddress { get; set; }

    public TermsAndCondition? TermsAndCondition { get; set; }

    public Customer? Customer { get; set; }
}