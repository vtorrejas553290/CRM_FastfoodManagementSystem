using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Text;
using CRM.infrastructure;

namespace CRM.winForms.Forms;

public partial class FrmTermsViewer : Form
{
    private readonly int _termsId;
    private string _formattedContent = string.Empty;
    private string _title = string.Empty;
    private string _version = string.Empty;
    private string _effectiveDate = string.Empty;

    // Multi-page printing state
    private int _printCharIndex = 0;
    private int _printPageNumber = 1;

    // Fonts — created once, disposed on FormClosed
    private readonly Font _fontTitle = new("Segoe UI", 16F, FontStyle.Bold);
    private readonly Font _fontBody = new("Consolas", 10F);
    private readonly Font _fontMeta = new("Segoe UI", 9F, FontStyle.Italic);
    private readonly Font _fontFooter = new("Segoe UI", 8F);

    public FrmTermsViewer(int termsId)
    {
        _termsId = termsId;
        InitializeComponent();

        ApplyTheme();

        btnPrint.Click += BtnPrint_Click;
        btnClose.Click += (_, __) => Close();

        Load += FrmTermsViewer_Load;
        Resize += FrmTermsViewer_Resize;
        FormClosed += FrmTermsViewer_FormClosed;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: false);
        lblVersion.ForeColor = AppTheme.TextSecondary;
        lblTitle.ForeColor = AppTheme.TextPrimary;
        AppTheme.StylePrimaryButton(btnPrint);
        AppTheme.StyleSecondaryButton(btnClose);

