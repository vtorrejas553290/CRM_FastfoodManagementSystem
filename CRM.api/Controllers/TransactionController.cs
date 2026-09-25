using CRM.domain.Controllers;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class TransactionController : ITransactionController
{
    public List<TransactionRow> GetTransactions(TransactionFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var search = (filter.Search ?? "").Trim().ToLower();
        var method = filter.Method ?? "All";
        var dateRange = filter.DateRange ?? "All";

        var query = db.Transactions
            .Include(x => x.Order!)
                .ThenInclude(o => o.Customer)
            .AsNoTracking();

        if (method != "All")
            query = query.Where(x => x.PaymentMethod == method);

        var now = filter.Now;

        if (dateRange == "Today")
            query = query.Where(x => x.PaidAt.Date == now.Date);
        else if (dateRange == "Last 7 Days")
            query = query.Where(x => x.PaidAt >= now.AddDays(-7));
        else if (dateRange == "Last 30 Days")
            query = query.Where(x => x.PaidAt >= now.AddDays(-30));

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                (x.Order != null && x.Order.OrderCode.ToLower().Contains(search)) ||
                (x.Order!.Customer != null && x.Order.Customer.CustomerName.ToLower().Contains(search)))
            .OrderByDescending(x => x.PaidAt)
            .Select(x => new TransactionRow
            {
                TransactionId = x.TransactionId,
                OrderCode = x.Order!.OrderCode,
                Customer = x.Order.Customer != null ? x.Order.Customer.CustomerName : "(deleted)",
                PaymentMethod = x.PaymentMethod,
                Total = x.Order.TotalAmount,
                AmountPaid = x.AmountPaid,
                ChangeDue = x.ChangeDue,
                Reference = x.ReferenceNumber ?? "-",
                PaidAt = x.PaidAt
            })
            .ToList();
    }
}