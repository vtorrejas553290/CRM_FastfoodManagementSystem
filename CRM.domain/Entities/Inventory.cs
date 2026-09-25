namespace CRM.domain.Entities;

public class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public decimal QuantityOnHand { get; set; }

    public decimal ReorderLevel { get; set; } = 10;

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    public Product? Product { get; set; }
}