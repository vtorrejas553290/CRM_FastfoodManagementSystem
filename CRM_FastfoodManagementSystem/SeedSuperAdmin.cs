using CRM.infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CRM.winForms;

public static class SeedSuperAdmin
{
    private const string Username = "superadmin";
    private const string Password = "SuperAdmin@123";

    public static void Run()
    {
        var report = new System.Text.StringBuilder();

        try
        {
            AppServices.Initialize();

            using var master = AppServices.CreateMasterContext();

            // Delete any existing superadmin row (clean slate)
            var existing = master.Users
                .Where(u => u.NormalizedUserName == "SUPERADMIN")
                .ToList();

            foreach (var u in existing)
                master.Users.Remove(u);

            if (existing.Count > 0)
                master.SaveChanges();

            report.AppendLine($"Deleted {existing.Count} existing superadmin row(s).");

            // Create the IdentityUser
            var identityUser = new IdentityUser
            {
                UserName = Username,
                NormalizedUserName = "SUPERADMIN",
                Email = "superadmin@local",
                NormalizedEmail = "SUPERADMIN@LOCAL",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = true
            };

            // Hash the password using the SAME hasher FrmLogin verifies with
            var hasher = new PasswordHasher<IdentityUser>();
            identityUser.PasswordHash = hasher.HashPassword(identityUser, Password);

            master.Users.Add(identityUser);
            master.SaveChanges();

            report.AppendLine($"SuperAdmin created successfully.");
            report.AppendLine($"Id:       {identityUser.Id}");
            report.AppendLine($"Username: {Username}");
            report.AppendLine($"Password: {Password}");
            report.AppendLine($"HashLen:  {identityUser.PasswordHash.Length}");
        }
        catch (Exception ex)
        {
            report.AppendLine("ERROR:");
            report.AppendLine(ex.GetType().Name);
            report.AppendLine(ex.Message);
            if (ex.InnerException != null)
            {
                report.AppendLine("--- Inner ---");
                report.AppendLine(ex.InnerException.Message);
            }
        }

        MessageBox.Show(report.ToString(),
            "SeedSuperAdmin Result",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}