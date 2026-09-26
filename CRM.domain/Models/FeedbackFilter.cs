namespace CRM.domain.Models;

public class FeedbackFilter
{
    public string Search { get; set; } = "";

    /// <summary>"All" | "New" | "Reviewed" | "Resolved" | "Archived"</summary>
    public string Status { get; set; } = "All";

    /// <summary>Raw text from cmbFilterStars, e.g. "All" or "5 ★".</summary>
    public string StarsText { get; set; } = "All";
}