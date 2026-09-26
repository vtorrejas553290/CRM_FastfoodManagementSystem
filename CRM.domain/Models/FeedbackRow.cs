namespace CRM.domain.Models;

public class FeedbackRow
{
    public int CustomerFeedbackId { get; set; }
    public string Customer { get; set; } = "";
    public int Rating { get; set; }
    public string Category { get; set; } = "";
    public string Comments { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime SubmittedAt { get; set; }
}