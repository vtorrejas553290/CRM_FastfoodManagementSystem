namespace CRM.domain.Entities;

public class Branch
{
    public int BranchId { get; set; }

    public string BranchCode { get; set; } = string.Empty;

    public string BranchName { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}