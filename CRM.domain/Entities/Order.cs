namespace CRM.domain.Entities;

public class Order
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int StaffUserId { get; set; }
    public string OrderType { get; set; } = "DineIn";
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }

    public int PointsRedeemed { get; set; } = 0;
    public int PointsEarned { get; set; } = 0;

    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    // NEW — which branch this order belongs to
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public Customer? Customer { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public Transaction? Transaction { get; set; }
    public ICollection<PromotionRedemption> Redemptions { get; set; } = new List<PromotionRedemption>();
}