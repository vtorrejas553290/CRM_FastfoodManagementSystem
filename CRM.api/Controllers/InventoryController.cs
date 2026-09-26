using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class InventoryController : IInventoryController
{
    public List<InventoryRow> GetInventory(InventoryFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        bool showArchived = filter.ShowArchived;

        var query = db.Inventories
            .Include(x => x.Product)
            .AsNoTracking()
            .AsQueryable();

        if (showArchived)
            query = query.Where(x => x.Product != null && !x.Product.IsActive);
        else
            query = query.Where(x => x.Product != null && x.Product.IsActive);

        return query
            .Where(x => string.IsNullOrWhiteSpace(search) ||
                        x.Product!.ProductName.ToLower().Contains(search) ||
                        x.Product.ProductCode.ToLower().Contains(search))
            .OrderBy(x => x.Product!.ProductName)
            .Select(x => new InventoryRow
            {
                InventoryId = x.InventoryId,
                ProductCode = x.Product!.ProductCode,
                Product = x.Product.ProductName,
                QuantityOnHand = x.QuantityOnHand,
                ReorderLevel = x.ReorderLevel,
                Alert = x.QuantityOnHand <= x.ReorderLevel ? "⚠ REORDER" : "OK",
                LastUpdatedAt = x.LastUpdatedAt
            })
            .ToList();
    }
}