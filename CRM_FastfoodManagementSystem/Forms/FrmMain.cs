using System.Linq;

namespace CRM.winForms.Forms;

public partial class FrmMain : Form
{
    private Button? _activeButton;

    public FrmMain()
    {
        InitializeComponent();
        ApplyTheme();
        Load += FrmMain_Load;
        btnLogout.Click += BtnLogout_Click;
    }

    private void ApplyTheme()
    {
        AppTheme.ApplyForm(this);
        BackColor = AppTheme.Background;

        pnlSidebar.BackColor = AppTheme.Primary20;
        pnlMenu.BackColor = AppTheme.Primary20;
        pnlTop.BackColor = AppTheme.Surface;
        pnlContent.BackColor = AppTheme.ContentSurface;

        lblBrand.ForeColor = AppTheme.TextOnDark;
        lblBrand.Text = "Fastfood MS";

        lblWelcome.ForeColor = AppTheme.Primary80;

        lblPageTitle.ForeColor = AppTheme.TextPrimary;

        btnLogout.BackColor = AppTheme.Danger;
        btnLogout.ForeColor = AppTheme.TextOnDark;
        btnLogout.FlatAppearance.MouseOverBackColor = AppTheme.DangerHover;
        btnLogout.Text = "Logout";
    }

    private void FrmMain_Load(object? sender, EventArgs e)
    {
        lblWelcome.Text = $"{UserSession.FullName}\n{UserSession.RoleName}";
        BuildMenu();

        if (UserSession.IsAdmin)
        {
            ShowForm(new FrmBusinessIntelligence(), "Business Intelligence",
                form => HookNavigation((FrmBusinessIntelligence)form));
            HighlightMenuButton("Business Intelligence");
        }
        else if (UserSession.IsManager)
        {
            ShowForm(new FrmManagerDashboard(), "Dashboard");
            HighlightMenuButton("Dashboard");
        }
        else if (UserSession.IsStaff)
        {
            ShowForm(new FrmOrderManagement(), "Order Management");
            HighlightMenuButton("Order Management");
        }
        else
        {
            ShowDashboard();
        }
    }

    private void HighlightMenuButton(string text)
    {
        var btn = pnlMenu.Controls
            .OfType<Button>()
            .FirstOrDefault(b => b.Text == text);

        if (btn is not null)
            SetActiveButton(btn);
    }

    private void BuildMenu()
    {
        pnlMenu.Controls.Clear();
        _activeButton = null;

        // ============ ADMIN — Business Intelligence FIRST ============
        if (UserSession.IsAdmin)
        {
            AddMenuButton("Business Intelligence",
                () => ShowForm(new FrmBusinessIntelligence(), "Business Intelligence",
                    form => HookNavigation((FrmBusinessIntelligence)form)));
        }

        // ============ MANAGER — Dashboard FIRST ============
        if (UserSession.IsManager)
        {
            AddMenuButton("Dashboard",
                () => ShowForm(new FrmManagerDashboard(), "Dashboard"));
        }

        // ============ SUPERADMIN & ADMIN — Core admin modules ============
        if (UserSession.IsSuperAdmin || UserSession.IsAdmin)
        {
            AddMenuButton("Terms and Conditions",
                () => ShowForm(new FrmTermsEditor(), "Terms and Conditions"));

            AddMenuButton("User Management",
                () => ShowForm(new FrmUserManagement(), "User Management"));
        }

        // ============ ADMIN & MANAGER — Operations ============
        if (UserSession.IsAdmin || UserSession.IsManager)
        {
            AddMenuButton("Customer Feedback",
                () => ShowForm(new FrmFeedbackList(), "Customer Feedback"));

            AddMenuButton("Complaints",
                () => ShowForm(new FrmComplaints(), "Complaints"));

            AddMenuButton("Customer Management",
                () => ShowForm(new FrmCustomerList(canEdit: true, canRegister: true),
                    "Customer Management"));

            AddMenuButton("Promotions",
                () => ShowForm(new FrmPromotions(), "Promotions"));

            AddMenuButton("Customer Retention",
                () => ShowForm(new FrmCustomerRetention(), "Customer Retention"));

            AddMenuButton("Product Management",
                () => ShowForm(new FrmProductManagement(), "Product Management"));

            AddMenuButton("Inventory Management",
                () => ShowForm(new FrmInventoryManagement(), "Inventory Management"));

            AddMenuButton("View Transactions",
                () => ShowForm(new FrmViewTransactions(), "View Transactions"));

            // ============ Reports ============
            AddMenuButton("Reports",
                () => ShowForm(new FrmReports(), "Reports"));
        }

        // ============ STAFF — Order Management FIRST ============
        if (UserSession.IsStaff)
        {
            AddMenuButton("Order Management",
                () => ShowForm(new FrmOrderManagement(), "Order Management"));

            // Staff can register new customers but cannot edit/archive
            AddMenuButton("Register Customer",
                () => ShowForm(new FrmCustomerList(canEdit: false, canRegister: true),
                    "Customer List"));

            AddMenuButton("Customer Feedback",
                () => ShowForm(new FrmFeedbackList(), "Customer Feedback"));

            AddMenuButton("Complaints",
                () => ShowForm(new FrmComplaints(), "Complaints"));
        }

        // ============ SUPERADMIN & ADMIN — Activity Logs LAST ============
        if (UserSession.IsSuperAdmin || UserSession.IsAdmin)
        {
            AddMenuButton("Activity Logs",
                () => ShowForm(new FrmActivityLogs(), "Activity Logs"));
        }
    }

