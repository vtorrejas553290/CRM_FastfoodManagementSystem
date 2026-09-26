using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IInventoryController
{
    /// <summary>
    /// Returns the full filtered inventory list, ordered by product name.
    /// Pagination is the form's responsibility.
    /// </summary>
    List<InventoryRow> GetInventory(InventoryFilter filter);
}