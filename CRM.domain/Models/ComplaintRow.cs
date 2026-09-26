namespace CRM.domain.Models;

public class ComplaintRow
{
    public int ComplaintId { get; set; }
    public string ComplaintCode { get; set; } = "";
    public string Category { get; set; } = "";
    public string Severity { get; set; } = "";
    public string Subject { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Status { get; set; } = "";
    public string AssignedTo { get; set; } = "";
    public DateTime SubmittedAt { get; set; }
    public bool IsArchived { get; set; }
}