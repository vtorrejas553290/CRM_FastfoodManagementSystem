namespace CRM.domain.Models;

public class ProductFilter
{
    public string Search { get; set; } = "";

    /// <summary>When true, query archived rows; when false, query active rows.</summary>
    public bool ShowArchived { get; set; }
}