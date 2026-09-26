using CRM.domain.Models;

namespace CRM.domain.Controllers;

public class ReportResult
{
    public List<string> Columns { get; set; } = new();
    public List<ReportRow> Rows { get; set; } = new();
}

public interface IReportController
{
    ReportResult GetReport(ReportFilter filter);
}