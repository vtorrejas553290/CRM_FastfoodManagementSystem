namespace CRM.domain.Models;

/// <summary>
/// One row in a small BI list (Recent Orders, Top Customers, At-Risk).
/// Only the columns present in a given list are populated; the grid binds
/// to whatever columns the form requests from the controller result.
/// </summary>
public class BiRow
{
    public Dictionary<string, object?> Cells { get; set; } = new();
}