namespace CRM.domain.Entities;

public class Product
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public decimal UnitPrice { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Category? Category { get; set; }

    public Inventory? Inventory { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}