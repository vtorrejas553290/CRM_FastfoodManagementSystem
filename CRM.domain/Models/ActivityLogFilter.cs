namespace CRM.domain.Models;

public class ActivityLogFilter
{
    public string Search { get; set; } = "";

    public string Action { get; set; } = "All";

    /// <summary>
    /// "All" | "Today" | "Last 7 Days" | "Last 30 Days" | "Custom".
    /// Only used as a label — the controller drives filtering off FromDate/ToDate.
    /// </summary>
    public string DateRange { get; set; } = "All";

    /// <summary>Inclusive lower bound, or null for "no lower bound".</summary>
    public DateTime? FromDate { get; set; }

    /// <summary>Inclusive upper bound, or null for "no upper bound".</summary>
    public DateTime? ToDate { get; set; }
}