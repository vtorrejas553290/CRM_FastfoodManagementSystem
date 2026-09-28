using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class ReportController : IReportController
{
    public ReportResult GetReport(ReportFilter filter)
    {
        using var db = AppServices.CreateTenantContext();

        var from = filter.FromDate;
        var to = filter.ToDate;
        var type = filter.ReportType ?? "Sales Report";

        // Branch filter computation:
        //  - Admin: uses filter.BranchIdFilter (chosen from the report's dropdown)
        //    null = All Branches → no filter
        //  - Manager/Staff: forced to their own branch (AppServices.CurrentBranchId)
        //    no branch assigned → showNothing
        int? effectiveBranchId = AppServices.CurrentUserIsAdmin
            ? filter.BranchIdFilter
            : AppServices.CurrentBranchId;

        bool showNothing = !AppServices.CurrentUserIsAdmin && !effectiveBranchId.HasValue;

        var result = new ReportResult();

        switch (type)
        {
            case "Sales Report":
                result.Columns.AddRange(new[] { "TransactionId", "PaidAt", "PaymentMethod", "AmountPaid" });

                if (showNothing)
                    break;

                {
                    var query = db.Transactions
                        .Where(t => (from == null || t.PaidAt >= from)
                                 && (to == null || t.PaidAt <= to));

                    if (effectiveBranchId.HasValue)
                        query = query.Where(t => t.BranchId == effectiveBranchId.Value);

                    foreach (var row in query
                        .OrderByDescending(t => t.PaidAt)
                        .Take(500)
                        .ToList()
                        .Select(t => new
                        {
                            t.TransactionId,
                            t.PaidAt,
                            t.PaymentMethod,
                            t.AmountPaid
                        }))
                    {
                        result.Rows.Add(new ReportRow
                        {
                            Cells =
                            {
                                ["TransactionId"] = row.TransactionId,
                                ["PaidAt"] = row.PaidAt,
                                ["PaymentMethod"] = row.PaymentMethod,
                                ["AmountPaid"] = row.AmountPaid
                            }
                        });
                    }
                }
                break;

            case "Inventory Report":
                result.Columns.AddRange(new[] { "Product", "QuantityOnHand", "ReorderLevel", "Status" });

                // NOTE — Inventories table has no BranchId column.
                // This report is intentionally unfiltered (global stock view).

                foreach (var row in db.Inventories
                    .Include(i => i.Product)
                    .ToList()
                    .Select(i => new
                    {
                        Product = i.Product?.ProductName ?? "(deleted)",
                        i.QuantityOnHand,
                        i.ReorderLevel,
                        Status = i.QuantityOnHand <= i.ReorderLevel ? "LOW" : "OK"
                    }))
                {
                    result.Rows.Add(new ReportRow
                    {
                        Cells =
                        {
                            ["Product"] = row.Product,
                            ["QuantityOnHand"] = row.QuantityOnHand,
                            ["ReorderLevel"] = row.ReorderLevel,
                            ["Status"] = row.Status
                        }
                    });
                }
                break;

            case "Customer Report":
                result.Columns.AddRange(new[] { "CustomerName", "CurrentPoints", "IsActive", "CreatedAt" });

                if (showNothing)
                    break;

                {
                    var query = db.Customers
                        .Where(c => (from == null || c.CreatedAt >= from)
                                 && (to == null || c.CreatedAt <= to));

                    if (effectiveBranchId.HasValue)
                        query = query.Where(c => c.BranchId == effectiveBranchId.Value);

                    foreach (var row in query
                        .OrderByDescending(c => c.CreatedAt)
                        .Take(500)
                        .ToList()
                        .Select(c => new
                        {
                            c.CustomerName,
                            c.CurrentPoints,
                            c.IsActive,
                            c.CreatedAt
                        }))
                    {
                        result.Rows.Add(new ReportRow
                        {
                            Cells =
                            {
                                ["CustomerName"] = row.CustomerName,
                                ["CurrentPoints"] = row.CurrentPoints,
                                ["IsActive"] = row.IsActive,
                                ["CreatedAt"] = row.CreatedAt
                            }
                        });
                    }
                }
                break;

            case "Feedback Report":
                result.Columns.AddRange(new[] { "Rating", "Status", "SubmittedAt" });

                if (showNothing)
                    break;

                {
                    var query = db.CustomerFeedbacks
                        .Where(f => (from == null || f.SubmittedAt >= from)
                                 && (to == null || f.SubmittedAt <= to));

                    if (effectiveBranchId.HasValue)
                        query = query.Where(f => f.BranchId == effectiveBranchId.Value);

                    foreach (var row in query
                        .OrderByDescending(f => f.SubmittedAt)
                        .Take(500)
                        .ToList()
                        .Select(f => new
                        {
                            f.Rating,
                            f.Status,
                            f.SubmittedAt
                        }))
                    {
                        result.Rows.Add(new ReportRow
                        {
                            Cells =
                            {
                                ["Rating"] = row.Rating,
                                ["Status"] = row.Status,
                                ["SubmittedAt"] = row.SubmittedAt
                            }
                        });
                    }
                }
                break;

            case "Promotions Report":
                result.Columns.AddRange(new[] { "PromotionCode", "StartDate", "EndDate", "IsActive" });

                // NOTE — Promotions table has no BranchId column.
                // This report is intentionally unfiltered (global promotions).

                foreach (var row in db.Promotions
                    .Where(p => (from == null || p.StartDate >= from)
                             && (to == null || p.EndDate <= to))
                    .ToList()
                    .Select(p => new
                    {
                        p.PromotionCode,
                        p.StartDate,
                        p.EndDate,
                        p.IsActive
                    }))
                {
                    result.Rows.Add(new ReportRow
                    {
                        Cells =
                        {
                            ["PromotionCode"] = row.PromotionCode,
                            ["StartDate"] = row.StartDate,
                            ["EndDate"] = row.EndDate,
                            ["IsActive"] = row.IsActive
                        }
                    });
                }
                break;
        }

        return result;
    }
}