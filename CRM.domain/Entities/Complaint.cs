namespace CRM.domain.Entities;

public class Complaint
{
    public int ComplaintId { get; set; }

    public string ComplaintCode { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public int? OrderId { get; set; }

    public string Category { get; set; } = "Other";
    public string Severity { get; set; } = "Medium";

    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "Open";

    public int CreatedByUserId { get; set; }

    public int? AssignedToUserId { get; set; }

    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public bool IsArchived { get; set; } = false;

    // NEW — which branch this complaint belongs to
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public Customer? Customer { get; set; }
    public Order? Order { get; set; }
    public User? CreatedByUser { get; set; }
    public User? AssignedToUser { get; set; }
}