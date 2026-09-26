namespace CRM.domain.Models;

public class PromotionFilter
{
    public string Search { get; set; } = "";

    /// <summary>When true, query archived rows; when false, query active rows.</summary>
    public bool ShowInactive { get; set; }

    /// <summary>
    /// The "now" used to compute Upcoming / Expired / Active state.
    /// Explicit input so the controller stays pure and behavior is identical.
    /// </summary>
    public DateTime Now { get; set; } = DateTime.UtcNow;
}