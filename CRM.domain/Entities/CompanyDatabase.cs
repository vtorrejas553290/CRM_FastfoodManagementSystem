namespace CRM.domain.Entities;

public class CompanyDatabase
{
    public int CompanyDatabaseId { get; set; }

    public int CompanyId { get; set; }

    public string ServerName { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public string CredentialKey { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Company? Company { get; set; }
}