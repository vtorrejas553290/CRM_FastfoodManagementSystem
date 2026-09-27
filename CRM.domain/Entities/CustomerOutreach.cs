namespace CRM.domain.Entities;

public class CustomerOutreach
{
    public int CustomerOutreachId { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? PromotionId { get; set; }
    public Promotion? Promotion { get; set; }

    /// <summary>Short label: "SMS", "Email", "SMS+Email", etc.</summary>
    public string Channel { get; set; } = "";

    /// <summary>The fully rendered message that was logged as "sent".</summary>
    public string MessageBody { get; set; } = "";

    public DateTime SentAt { get; set; }

    public int? SentByUserId { get; set; }
}