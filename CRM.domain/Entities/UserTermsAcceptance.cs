namespace CRM.domain.Entities;

public class UserTermsAcceptance
{
    public int UserTermsAcceptanceId { get; set; }

    public int TermsAndConditionId { get; set; }

    public int UserId { get; set; }

    public DateTime AcceptedAt { get; set; } = DateTime.UtcNow;

    public string? IpAddress { get; set; }

    public TermsAndCondition? TermsAndCondition { get; set; }

    public User? User { get; set; }
}