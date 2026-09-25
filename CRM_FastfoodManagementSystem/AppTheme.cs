using System.Drawing;
using System.Windows.Forms;
namespace CRM.winForms;

/// <summary>
/// Central theme for the entire CRM application.
/// Colors are derived from the IT13 French Blue palette
/// (Hue / Shade / Tone spectrum).
///
///  Base Hue: RGB(26, 99, 148) #1A6394  → French Blue
/// </summary>
public static class AppTheme
{
    // =========================================================
    //  PRIMARY PALETTE (Hue scale — French Blue)
    // =========================================================
    public static readonly Color Primary10 = Color.FromArgb(26, 99, 148);
    public static readonly Color Primary20 = Color.FromArgb(51, 117, 160);
    public static readonly Color Primary30 = Color.FromArgb(77, 134, 172);
    public static readonly Color Primary40 = Color.FromArgb(102, 152, 184);
    public static readonly Color Primary50 = Color.FromArgb(128, 169, 196);
    public static readonly Color Primary60 = Color.FromArgb(153, 188, 208);
    public static readonly Color Primary70 = Color.FromArgb(179, 206, 220);
    public static readonly Color Primary80 = Color.FromArgb(204, 223, 232);
    public static readonly Color Primary90 = Color.FromArgb(230, 239, 244);
    public static readonly Color Primary100 = Color.FromArgb(255, 255, 255);

    // =========================================================
    //  SHADE PALETTE (Adding Black — Deep Navy)
    // =========================================================
    public static readonly Color Shade10 = Color.FromArgb(0, 74, 122);
    public static readonly Color Shade20 = Color.FromArgb(0, 66, 109);
    public static readonly Color Shade30 = Color.FromArgb(0, 58, 95);
    public static readonly Color Shade40 = Color.FromArgb(0, 49, 82);
    public static readonly Color Shade50 = Color.FromArgb(0, 41, 68);
    public static readonly Color Shade60 = Color.FromArgb(0, 33, 54);
    public static readonly Color Shade70 = Color.FromArgb(0, 25, 41);
    public static readonly Color Shade80 = Color.FromArgb(0, 16, 27);
    public static readonly Color Shade90 = Color.FromArgb(0, 8, 14);
    public static readonly Color Shade100 = Color.FromArgb(0, 0, 0);

    // =========================================================
    //  TONE PALETTE (Adding Neutral Gray)
    // =========================================================
    public static readonly Color Tone10 = Color.FromArgb(13, 87, 135);
    public static readonly Color Tone20 = Color.FromArgb(26, 92, 134);
    public static readonly Color Tone30 = Color.FromArgb(38, 97, 133);
    public static readonly Color Tone40 = Color.FromArgb(51, 101, 132);
    public static readonly Color Tone50 = Color.FromArgb(64, 106, 132);
    public static readonly Color Tone60 = Color.FromArgb(77, 111, 131);
    public static readonly Color Tone70 = Color.FromArgb(90, 115, 130);
    public static readonly Color Tone80 = Color.FromArgb(102, 120, 130);
    public static readonly Color Tone90 = Color.FromArgb(115, 124, 129);
    public static readonly Color Tone100 = Color.FromArgb(128, 128, 128);

    // =========================================================
    //  SEMANTIC COLORS (Use these in forms, NOT raw palette)
    // =========================================================

    /// Brand / primary accent
    public static readonly Color Primary = Primary20;
    public static readonly Color PrimaryHover = Shade10;
    public static readonly Color PrimaryPressed = Shade30;

    /// Secondary button (neutral, quiet action)
    public static readonly Color Secondary = Tone50;
    public static readonly Color SecondaryHover = Tone60;

    /// Main window background
    public static readonly Color Background = Primary90;

    /// Card / panel background
    public static readonly Color Surface = Primary100;

    /// Content surface for the main content area
    public static readonly Color ContentSurface = Color.FromArgb(248, 249, 250);

    /// Sidebar / navigation background
    public static readonly Color SidebarBackground = Primary20;
    public static readonly Color SidebarItem = Primary20;
    public static readonly Color SidebarItemHover = Primary30;
    public static readonly Color SidebarItemHover2 = Color.FromArgb(60, 130, 170);
    public static readonly Color SidebarItemActive = Shade10;
    public static readonly Color SidebarText = Primary100;

