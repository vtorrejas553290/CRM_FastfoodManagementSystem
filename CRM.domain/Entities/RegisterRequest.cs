namespace CRM.domain.Entities;

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? ContactNumber { get; set; }

    public int RoleId { get; set; }
}