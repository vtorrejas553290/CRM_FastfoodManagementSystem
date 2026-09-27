namespace CRM.winForms;

public static class UserSession
{
    public static int UserId { get; set; }

    public static string Username { get; set; } = string.Empty;

    public static string FullName { get; set; } = string.Empty;

    public static string RoleCode { get; set; } = string.Empty;

    public static string RoleName { get; set; } = string.Empty;

    // ---- Tenant context (NEW) ----
    public static int CompanyId { get; set; }

    public static string CompanyCode { get; set; } = string.Empty;

    public static string CompanyName { get; set; } = string.Empty;

    // ---- Plan feature flags (NEW) ----
    public static bool HasMainTransaction { get; set; }

    public static bool HasDataCollection { get; set; }

    public static bool HasBusinessIntelligence { get; set; }

    public static bool HasActions { get; set; }

    public static bool HasBranching { get; set; }

    public static bool IsSuperAdmin => RoleCode == "SUPERADMIN";

    public static bool IsAdmin => RoleCode == "ADMIN";

    public static bool IsManager => RoleCode == "MANAGER";

    public static bool IsStaff => RoleCode == "STAFF";

    public static void Clear()
    {
        UserId = 0;
        Username = string.Empty;
        FullName = string.Empty;
        RoleCode = string.Empty;
        RoleName = string.Empty;

        // ---- NEW resets ----
        CompanyId = 0;
        CompanyCode = string.Empty;
        CompanyName = string.Empty;

        HasMainTransaction = false;
        HasDataCollection = false;
        HasBusinessIntelligence = false;
        HasActions = false;
        HasBranching = false;
    }
}