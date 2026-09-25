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

    public Customer? Customer { get; set; }
}