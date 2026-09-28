namespace CRM.domain.Models;

public class UserFilter
{
    public string Search { get; set; } = "";

    public bool ShowArchived { get; set; }

    public bool IsSuperAdmin { get; set; }
    public bool IsAdmin { get; set; }
    public int CurrentUserId { get; set; }

    public int CompanyId { get; set; }
    public bool ShowAllTenants { get; set; }

    public string RoleCodeFilter { get; set; } = "All";

    // ---- NEW — Branch filter ----
    /// <summary>
    /// null       = no branch filter (show everyone)
    /// > 0        = only users assigned to this branch
    /// -1         = only users with no branch (Unassigned)
    /// Ignored when ShowAllTenants is true (SuperAdmin).
    /// </summary>
    public int? BranchIdFilter { get; set; } = null;
}