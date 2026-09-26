using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IActivityLogController
{
    /// <summary>
    /// Returns the full filtered list, ordered by PerformedAt descending,
    /// capped at 1000 rows. Pagination is the form's responsibility.
    /// </summary>
    List<ActivityLogRow> GetActivityLogs(ActivityLogFilter filter);
}