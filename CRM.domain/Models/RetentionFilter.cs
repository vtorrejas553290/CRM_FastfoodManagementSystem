namespace CRM.domain.Models;

public class RetentionFilter
{
    public string Search { get; set; } = "";

    /// <summary>"All" | "Active" | "At Risk" | "Dormant" | "Never"</summary>
    public string Status { get; set; } = "All";

    /// <summary>
    /// The "now" used to compute days-since-last-order and the Retention bucket.
    /// Explicit input so the controller stays pure and behavior is identical.
    /// </summary>
    public DateTime Now { get; set; } = DateTime.UtcNow;
}