    private void AddMenuButton(string text, Action onClick)
    {
        var btn = new Button
        {
            Text = text,
            Width = pnlMenu.ClientSize.Width - pnlMenu.Padding.Left - pnlMenu.Padding.Right - 4,
            Height = 46,
            Margin = new Padding(0, 0, 0, 4),
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = AppTheme.FontBody,
            BackColor = AppTheme.SidebarItem,
            ForeColor = AppTheme.SidebarText,
            Cursor = Cursors.Hand,
            Padding = new Padding(16, 0, 0, 0)
        };

        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = AppTheme.SidebarItemHover2;
        btn.FlatAppearance.MouseDownBackColor = AppTheme.SidebarItemActive;

        btn.Click += (_, __) =>
        {
            onClick();
            SetActiveButton(btn);
        };

        pnlMenu.Controls.Add(btn);
    }

    private void SetActiveButton(Button btn)
    {
        if (_activeButton is not null)
            _activeButton.BackColor = AppTheme.SidebarItem;

        btn.BackColor = AppTheme.SidebarItemActive;
        _activeButton = btn;
    }

    private void ShowForm(Form form, string pageTitle, Action<Form>? onBeforeShow = null)
    {
        lblPageTitle.Text = pageTitle;

        foreach (Control c in pnlContent.Controls)
            c.Dispose();
        pnlContent.Controls.Clear();

        onBeforeShow?.Invoke(form);

        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(0, 0);

        pnlContent.Controls.Add(form);
        form.Show();
    }

    private void HookNavigation(FrmBusinessIntelligence bi)
    {
        bi.NavigateRequested += pageKey => NavigateTo(pageKey);
    }

    private void NavigateTo(string pageKey)
    {
        switch (pageKey)
        {
            case "ViewTransactions":
                ShowForm(new FrmViewTransactions(), "View Transactions");
                HighlightMenuButton("View Transactions");
                break;

            case "CustomerManagement":
                ShowForm(new FrmCustomerList(canEdit: UserSession.IsAdmin || UserSession.IsManager),
                    "Customer Management");
                HighlightMenuButton("Customer Management");
                break;

            case "CustomerRetention":
                ShowForm(new FrmCustomerRetention(), "Customer Retention");
                HighlightMenuButton("Customer Retention");
                break;

            case "CustomerFeedback":
                ShowForm(new FrmFeedbackList(), "Customer Feedback");
                HighlightMenuButton("Customer Feedback");
                break;

            case "Complaints":
                ShowForm(new FrmComplaints(), "Complaints");
                HighlightMenuButton("Complaints");
                break;

            case "InventoryManagement":
                ShowForm(new FrmInventoryManagement(), "Inventory Management");
                HighlightMenuButton("Inventory Management");
                break;

            case "ProductManagement":
                ShowForm(new FrmProductManagement(), "Product Management");
                HighlightMenuButton("Product Management");
                break;

            case "Promotions":
                ShowForm(new FrmPromotions(), "Promotions");
                HighlightMenuButton("Promotions");
                break;

            case "Reports":
                ShowForm(new FrmReports(), "Reports");
                HighlightMenuButton("Reports");
                break;

            default:
                MessageBox.Show($"Unknown navigation target: {pageKey}",
                    "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                break;
        }
    }

    private void ShowDashboard()
    {
        lblPageTitle.Text = "Dashboard";

        var lbl = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14F),
            ForeColor = AppTheme.TextSecondary,
            Text = $"Welcome to the Fastfood Management System.\n\n" +
                   $"Signed in as: {UserSession.FullName}\n" +
                   $"Role: {UserSession.RoleName}\n\n" +
                   $"Select a menu item on the left to begin."
        };

        pnlContent.Controls.Clear();
        pnlContent.Controls.Add(lbl);
    }

    private void BtnLogout_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to logout?",
            "Confirm Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            ActivityLogger.Log("Logout", "User", UserSession.UserId,
                $"{UserSession.Username} logged out");

            UserSession.Clear();
            Close();
        }
    }
}