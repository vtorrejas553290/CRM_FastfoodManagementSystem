namespace CRM.domain.Models;

public class BranchRow
{
    public int BranchId { get; set; }

    public string BranchCode { get; set; } = "";

    public string BranchName { get; set; } = "";

    public string Address { get; set; } = "";

    public string ContactNumber { get; set; } = "";

    public string Status { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}