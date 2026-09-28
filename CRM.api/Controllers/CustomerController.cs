using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class CustomerController : ICustomerController
{
    public List<CustomerRow> GetCustomers(CustomerFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var showArchived = filter.ShowArchived;

        var query = db.Customers.AsNoTracking();

        // NEW — branch filter.
        // Admin/SuperAdmin → see all branches.
        // Manager/Staff with a branch → only that branch's customers.
        // Manager/Staff without a branch → nothing.
        if (!AppServices.CurrentUserIsAdmin)
        {
            if (!AppServices.CurrentBranchId.HasValue)
                return new List<CustomerRow>();

            query = query.Where(x => x.BranchId == AppServices.CurrentBranchId.Value);
        }

        // Show only the relevant set: archived when toggled, active otherwise
        if (showArchived)
            query = query.Where(x => !x.IsActive);
        else
            query = query.Where(x => x.IsActive);

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.CustomerName.ToLower().Contains(search) ||
                x.CustomerCode.ToLower().Contains(search))
            .OrderBy(x => x.CustomerId)
            .Select(x => new CustomerRow
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                ContactNumber = x.ContactNumber ?? "",
                EmailAddress = x.EmailAddress ?? "",
                Address = x.Address ?? "",
                Status = x.IsActive ? "Active" : "Archived",
                CreatedAt = x.CreatedAt
            })
            .ToList();
    }
}