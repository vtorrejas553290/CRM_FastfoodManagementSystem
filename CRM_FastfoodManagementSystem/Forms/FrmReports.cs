using CRM.api.Controllers;
using CRM.domain.Controllers;
using CRM.domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;

namespace CRM.winForms.Forms;

public partial class FrmReports : Form
{
    private readonly IReportController _controller = new ReportController();

    // ---- Report state ----
    private List<string> _columns = new();
    private List<ReportRow> _allRows = new();
    private int _currentPage = 1;

    private bool _syncingDates = false;

    public FrmReports()
    {
        InitializeComponent();
        ApplyTheme();

        Load += FrmReports_Load;
        btnExportPdf.Click += (_, __) => ExportToPdf();

        cmbReportType.SelectedIndexChanged += (_, __) =>
        {
            _currentPage = 1;
            GenerateReport();
        };

        cmbDateRange.SelectedIndexChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            SyncDatePickersFromRange();
            _syncingDates = false;

            _currentPage = 1;
            GenerateReport();
        };

        dtpFromDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateRange.SelectedItem?.ToString() != "Custom")
                cmbDateRange.SelectedItem = "Custom";
            _syncingDates = false;

            _currentPage = 1;
            GenerateReport();
        };

        dtpToDate.ValueChanged += (_, __) =>
        {
            if (_syncingDates) return;

            _syncingDates = true;
            if (cmbDateRange.SelectedItem?.ToString() != "Custom")
                cmbDateRange.SelectedItem = "Custom";
            _syncingDates = false;

            _currentPage = 1;
            GenerateReport();
        };

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

        

        AppTheme.StyleLabel(lblReportType);
        AppTheme.StyleLabel(lblDateRange);
        AppTheme.StyleLabel(lblFromDate);
        AppTheme.StyleLabel(lblToDate);
        AppTheme.StyleInput(cmbReportType);
        AppTheme.StyleInput(cmbDateRange);
        AppTheme.StyleSecondaryButton(btnExportPdf);
        AppTheme.StyleGrid(gridReport);
        AppTheme.StyleLabel(lblStatus);

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
            "Today", "Last 7 Days", "Last 30 Days", "All Time", "Custom"
        });

        cmbPageSize.Items.Clear();
        cmbPageSize.Items.AddRange(new object[] { "10", "25", "50", "100", "All" });
        cmbPageSize.SelectedIndex = 1;

        _syncingDates = true;
        cmbDateRange.SelectedIndex = 1;   // "Last 7 Days"
        SyncDatePickersFromRange();
        _syncingDates = false;

        GenerateReport();
    }

    private void SyncDatePickersFromRange()
    {
        var now = DateTime.UtcNow;
        var sel = cmbDateRange.SelectedItem?.ToString() ?? "Last 7 Days";

        switch (sel)
        {
            case "Today":
                dtpFromDate.Value = now.Date;
                dtpToDate.Value = now.Date;
                break;
            case "Last 7 Days":
                dtpFromDate.Value = now.AddDays(-7).Date;
                dtpToDate.Value = now.Date;
                break;
            case "Last 30 Days":
                dtpFromDate.Value = now.AddDays(-30).Date;
                dtpToDate.Value = now.Date;
                break;
            case "All Time":
                dtpFromDate.Value = new DateTime(2000, 1, 1);
                dtpToDate.Value = now.Date;
                break;
            case "Custom":
                // leave current picker values alone
                break;
        }
    }

    private (DateTime? from, DateTime? to) GetFromTo()
    {
        var sel = cmbDateRange.SelectedItem?.ToString() ?? "Last 7 Days";

        if (sel == "All Time")
            return (null, null);

        DateTime from = dtpFromDate.Value.Date;
        DateTime to = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
        return (from, to);
    }

    // ============================================================
    //  DATA LOAD
    // ============================================================

    private void GenerateReport()
    {
        try
        {
            var (from, to) = GetFromTo();

            var filter = new ReportFilter
            {
                ReportType = cmbReportType.SelectedItem?.ToString() ?? "Sales Report",
                DateRange = cmbDateRange.SelectedItem?.ToString() ?? "Last 7 Days",
                FromDate = from,
                ToDate = to
            };

            var result = _controller.GetReport(filter);

            _columns = result.Columns;
            _allRows = result.Rows;

            if (_currentPage > TotalPages) _currentPage = Math.Max(1, TotalPages);

            RenderPage();

            lblStatus.Text = $"Generated {_allRows.Count} rows at {DateTime.Now:HH:mm:ss}";
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
            int total = _allRows.Count;
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
        List<ReportRow> pageRows;

        if (size == int.MaxValue)
            pageRows = _allRows;
        else
            pageRows = _allRows.Skip((_currentPage - 1) * size).Take(size).ToList();

        var table = BuildDataTable(pageRows);
        gridReport.DataSource = table;

        int total = TotalPages;
        lblPageInfo.Text = $"Page {_currentPage} of {total}";

        int first = _allRows.Count == 0
            ? 0
            : ((_currentPage - 1) * (size == int.MaxValue ? _allRows.Count : size)) + 1;
        int last = size == int.MaxValue
            ? _allRows.Count
            : Math.Min(_currentPage * size, _allRows.Count);
        lblShowing.Text = $"Showing {first}–{last} of {_allRows.Count}";

        bool multiPage = total > 1;
        btnFirstPage.Enabled = multiPage && _currentPage > 1;
        btnPrevPage.Enabled = multiPage && _currentPage > 1;
        btnNextPage.Enabled = multiPage && _currentPage < total;
        btnLastPage.Enabled = multiPage && _currentPage < total;
    }

    private DataTable BuildDataTable(List<ReportRow> rows)
    {
        var table = new DataTable();

        foreach (var col in _columns)
        {
            Type t = typeof(string);

            foreach (var row in rows)
            {
                if (row.Cells.TryGetValue(col, out var v) && v is not null)
                {
                    t = v.GetType();
                    break;
                }
            }

            table.Columns.Add(col, t);
        }

        foreach (var row in rows)
        {
            var values = new object?[_columns.Count];
            for (int i = 0; i < _columns.Count; i++)
            {
                row.Cells.TryGetValue(_columns[i], out var v);
                values[i] = v ?? DBNull.Value;
            }
            table.Rows.Add(values);
        }

        return table;
    }

    // ============================================================
    //  PDF EXPORT — full dataset, not just the current page
    // ============================================================

    private void ExportToPdf()
    {
        if (_allRows.Count == 0)
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
            var columns = _columns;

            var rows = _allRows
                .Select(r => columns.Select(c =>
                {
                    r.Cells.TryGetValue(c, out var v);
                    return FormatCell(v);
                }).ToList())
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