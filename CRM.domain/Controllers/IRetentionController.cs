using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IRetentionController
{
    /// <summary>
    /// Returns the full filtered retention list. The controller applies
    /// the status filter (Active / At Risk / Dormant / Never) after computing
    /// each row's bucket from Now.
    /// Pagination is the form's responsibility.
    /// </summary>
    List<RetentionRow> GetRetention(RetentionFilter filter);
}