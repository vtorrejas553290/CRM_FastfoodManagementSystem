using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class ProductController : IProductController
{
    public List<ProductRow> GetProducts(ProductFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        bool showArchived = filter.ShowArchived;

        var query = db.Products
            .Include(x => x.Category)
            .Include(x => x.Inventory)
            .AsNoTracking()
            .AsQueryable();

        // Filter by archive state
        if (showArchived)
            query = query.Where(x => !x.IsActive);
        else
            query = query.Where(x => x.IsActive);

        return query
            .Where(x => string.IsNullOrWhiteSpace(search) ||
                        x.ProductName.ToLower().Contains(search) ||
                        x.ProductCode.ToLower().Contains(search))
            .OrderBy(x => x.ProductId)
            .Select(x => new ProductRow
            {
                ProductId = x.ProductId,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                Category = x.Category != null ? x.Category.CategoryName : "(none)",
                UnitPrice = x.UnitPrice,
                Qty = x.Inventory != null ? x.Inventory.QuantityOnHand : 0,
                Status = x.IsActive ? "Active" : "Archived"
            })
            .ToList();
    }
}