    /// Primary text
    public static readonly Color TextPrimary = Shade50;
    public static readonly Color TextSecondary = Tone60;
    public static readonly Color TextMuted = Tone90;
    public static readonly Color TextOnDark = Primary100;

    /// Success messages
    public static readonly Color Success = Color.FromArgb(60, 150, 90);
    public static readonly Color Error = Color.FromArgb(180, 60, 60);
    public static readonly Color Warning = Color.FromArgb(200, 150, 60);
    public static readonly Color Info = Tone10;

    /// Borders and separators
    public static readonly Color Border = Primary70;
    public static readonly Color InputBorder = Primary60;
    public static readonly Color InputBorderFocused = Primary20;

    /// Grid colors
    public static readonly Color GridHeaderBackground = Primary20;
    public static readonly Color GridHeaderText = Primary100;
    public static readonly Color GridRowAlternate = Primary90;
    public static readonly Color GridRowSelected = Primary80;

    // =========================================================
    //  SEMANTIC ACTION COLORS (Buttons)
    // =========================================================

    // Danger (Archive, Delete, Logout, Decline)
    public static readonly Color Danger = Color.FromArgb(192, 57, 43);
    public static readonly Color DangerHover = Color.FromArgb(160, 45, 34);
    public static readonly Color DangerPressed = Color.FromArgb(130, 35, 26);

    // Success (Save, Add, Accept)
    public static readonly Color SuccessGreen = Color.FromArgb(39, 174, 96);
    public static readonly Color SuccessGreenHover = Color.FromArgb(30, 140, 76);

    // Warning (Edit)
    public static readonly Color WarningAmber = Color.FromArgb(243, 156, 18);
    public static readonly Color WarningAmberHover = Color.FromArgb(211, 132, 10);

    // Neutral (Cancel, Close)
    public static readonly Color NeutralGray = Color.FromArgb(108, 122, 137);
    public static readonly Color NeutralGrayHover = Color.FromArgb(90, 102, 115);

    // =========================================================
    //  FONTS
    // =========================================================
    public const string FontFamily = "Segoe UI";

    public static readonly Font FontTitle = new Font(FontFamily, 20F, FontStyle.Bold);
    public static readonly Font FontHeading = new Font(FontFamily, 14F, FontStyle.Bold);
    public static readonly Font FontSubheading = new Font(FontFamily, 11F, FontStyle.Bold);
    public static readonly Font FontBody = new Font(FontFamily, 10F, FontStyle.Regular);
    public static readonly Font FontLabel = new Font(FontFamily, 9.5F, FontStyle.Regular);
    public static readonly Font FontButton = new Font(FontFamily, 10F, FontStyle.Bold);
    public static readonly Font FontSmall = new Font(FontFamily, 8.5F, FontStyle.Regular);

    /// Dedicated font for grid action buttons — smaller so captions fit
    /// inside narrow button columns.
    public static readonly Font FontGridButton = new Font(FontFamily, 9F, FontStyle.Bold);

    // =========================================================
    //  SIZING
    // =========================================================
    public const int ButtonHeight = 38;
    public const int ButtonWidth = 130;
    public const int InputHeight = 30;
    public const int LabelWidth = 130;
    public const int FormPadding = 24;

    // =========================================================
    //  HELPER METHODS — FORM / LAYOUT
    // =========================================================

    public static void ApplyForm(Form form, bool isDialog = false)
    {
        form.BackColor = Background;
        form.Font = FontBody;
        form.ForeColor = TextPrimary;

        if (isDialog)
        {
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
        }
    }

    public static void StyleInput(Control input)
    {
        input.BackColor = Surface;
        input.ForeColor = TextPrimary;
        input.Font = FontBody;
        input.Height = InputHeight;
    }

