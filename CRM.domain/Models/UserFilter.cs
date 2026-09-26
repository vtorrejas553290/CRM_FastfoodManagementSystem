namespace CRM.domain.Models;

public class UserFilter
{
    public string Search { get; set; } = "";

    /// <summary>When true, query archived rows; when false, query active rows.</summary>
    public bool ShowArchived { get; set; }

    /// <summary>
    /// Who is asking. Drives the role-scoping:
    ///  - SuperAdmin: sees ADMIN role users + self
    ///  - Admin: sees MANAGER + STAFF role users
    ///  - anyone else: sees nothing (form blanks the grid before calling)
    /// </summary>
    public bool IsSuperAdmin { get; set; }
    public bool IsAdmin { get; set; }
    public int CurrentUserId { get; set; }
}