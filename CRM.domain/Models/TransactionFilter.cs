namespace CRM.domain.Models;

/// <summary>
/// Input parameters for the View Transactions list query.
/// Built by the form from its UI controls.
/// </summary>
public class TransactionFilter
{
    /// <summary>Raw search text (already trimmed/lowercased by the form? No — form passes raw, controller normalizes).</summary>
    public string Search { get; set; } = "";

    /// <summary>"All" | "Cash" | "GCash"</summary>
    public string Method { get; set; } = "All";

    /// <summary>"All" | "Today" | "Last 7 Days" | "Last 30 Days"</summary>
    public string DateRange { get; set; } = "All";

    /// <summary>
    /// The "now" used for date-range math. The form supplied DateTime.UtcNow
    /// inline before; now it's an explicit input so the controller stays pure
    /// and the behavior is identical.
    /// </summary>
    public DateTime Now { get; set; } = DateTime.UtcNow;
}