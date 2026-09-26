namespace CRM.domain.Models;

public class InventoryRow
{
    public int InventoryId { get; set; }
    public string ProductCode { get; set; } = "";
    public string Product { get; set; } = "";
    public decimal QuantityOnHand { get; set; }
    public decimal ReorderLevel { get; set; }
    public string Alert { get; set; } = "";
    public DateTime LastUpdatedAt { get; set; }
}