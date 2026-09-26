namespace CRM.domain.Models;

public class InventoryFilter
{
    public string Search { get; set; } = "";

    /// <summary>
    /// When true, query inventory whose product is inactive (archived);
    /// when false, query inventory whose product is active.
    /// </summary>
    public bool ShowArchived { get; set; }
}