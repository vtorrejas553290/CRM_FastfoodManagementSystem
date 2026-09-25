namespace CRM.winForms;

public static class UserSession
{
    public static int UserId { get; set; }

    public static string Username { get; set; } = string.Empty;

    public static string FullName { get; set; } = string.Empty;

    public static string RoleCode { get; set; } = string.Empty;

    public static string RoleName { get; set; } = string.Empty;

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
    }
}