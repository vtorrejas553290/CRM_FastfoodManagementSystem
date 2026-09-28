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

    // NEW — for the branches grid
    public string ManagerName { get; set; } = "";   // first manager's FullName, or "—"
    public int StaffCount { get; set; }             // number of STAFF users in this branch
}