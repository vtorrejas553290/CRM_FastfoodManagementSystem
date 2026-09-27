namespace CRM.domain.Entities;

public class Plan
{
    public int PlanId { get; set; }

    public string PlanCode { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;

    public bool HasMainTransaction { get; set; }

    public bool HasDataCollection { get; set; }

    public bool HasBusinessIntelligence { get; set; }

    public bool HasActions { get; set; }

    public bool HasBranching { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}