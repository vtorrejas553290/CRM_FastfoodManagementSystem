using CRM.domain.Entities;

namespace CRM.winForms;

/// <summary>
/// Central helper for writing entries into the ActivityLogs table.
/// Call from any form after a significant action.
/// </summary>
public static class ActivityLogger
{
    /// <summary>
    /// Writes a log entry. Never throws — logging failures are swallowed so they
    /// can't break the calling action.
    /// </summary>
    public static void Log(
        string actionType,
        string entityName,
        int? entityId = null,
        string? description = null)
    {
        try
        {
            using var db = AppServices.CreateTenantContext();

            db.ActivityLogs.Add(new ActivityLog
            {
                UserId = UserSession.UserId > 0 ? UserSession.UserId : null,
                Username = string.IsNullOrEmpty(UserSession.Username) ? null : UserSession.Username,
                RoleCode = string.IsNullOrEmpty(UserSession.RoleCode) ? null : UserSession.RoleCode,
                ActionType = actionType,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                PerformedAt = DateTime.UtcNow
            });

            db.SaveChanges();
        }
        catch
        {
            // Swallow — logging should never break the main flow.
        }
    }
}