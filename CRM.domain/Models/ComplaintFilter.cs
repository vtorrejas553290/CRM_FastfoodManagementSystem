namespace CRM.domain.Models;

public class ComplaintFilter
{
    public string Search { get; set; } = "";

    /// <summary>"All" or one of: Order Accuracy, Food Quality, Service, Cleanliness</summary>
    public string Category { get; set; } = "All";

    /// <summary>"All" or one of: Open, In Progress, Resolved, Closed, Rejected</summary>
    public string Status { get; set; } = "All";

    /// <summary>When true, query archived rows; when false, query non-archived rows.</summary>
    public bool ShowArchived { get; set; }
}