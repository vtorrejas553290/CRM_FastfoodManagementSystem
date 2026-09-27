namespace CRM.domain.Entities;

public class PromotionCategory
{
    public int PromotionCategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}