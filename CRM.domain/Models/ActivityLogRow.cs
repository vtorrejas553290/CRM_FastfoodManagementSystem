namespace CRM.domain.Models;

/// <summary>
/// Flattened row for the Activity Logs grid.
/// Property names match the anonymous-type projection that was previously
/// bound to gridLogs, so column bindings are unchanged.
/// </summary>
public class ActivityLogRow
{
    public int ActivityLogId { get; set; }
    public DateTime PerformedAt { get; set; }
    public string Username { get; set; } = "";
    public string RoleCode { get; set; } = "";
    public string ActionType { get; set; } = "";
    public string EntityName { get; set; } = "";
    public int? EntityId { get; set; }
    public string Description { get; set; } = "";
}