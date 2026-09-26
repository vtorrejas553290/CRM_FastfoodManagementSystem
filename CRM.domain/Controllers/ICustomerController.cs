using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface ICustomerController
{
    List<CustomerRow> GetCustomers(CustomerFilter filter);
}