namespace CRM.domain.Entities;

public class Subscription
{
    public int SubscriptionId { get; set; }

    public int CompanyId { get; set; }

    public int PlanId { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? EndsAt { get; set; }

    public bool IsActive { get; set; } = true;

    public Company? Company { get; set; }

    public Plan? Plan { get; set; }
}