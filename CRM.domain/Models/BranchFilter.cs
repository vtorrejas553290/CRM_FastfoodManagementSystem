namespace CRM.domain.Models;

public class BranchFilter
{
    /// <summary>Free-text search across BranchCode and BranchName.</summary>
    public string Search { get; set; } = "";

    /// <summary>When true, query archived (inactive) rows; when false, active rows.</summary>
    public bool ShowArchived { get; set; }

    /// <summary>
    /// Which tenant DB to query. Set from UserSession.CompanyId.
    /// Required — the controller uses it to route to the correct tenant instance.
    /// </summary>
    public int CompanyId { get; set; }
}