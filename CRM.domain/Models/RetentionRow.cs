namespace CRM.domain.Models;

public class RetentionRow
{
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string ContactNumber { get; set; } = "";
    public int CurrentPoints { get; set; }
    public decimal PointsValue { get; set; }
    public DateTime? LastOrderAt { get; set; }
    public int? DaysSince { get; set; }
    public int TotalOrders { get; set; }
    public string Retention { get; set; } = "Active";
}