        txtContent.Multiline = true;
        txtContent.ReadOnly = true;
        txtContent.WordWrap = true;
        txtContent.ScrollBars = ScrollBars.Vertical;
        txtContent.AcceptsReturn = true;
        txtContent.BackColor = System.Drawing.Color.White;
        txtContent.ForeColor = System.Drawing.Color.Black;
        txtContent.Font = new System.Drawing.Font("Consolas", 11F);
    }

    private void FrmTermsViewer_FormClosed(object? sender, FormClosedEventArgs e)
    {
        _fontTitle.Dispose();
        _fontBody.Dispose();
        _fontMeta.Dispose();
        _fontFooter.Dispose();
    }

    private void FrmTermsViewer_Load(object? sender, EventArgs e)
    {
        LayoutChildren();

        try
        {
            using var db = AppServices.CreateTenantContext();
            var terms = db.TermsAndConditions.AsNoTracking()
                .FirstOrDefault(x => x.TermsAndConditionId == _termsId);

            if (terms is null)
            {
                MessageBox.Show("Terms not found.", "Error");
                Close();
                return;
            }

            Text = $"Terms — {terms.Version}";
            lblVersion.Text = $"Version: {terms.Version}    |    Effective: {terms.EffectiveFrom:yyyy-MM-dd}";
            lblTitle.Text = terms.Title;

            _title = terms.Title;
            _version = terms.Version;
            _effectiveDate = terms.EffectiveFrom.ToString("yyyy-MM-dd");

            string formatted = FormatTerms(terms.Content);

            if (!formatted.Contains('\n') && formatted.Length > 80)
                formatted = ForceWrap(formatted, 100);

            _formattedContent = formatted;
            txtContent.Text = formatted;

            txtContent.SelectionStart = 0;
            txtContent.SelectionLength = 0;
            txtContent.ScrollToCaret();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
            Close();
        }
    }

    private void FrmTermsViewer_Resize(object? sender, EventArgs e)
    {
        LayoutChildren();
    }

    private void LayoutChildren()
    {
        int margin = 20;
        int versionH = 22;
        int titleH = 32;
        int buttonRowH = 50;

        int topY = margin;
        lblVersion.Location = new System.Drawing.Point(margin, topY);
        topY += versionH + 6;

        lblTitle.Location = new System.Drawing.Point(margin, topY);
        topY += titleH + 10;

        int contentWidth = Math.Max(300, this.ClientSize.Width - (margin * 2));
        int contentHeight = Math.Max(200, this.ClientSize.Height - topY - buttonRowH - margin);

        txtContent.Location = new System.Drawing.Point(margin, topY);
        txtContent.Size = new System.Drawing.Size(contentWidth, contentHeight);
        txtContent.Multiline = true;

        int buttonY = this.ClientSize.Height - margin - 40;

        btnPrint.Location = new System.Drawing.Point(margin, buttonY);
        btnPrint.Size = new System.Drawing.Size(180, 38);

        btnClose.Location = new System.Drawing.Point(margin + 190, buttonY);
        btnClose.Size = new System.Drawing.Size(120, 38);
    }

    // ============================================================
    //  PRINT (multi-page)
    // ============================================================

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        _printCharIndex = 0;
        _printPageNumber = 1;

        using var printDoc = new PrintDocument();
        printDoc.DocumentName = $"Terms_{_version}";
        printDoc.PrintPage += PrintPage;

        using var preview = new PrintPreviewDialog
        {
            Document = printDoc,
            Width = 900,
            Height = 700,
            Text = "Print Preview — Terms and Conditions"
        };

        preview.ShowDialog(this);
    }

    private void PrintPage(object? sender, PrintPageEventArgs e)
    {
        if (e.Graphics is null) return;

        var g = e.Graphics;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        float x = e.MarginBounds.Left;
        float y = e.MarginBounds.Top;
        float width = e.MarginBounds.Width;

        // ---- Page header ----
        g.DrawString(_title, _fontTitle, Brushes.Black, x, y);
        y += _fontTitle.GetHeight(g) + 4;

        g.DrawString($"Version: {_version}    |    Effective: {_effectiveDate}",
            _fontMeta, Brushes.Gray, x, y);
        y += _fontMeta.GetHeight(g) + 8;

        using var linePen = new Pen(Color.FromArgb(200, 200, 200), 1);
        g.DrawLine(linePen, x, y, x + width, y);
        y += 10;

        // ---- Reserve footer space ----
        float footerHeight = _fontFooter.GetHeight(g) + 8;
        float bodyBottom = e.MarginBounds.Bottom - footerHeight;
        float bodyHeight = bodyBottom - y;
        var bodyRect = new RectangleF(x, y, width, bodyHeight);

        using var format = new StringFormat(StringFormatFlags.LineLimit)
        {
            Trimming = StringTrimming.Word,
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        string remaining = _formattedContent.Substring(
            Math.Min(_printCharIndex, _formattedContent.Length));

        int charsFitted;
        int linesFilled;

        g.MeasureString(
            remaining,
            _fontBody,
            new SizeF(bodyRect.Width, bodyRect.Height),
            format,
            out charsFitted,
            out linesFilled);

        if (charsFitted <= 0) charsFitted = remaining.Length;

        string chunk = remaining.Substring(0, charsFitted);

        g.DrawString(chunk, _fontBody, Brushes.Black, bodyRect, format);

        _printCharIndex += charsFitted;

        // Don't split mid-word
        if (_printCharIndex < _formattedContent.Length)
        {
            int lastSpace = chunk.LastIndexOfAny(new[] { ' ', '\n', '\t' });
            if (lastSpace > 0 && lastSpace < chunk.Length - 1)
            {
                _printCharIndex -= (chunk.Length - lastSpace - 1);
            }
        }

        bool morePages = _printCharIndex < _formattedContent.Length;

        // ---- Footer ----
        g.DrawString($"Page {_printPageNumber}", _fontFooter, Brushes.Gray,
            x, e.MarginBounds.Bottom - _fontFooter.GetHeight(g));

        using var centeredFooter = new StringFormat
        {
            Alignment = StringAlignment.Center
        };
        g.DrawString("Fastfood MS — Terms and Conditions", _fontFooter, Brushes.Gray,
            new RectangleF(x, e.MarginBounds.Bottom - _fontFooter.GetHeight(g),
                width, _fontFooter.GetHeight(g)), centeredFooter);

        // ---- Continue or stop ----
        if (morePages)
        {
            _printPageNumber++;
            e.HasMorePages = true;
        }
        else
        {
            e.HasMorePages = false;
            _printCharIndex = 0;
            _printPageNumber = 1;
        }
    }

    // ============= Formatter =============

    private static string FormatTerms(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

        var text = raw.Replace("\r\n", "\n").Replace("\r", "\n").Trim();

        if (text.Contains('\n'))
        {
            var sb = new StringBuilder();
            foreach (var line in text.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) { sb.AppendLine(); continue; }
                foreach (var w in WrapLine(line.Trim(), 100)) sb.AppendLine(w);
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        var sentences = SplitIntoSentences(text);
        var output = new StringBuilder();
        for (int i = 0; i < sentences.Count; i++)
        {
            foreach (var w in WrapLine($"{i + 1}. " + sentences[i], 100))
                output.AppendLine(w);
            output.AppendLine();
        }
        return output.ToString().TrimEnd();
    }

    private static List<string> SplitIntoSentences(string text)
    {
        var result = new List<string>();
        var current = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            current.Append(text[i]);
            bool isEnd = text[i] == '.' || text[i] == '!' || text[i] == '?';
            if (isEnd)
            {
                int j = i + 1;
                while (j < text.Length && char.IsWhiteSpace(text[j])) j++;
                if (current.Length > 40)
                {
                    result.Add(current.ToString().Trim());
                    current.Clear();
                }
                i = j - 1;
            }
        }

        if (current.Length > 0) result.Add(current.ToString().Trim());

        if (result.Count <= 1 && text.Length > 80)
        {
            result.Clear();
            for (int i = 0; i < text.Length; i += 80)
            {
                var chunk = text.Substring(i, Math.Min(80, text.Length - i)).Trim();
                if (!string.IsNullOrWhiteSpace(chunk)) result.Add(chunk);
            }
        }

        return result;
    }

    private static IEnumerable<string> WrapLine(string line, int width)
    {
        if (line.Length <= width) { yield return line; yield break; }
        var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        foreach (var word in words)
        {
            if (sb.Length == 0) sb.Append(word);
            else if (sb.Length + 1 + word.Length <= width) sb.Append(' ').Append(word);
            else { yield return sb.ToString(); sb.Clear(); sb.Append(word); }
        }
        if (sb.Length > 0) yield return sb.ToString();
    }

    private static string ForceWrap(string text, int width)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < text.Length; i += width)
            sb.AppendLine(text.Substring(i, Math.Min(width, text.Length - i)));
        return sb.ToString().TrimEnd();
    }
}