namespace CRM.domain.Models;

public class PromotionRow
{
    public int PromotionId { get; set; }
    public string PromotionCode { get; set; } = "";
    public string PromotionName { get; set; } = "";
    public string Discount { get; set; } = "";
    public string MinPurchase { get; set; } = "";
    public string Validity { get; set; } = "";
    public string State { get; set; } = "";
}