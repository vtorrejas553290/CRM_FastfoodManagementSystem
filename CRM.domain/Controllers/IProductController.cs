using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IProductController
{
    List<ProductRow> GetProducts(ProductFilter filter);
}