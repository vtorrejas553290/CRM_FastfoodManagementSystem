namespace CRM.domain.Models;

/// <summary>
/// One row in a report. Column names are defined by the report type;
/// values are stored by column name. This lets us keep one type
/// for all five report shapes.
/// </summary>
public class ReportRow
{
    public Dictionary<string, object?> Cells { get; set; } = new();
}