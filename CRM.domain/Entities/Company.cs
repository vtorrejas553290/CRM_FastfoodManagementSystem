namespace CRM.domain.Entities;

public class Company
{
    public int CompanyId { get; set; }

    public string CompanyCode { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CompanyDatabase> CompanyDatabases { get; set; } = new List<CompanyDatabase>();
}