    public static void StyleLabel(Label lbl, bool isHeading = false)
    {
        lbl.ForeColor = isHeading ? TextPrimary : TextSecondary;
        lbl.Font = isHeading ? FontHeading : FontLabel;
        lbl.BackColor = Color.Transparent;
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.None;
        grid.GridColor = Border;
        grid.EnableHeadersVisualStyles = false;

        grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBackground;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = GridHeaderText;
        grid.ColumnHeadersDefaultCellStyle.Font = FontSubheading;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeaderBackground;
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        grid.DefaultCellStyle.BackColor = Surface;
        grid.DefaultCellStyle.ForeColor = TextPrimary;
        grid.DefaultCellStyle.Font = FontBody;
        grid.DefaultCellStyle.SelectionBackColor = GridRowSelected;
        grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
        grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        grid.RowTemplate.Height = 30;

        grid.AlternatingRowsDefaultCellStyle.BackColor = GridRowAlternate;
        grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = GridRowSelected;

        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.MultiSelect = false;
        grid.ReadOnly = true;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    // =========================================================
    //  HELPER METHODS — BUTTONS
    // =========================================================

    /// Primary action button (blue) — View, Print, Submit
    public static void StylePrimaryButton(Button btn)
    {
        btn.BackColor = Primary;
        btn.ForeColor = TextOnDark;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = PrimaryHover;
        btn.FlatAppearance.MouseDownBackColor = PrimaryPressed;
        btn.Font = FontButton;
        btn.Cursor = Cursors.Hand;
        btn.Height = ButtonHeight;
    }

    /// Secondary button (outlined) — Refresh, Close
    public static void StyleSecondaryButton(Button btn)
    {
        btn.BackColor = Surface;
        btn.ForeColor = TextPrimary;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = InputBorder;
        btn.FlatAppearance.MouseOverBackColor = Primary90;
        btn.FlatAppearance.MouseDownBackColor = Primary80;
        btn.Font = FontButton;
        btn.Cursor = Cursors.Hand;
        btn.Height = ButtonHeight;
    }

    /// Success button (green) — Save, Add, Accept
    public static void StyleSuccessButton(Button btn)
    {
        btn.BackColor = SuccessGreen;
        btn.ForeColor = TextOnDark;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = SuccessGreenHover;
        btn.Font = FontButton;
        btn.Cursor = Cursors.Hand;
        btn.Height = ButtonHeight;
    }

    /// Warning button (amber) — Edit
    public static void StyleWarningButton(Button btn)
    {
        btn.BackColor = WarningAmber;
        btn.ForeColor = TextOnDark;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = WarningAmberHover;
        btn.Font = FontButton;
        btn.Cursor = Cursors.Hand;
        btn.Height = ButtonHeight;
    }

    /// Danger button (red) — Archive, Delete, Logout, Decline
    public static void StyleDangerButton(Button btn)
    {
        btn.BackColor = Danger;
        btn.ForeColor = TextOnDark;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = DangerHover;
        btn.FlatAppearance.MouseDownBackColor = DangerPressed;
        btn.Font = FontButton;
        btn.Cursor = Cursors.Hand;
        btn.Height = ButtonHeight;
    }

    /// Neutral button (gray) — Cancel
    public static void StyleNeutralButton(Button btn)
    {
        btn.BackColor = NeutralGray;
        btn.ForeColor = TextOnDark;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = NeutralGrayHover;
        btn.Font = FontButton;
        btn.Cursor = Cursors.Hand;
        btn.Height = ButtonHeight;
    }

    // =========================================================
    //  HELPER METHODS — SIDEBAR
    // =========================================================

    public static void StyleSidebarButton(Button btn, bool isLogout = false)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.Padding = new Padding(20, 0, 0, 0);
        btn.Height = 48;
        btn.Cursor = Cursors.Hand;
        btn.Font = FontBody;

        if (isLogout)
        {
            btn.BackColor = Danger;
            btn.ForeColor = TextOnDark;
            btn.FlatAppearance.MouseOverBackColor = DangerHover;
            btn.FlatAppearance.MouseDownBackColor = DangerPressed;
        }
        else
        {
            btn.BackColor = SidebarItem;
            btn.ForeColor = SidebarText;
            btn.FlatAppearance.MouseOverBackColor = SidebarItemHover2;
            btn.FlatAppearance.MouseDownBackColor = SidebarItemActive;
        }
    }

