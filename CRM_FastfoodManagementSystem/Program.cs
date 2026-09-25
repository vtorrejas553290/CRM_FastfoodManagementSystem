using CRM.winForms.Forms;

namespace CRM.winForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        AppServices.Initialize();
        // Set the global default font
        Application.SetDefaultFont(AppTheme.FontBody);

        // Loop: login → main menu → back to login on logout
        while (true)
        {
            var login = new FrmLogin();

            if (login.ShowDialog() != DialogResult.OK)
            {
                // User clicked Exit or closed the login form
                break;
            }

            var main = new FrmMain();
            main.ShowDialog();

            // If we reach here, FrmMain was closed (either logout or X button).
            // Loop back and show login again.
        }
    }
}