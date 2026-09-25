using CRM.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;

namespace CRM.winForms.Forms;

public partial class FrmReports : Form
{
    // ---- Pagination state ----
    private DataTable _allRows = new();
    private int _currentPage = 1;

    public FrmReports()
    {
        InitializeComponent();
        ApplyTheme();

        Load += FrmReports_Load;
        btnGenerate.Click += (_, __) => GenerateReport();
        btnExportPdf.Click += (_, __) => ExportToPdf();

        // Reset page when report type or date range changes
        cmbReportType.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            GenerateReport();
        };
        cmbDateRange.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            GenerateReport();
        };

        // Pagination events
        cmbPageSize.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            RenderPage();
        };
        btnFirstPage.Click += (_, __) => GoToPage(1);
        btnPrevPage.Click += (_, __) => GoToPage(_currentPage - 1);
        btnNextPage.Click += (_, __) => GoToPage(_currentPage + 1);
        btnLastPage.Click += (_, __) => GoToPage(TotalPages);
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        BackColor = AppTheme.ContentSurface;
        pnlHeader.BackColor = AppTheme.Surface;
        pnlPager.BackColor = AppTheme.Surface;

        lblTitle.Font = AppTheme.FontHeading;
        lblTitle.ForeColor = AppTheme.TextPrimary;

        AppTheme.StyleLabel(lblReportType);
        AppTheme.StyleLabel(lblDateRange);
        AppTheme.StyleInput(cmbReportType);
        AppTheme.StyleInput(cmbDateRange);
        AppTheme.StyleSecondaryButton(btnGenerate);
        AppTheme.StyleSecondaryButton(btnExportPdf);
        AppTheme.StyleGrid(gridReport);
        AppTheme.StyleLabel(lblStatus);

        // Pager styling
        AppTheme.StyleLabel(lblPageSize);
        AppTheme.StyleLabel(lblPageInfo);
        AppTheme.StyleLabel(lblShowing);
        AppTheme.StyleInput(cmbPageSize);
        AppTheme.StyleSecondaryButton(btnFirstPage);
        AppTheme.StyleSecondaryButton(btnPrevPage);
        AppTheme.StyleSecondaryButton(btnNextPage);
        AppTheme.StyleSecondaryButton(btnLastPage);
    }

    private void FrmReports_Load(object? sender, EventArgs e)
    {
        cmbReportType.Items.Clear();
        cmbReportType.Items.AddRange(new object[]
        {
            "Sales Report",
            "Inventory Report",
            "Customer Report",
            "Feedback Report",
            "Promotions Report"
        });
        cmbReportType.SelectedIndex = 0;

        cmbDateRange.Items.Clear();
        cmbDateRange.Items.AddRange(new object[]
        {
            "Today", "Last 7 Days", "Last 30 Days", "All Time"
        });
        cmbDateRange.SelectedIndex = 1;

        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;   // default 25

        GenerateReport();
    }

    private DateTime? GetFromDate()
    {
        var now = DateTime.UtcNow;
        return cmbDateRange.SelectedItem?.ToString() switch
        {
            "Today" => now.Date,
            "Last 7 Days" => now.AddDays(-7),
            "Last 30 Days" => now.AddDays(-30),
            _ => null
        };
    }

    // ============================================================
    //  DATA LOAD
    // ============================================================

    private void GenerateReport()
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var from = GetFromDate();
            var type = cmbReportType.SelectedItem?.ToString() ?? "Sales Report";

            DataTable table = new();

            switch (type)
            {
                case "Sales Report":
                    table.Columns.Add("TransactionId", typeof(int));
                    table.Columns.Add("PaidAt", typeof(DateTime));
                    table.Columns.Add("PaymentMethod", typeof(string));
                    table.Columns.Add("AmountPaid", typeof(decimal));

                    foreach (var row in db.Transactions
                        .Where(t => from == null || t.PaidAt >= from)
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
                        table.Rows.Add(row.TransactionId, row.PaidAt, row.PaymentMethod, row.AmountPaid);
                    }
                    break;

                case "Inventory Report":
                    table.Columns.Add("Product", typeof(string));
                    table.Columns.Add("QuantityOnHand", typeof(decimal));
                    table.Columns.Add("ReorderLevel", typeof(decimal));
                    table.Columns.Add("Status", typeof(string));

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
                        table.Rows.Add(row.Product, row.QuantityOnHand, row.ReorderLevel, row.Status);
                    }
                    break;

                case "Customer Report":
                    table.Columns.Add("CustomerName", typeof(string));
                    table.Columns.Add("CurrentPoints", typeof(int));
                    table.Columns.Add("IsActive", typeof(bool));
                    table.Columns.Add("CreatedAt", typeof(DateTime));

                    foreach (var row in db.Customers
                        .Where(c => from == null || c.CreatedAt >= from)
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
                        table.Rows.Add(row.CustomerName, row.CurrentPoints, row.IsActive, row.CreatedAt);
                    }
                    break;

                case "Feedback Report":
                    table.Columns.Add("Rating", typeof(int));
                    table.Columns.Add("Status", typeof(string));
                    table.Columns.Add("SubmittedAt", typeof(DateTime));

                    foreach (var row in db.CustomerFeedbacks
                        .Where(f => from == null || f.SubmittedAt >= from)
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
                        table.Rows.Add(row.Rating, row.Status, row.SubmittedAt);
                    }
                    break;

                case "Promotions Report":
                    table.Columns.Add("PromotionCode", typeof(string));
                    table.Columns.Add("StartDate", typeof(DateTime));
                    table.Columns.Add("EndDate", typeof(DateTime));
                    table.Columns.Add("IsActive", typeof(bool));

                    foreach (var row in db.Promotions
                        .ToList()
                        .Select(p => new
                        {
                            p.PromotionCode,
                            p.StartDate,
                            p.EndDate,
                            p.IsActive
                        }))
                    {
                        table.Rows.Add(row.PromotionCode, row.StartDate, row.EndDate, row.IsActive);
                    }
                    break;
            }

            _allRows = table;

            // Clamp page if new report has fewer rows
            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            lblStatus.Text = $"Generated {_allRows.Rows.Count} rows at {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ============================================================
    //  PAGINATION
    // ============================================================

    private int PageSize
    {
        get
        {
            var s = cmbPageSize.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(s) || s == "All") return int.MaxValue;
            return int.TryParse(s, out int n) && n > 0 ? n : 25;
        }
    }

    private int TotalPages
    {
        get
        {
            if (PageSize == int.MaxValue) return 1;
            int total = _allRows.Rows.Count;
            return Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        }
    }

    private void GoToPage(int page)
    {
        int target = Math.Clamp(page, 1, TotalPages);
        if (target == _currentPage) return;

        _currentPage = target;
        RenderPage();
    }

    private void RenderPage()
    {
        int size = PageSize;
        DataTable pageTable;

        if (size == int.MaxValue)
        {
            pageTable = _allRows;
        }
        else
        {
            pageTable = _allRows.Clone();   // same schema, no rows
            int skip = (_currentPage - 1) * size;
            int take = Math.Min(size, _allRows.Rows.Count - skip);

            for (int i = 0; i < take; i++)
                pageTable.ImportRow(_allRows.Rows[skip + i]);
        }

        gridReport.DataSource = pageTable;

        int total = TotalPages;
        lblPageInfo.Text = $"Page {_currentPage} of {total}";

        int first = _allRows.Rows.Count == 0
            ? 0
            : ((_currentPage - 1) * (size == int.MaxValue ? _allRows.Rows.Count : size)) + 1;
        int last = size == int.MaxValue
            ? _allRows.Rows.Count
            : Math.Min(_currentPage * size, _allRows.Rows.Count);
        lblShowing.Text = $"Showing {first}–{last} of {_allRows.Rows.Count}";

        bool multiPage = total > 1;
        btnFirstPage.Enabled = multiPage && _currentPage > 1;
        btnPrevPage.Enabled = multiPage && _currentPage > 1;
        btnNextPage.Enabled = multiPage && _currentPage < total;
        btnLastPage.Enabled = multiPage && _currentPage < total;
    }

    // ============================================================
    //  PDF EXPORT — uses the FULL dataset, not just the current page
    // ============================================================

    private void ExportToPdf()
    {
        if (_allRows.Rows.Count == 0)
        {
            MessageBox.Show("Nothing to export. Generate a report first.",
                "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf",
            FileName = $"{cmbReportType.SelectedItem}_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
        };

        if (sfd.ShowDialog() != DialogResult.OK) return;

        try
        {
            var columns = _allRows.Columns
                .Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var rows = _allRows.Rows
                .Cast<DataRow>()
                .Select(r => columns.Select(c => FormatCell(r[c])).ToList())
                .ToList();

            var reportTitle = cmbReportType.SelectedItem?.ToString() ?? "Report";
            var rangeText = cmbDateRange.SelectedItem?.ToString() ?? "All Time";
            var generatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            QuestPDF.Settings.License = LicenseType.Community;

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontSize(9));

                    page.Header().Column(col =>
                    {
                        col.Item().Text(reportTitle).FontSize(16).Bold();
                        col.Item().Text($"Range: {rangeText}  |  Generated: {generatedAt}  |  Rows: {rows.Count}")
                            .FontSize(9).FontColor(Colors.Grey.Darken1);
                    });

                    page.Content().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            foreach (var _ in columns)
                                cols.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (var col in columns)
                            {
                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(4)
                                    .Text(col).Bold();
                            }
                        });

                        foreach (var row in rows)
                        {
                            foreach (var cell in row)
                            {
                                table.Cell().BorderBottom(0.5f)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Padding(4)
                                    .Text(cell);
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Page ");
                        t.CurrentPageNumber();
                        t.Span(" of ");
                        t.TotalPages();
                    });
                });
            }).GeneratePdf(sfd.FileName);

            lblStatus.Text = $"Exported {rows.Count} rows to {sfd.FileName}";

            var open = MessageBox.Show("PDF exported. Open it now?",
                "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (open == DialogResult.Yes)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = sfd.FileName,
                    UseShellExecute = true
                });
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string FormatCell(object? value)
    {
        if (value is null || value == DBNull.Value) return "";

        return value switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd HH:mm"),
            decimal d => d.ToString("N2"),
            bool b => b ? "Yes" : "No",
            _ => value.ToString() ?? ""
        };
    }
}