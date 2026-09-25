namespace CRM.domain.Entities;

public class TermsAndCondition
{
    public int TermsAndConditionId { get; set; }

    public string Version { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

    public DateTime? EffectiveTo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // NEW — track who created this version
    public int CreatedByUserId { get; set; }

    public string CreatedByRoleCode { get; set; } = string.Empty;

    public ICollection<TermsAcceptance> Acceptances { get; set; } = new List<TermsAcceptance>();
}