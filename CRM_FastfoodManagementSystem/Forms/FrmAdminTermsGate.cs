using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CRM.winForms.Forms;

public partial class FrmAdminTermsGate : Form
{
    private readonly int _termsId;
    private bool _accepted;

    public FrmAdminTermsGate(int termsId)
    {
        _termsId = termsId;
        InitializeComponent();

        ApplyTheme();

        btnAccept.Click += BtnAccept_Click;
        btnDecline.Click += BtnDecline_Click;
        chkIAccept.CheckedChanged += (_, __) => btnAccept.Enabled = chkIAccept.Checked;

        Load += FrmAdminTermsGate_Load;
    }

    public bool WasAccepted => _accepted;

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this, isDialog: true);
        lblHeader.ForeColor = AppTheme.TextPrimary;
        lblTitle.ForeColor = AppTheme.TextPrimary;
        lblVersion.ForeColor = AppTheme.TextSecondary;
        AppTheme.StylePrimaryButton(btnAccept);
        AppTheme.StyleSecondaryButton(btnDecline);
        chkIAccept.Font = AppTheme.FontBody;
        chkIAccept.ForeColor = AppTheme.TextPrimary;

        // Force textbox settings
        txtContent.Multiline = true;
        txtContent.ReadOnly = true;
        txtContent.WordWrap = true;
        txtContent.ScrollBars = ScrollBars.Vertical;
        txtContent.AcceptsReturn = true;
        txtContent.BackColor = System.Drawing.Color.White;
        txtContent.ForeColor = System.Drawing.Color.Black;
        txtContent.Font = new System.Drawing.Font("Consolas", 10F);

        // Force its size
        txtContent.Location = new System.Drawing.Point(20, 120);
        txtContent.Size = new System.Drawing.Size(
            Math.Max(300, this.ClientSize.Width - 40),
            340);
    }

    private void FrmAdminTermsGate_Load(object? sender, EventArgs e)
    {
        try
        {
            using var db = AppServices.CreateTenantContext();
            var terms = db.TermsAndConditions
                .AsNoTracking()
                .FirstOrDefault(x => x.TermsAndConditionId == _termsId);

            if (terms is null)
            {
                MessageBox.Show("Terms not found.", "Error");
                Close();
                return;
            }

            lblVersion.Text = $"Version: {terms.Version}    |    Effective: {terms.EffectiveFrom:yyyy-MM-dd}";
            lblTitle.Text = terms.Title;

            string formatted = FormatTerms(terms.Content);

            if (!formatted.Contains('\n') && formatted.Length > 80)
                formatted = ForceWrap(formatted, 90);

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

    private void BtnAccept_Click(object? sender, EventArgs e)
    {
        _accepted = true;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnDecline_Click(object? sender, EventArgs e)
    {
        var confirm = MessageBox.Show(
            "If you decline, you will be logged out. Continue?",
            "Decline Terms",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        _accepted = false;
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // =========== Formatter ===========

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
                foreach (var w in WrapLine(line.Trim(), 90)) sb.AppendLine(w);
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }

        var sentences = SplitIntoSentences(text);
        var output = new StringBuilder();
        for (int i = 0; i < sentences.Count; i++)
        {
            foreach (var w in WrapLine($"{i + 1}. " + sentences[i], 90))
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