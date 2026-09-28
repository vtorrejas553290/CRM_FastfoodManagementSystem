namespace CRM.domain.Models;

public class BiFilter
{
    /// <summary>
    /// "Last 7 Days" | "Last 30 Days" | "This Month" | "Last Month" |
    /// "This Year" | "Last Year" | "All Time" | "Custom Range"
    /// </summary>
    public string Period { get; set; } = "This Month";

    /// <summary>Inclusive lower bound. Null means "no lower bound".</summary>
    public DateTime? FromDate { get; set; }

    /// <summary>Inclusive upper bound. Null means "no upper bound".</summary>
    public DateTime? ToDate { get; set; }

    /// <summary>"now" passed in so the controller stays pure.</summary>
    public DateTime Now { get; set; } = DateTime.UtcNow;

    // ---- NEW — branch filter ----
    /// <summary>
    /// When set (positive), all BI queries that have a BranchId column
    /// filter to this branch. null = show all branches.
    /// Queries on tables without a BranchId (Promotions, Inventories) are
    /// unaffected.
    /// </summary>
    public int? BranchIdFilter { get; set; } = null;
}