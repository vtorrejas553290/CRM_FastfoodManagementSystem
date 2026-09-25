namespace CRM.domain.Entities;

public class ActivityLog
{
    public int ActivityLogId { get; set; }

    public int? UserId { get; set; }

    public string? Username { get; set; }

    public string? RoleCode { get; set; }

    public string ActionType { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public int? EntityId { get; set; }

    public string? Description { get; set; }

    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
}