    // =========================================================
    //  HELPER METHODS — GRID BUTTON COLUMNS
    // =========================================================

    /// <summary>
    /// Builds a DataGridViewCellStyle for a button column that matches
    /// the semantic color scheme (success, warning, danger, primary, neutral).
    ///
    /// Uses a smaller font (9pt bold) than regular buttons so captions
    /// fit inside narrow button columns. Also zeroes out the internal
    /// cell padding so text gets the full column width.
    /// </summary>
    public static DataGridViewCellStyle GridButtonStyle(string semantic)
    {
        Color back, fore, hover;

        switch (semantic.ToLowerInvariant())
        {
            case "success":
                back = SuccessGreen;
                fore = TextOnDark;
                hover = SuccessGreenHover;
                break;

            case "warning":
                back = WarningAmber;
                fore = TextOnDark;
                hover = WarningAmberHover;
                break;

            case "danger":
                back = Danger;
                fore = TextOnDark;
                hover = DangerHover;
                break;

            case "neutral":
                back = NeutralGray;
                fore = TextOnDark;
                hover = NeutralGrayHover;
                break;

            case "primary":
            default:
                back = Primary;
                fore = TextOnDark;
                hover = PrimaryHover;
                break;
        }

        return new DataGridViewCellStyle
        {
            BackColor = back,
            ForeColor = fore,
            SelectionBackColor = hover,
            SelectionForeColor = fore,
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            Font = FontGridButton,             // ← 9pt bold, not 10pt
            Padding = new Padding(0)           // ← removes ~8px internal padding
        };
    }

    /// <summary>
    /// Creates a themed button column for a DataGridView.
    /// </summary>
    public static DataGridViewButtonColumn CreateGridButtonColumn(
        string name,
        string headerText,
        string buttonText,
        string semantic,
        int width = 100)
    {
        return new DataGridViewButtonColumn
        {
            Name = name,
            HeaderText = headerText,
            Text = buttonText,
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat,
            Width = width,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            DefaultCellStyle = GridButtonStyle(semantic)
        };
    }

    /// <summary>
    /// Applies the semantic button style to every cell in the named button column,
    /// overriding the alternating-row style that would otherwise wash out the color.
    /// Safe to call multiple times.
    /// </summary>
    public static void ApplyStyleToButtonColumn(DataGridView grid, string columnName, string semantic)
    {
        if (!grid.Columns.Contains(columnName)) return;

        var baseStyle = GridButtonStyle(semantic);

        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.IsNewRow) continue;

            var cell = row.Cells[columnName];

            cell.Style.BackColor = baseStyle.BackColor;
            cell.Style.ForeColor = baseStyle.ForeColor;
            cell.Style.SelectionBackColor = baseStyle.SelectionBackColor;
            cell.Style.SelectionForeColor = baseStyle.SelectionForeColor;
            cell.Style.Alignment = baseStyle.Alignment;
            cell.Style.Font = baseStyle.Font;
            cell.Style.Padding = baseStyle.Padding;
        }
    }

    /// <summary>
    /// Registers a DataBindingComplete handler on the grid that re-applies
    /// button column styles every time the grid finishes binding.
    /// This guarantees the styles are applied AFTER all rows exist, fixing
    /// the "gray on first load" issue.
    ///
    /// Call this ONCE per grid, typically in the form's constructor or
    /// ApplyTheme() method.
    /// </summary>
    public static void RegisterButtonColumnStyles(
        DataGridView grid,
        params (string ColumnName, string Semantic)[] columns)
    {
        // Store the column list in the grid's Tag so the handler knows what to style
        grid.Tag = columns;

        // Remove previous handler to avoid duplicate registrations
        grid.DataBindingComplete -= Grid_DataBindingComplete;
        grid.DataBindingComplete += Grid_DataBindingComplete;
    }

    private static void Grid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        if (sender is not DataGridView grid) return;
        if (grid.Tag is not (string ColumnName, string Semantic)[] columns) return;

        foreach (var (columnName, semantic) in columns)
        {
            ApplyStyleToButtonColumn(grid, columnName, semantic);
        }
    }
}