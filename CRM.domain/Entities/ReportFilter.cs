namespace CRM.domain.Models;

public class ReportFilter
{
    /// <summary>
    /// "Sales Report" | "Inventory Report" | "Customer Report" |
    /// "Feedback Report" | "Promotions Report"
    /// </summary>
    public string ReportType { get; set; } = "Sales Report";

    /// <summary>
    /// "Today" | "Last 7 Days" | "Last 30 Days" | "All Time" | "Custom"
    /// When "Custom", FromDate and ToDate drive the range.
    /// </summary>
    public string DateRange { get; set; } = "Last 7 Days";

    /// <summary>Inclusive lower bound, or null for "no lower bound".</summary>
    public DateTime? FromDate { get; set; }

    /// <summary>Inclusive upper bound, or null for "no upper bound".</summary>
    public DateTime? ToDate { get; set; }
}