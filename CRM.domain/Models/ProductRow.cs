namespace CRM.domain.Models;

public class ProductRow
{
    public int ProductId { get; set; }
    public string ProductCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public decimal Qty { get; set; }
    public string Status { get; set; } = "";
}