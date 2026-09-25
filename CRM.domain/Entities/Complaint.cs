namespace CRM.domain.Entities;

public class Complaint
{
    public int ComplaintId { get; set; }

    // Human-readable ID like "CMP-00042"
    public string ComplaintCode { get; set; } = string.Empty;

    // Who the complaint is about
    public int CustomerId { get; set; }

    // Optional — the order the complaint refers to
    public int? OrderId { get; set; }

    // Classification
    public string Category { get; set; } = "Other";      // Order Accuracy / Food Quality / Service / Cleanliness / Other
    public string Severity { get; set; } = "Medium";      // Low / Medium / High / Critical

    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Workflow
    public string Status { get; set; } = "Open";          // Open / In Progress / Resolved / Closed / Rejected

    // Who logged it
    public int CreatedByUserId { get; set; }

    // Optional assignee
    public int? AssignedToUserId { get; set; }

    // Resolution
    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedAt { get; set; }

    // Timeline
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Archive
    public bool IsArchived { get; set; } = false;

    // Navigation
    public Customer? Customer { get; set; }
    public Order? Order { get; set; }
    public User? CreatedByUser { get; set; }
    public User? AssignedToUser { get; set; }
}