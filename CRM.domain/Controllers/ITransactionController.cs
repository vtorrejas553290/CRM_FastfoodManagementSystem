using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface ITransactionController
{
    /// <summary>
    /// Returns the full filtered list, ordered by PaidAt descending.
    /// Pagination is the form's responsibility.
    /// </summary>
    List<TransactionRow> GetTransactions(TransactionFilter filter);
}