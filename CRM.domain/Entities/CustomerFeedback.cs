namespace CRM.domain.Entities;

public class CustomerFeedback
{
    public int CustomerFeedbackId { get; set; }

    public int CustomerId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public string? Category { get; set; }

    public string Status { get; set; } = "New";

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    // NEW — which branch this feedback belongs to
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public Customer? Customer { get; set; }
}