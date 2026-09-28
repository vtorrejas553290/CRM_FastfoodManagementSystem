using CRM.domain.Entities;
using CRM.infrastructure;
using CRM.infrastructure.Data;
using CRM.infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms;

/// <summary>
/// One-time reset + reseed for Tenant C. Run via:
///   CRM.winforms.exe --reseed
/// Deletes all tenant data except admin_c, then inserts fresh demo data.
/// </summary>
public static class SeedTenantCReseed
{
    private const string TenantCServer = "(localdb)\\TenantC";
    private const string TenantCDatabase = "DB_TenantCRM";

    private static readonly string[] FilipinoFirstNames =
    {
        "Juan", "Maria", "Jose", "Ana", "Pedro", "Rosa", "Carlos", "Lucia",
        "Miguel", "Carmen", "Rafael", "Elena", "Antonio", "Sofia", "Diego",
        "Isabella", "Manuel", "Teresa", "Ramon", "Beatriz", "Felipe", "Gloria",
        "Alfredo", "Pilar", "Ricardo", "Conchita", "Ernesto", "Rosario",
        "Fernando", "Amelia", "Rodrigo", "Dolores", "Sergio", "Rosalinda",
        "Eduardo", "Marilou", "Rogelio", "Corazon", "Alfonso", "Imelda",
        "Benjamin", "Lourdes", "Cesar", "Josefina", "Hector", "Aurora",
        "Arturo", "Milagros", "Rolando", "Estrella"
    };

    private static readonly string[] FilipinoLastNames =
    {
        "Santos", "Reyes", "Cruz", "Bautista", "Ocampo", "Garcia", "Mendoza",
        "Torres", "Flores", "Ramos", "Gonzales", "Bautista", "Villanueva",
        "Aquino", "Castillo", "Domingo", "Rivera", "Aguilar", "Navarro",
        "Salazar", "Del Rosario", "Pascual", "Marquez", "Padilla", "Soriano",
        "Velasco", "Fajardo", "Espinosa", "Alcantara", "Bernardo"
    };

    private static readonly string[] Streets =
    {
        "Rizal Avenue", "Mabini Street", "Bonifacio Road", "Aguinaldo Highway",
        "Quezon Boulevard", "Luna Street", "Del Pilar Avenue", "Katipunan Road",
        "Ortigas Avenue", "Shaw Boulevard", "Ayala Avenue", "Makati Avenue"
    };

    private static readonly string[] Cities =
    {
        "Manila", "Quezon City", "Makati", "Cebu City", "Davao City",
        "Pasig", "Taguig", "Iloilo City", "Bacolod", "Baguio"
    };

    public static void Run()
    {
        var report = new System.Text.StringBuilder();

        try
        {
            AppServices.Initialize();

            var cs =
                $"Server={TenantCServer};Database={TenantCDatabase};" +
                "Trusted_Connection=True;TrustServerCertificate=True;" +
                "MultipleActiveResultSets=True;";

            var options = new DbContextOptionsBuilder<TenantCrmDbContext>()
                .UseSqlServer(cs)
                .Options;

            using var db = new TenantCrmDbContext(options);

            report.AppendLine("=== TenantC Reset + Reseed ===");
            report.AppendLine();

            // ============================================================
            // STEP 1 — DELETE all business data (FK-safe order)
            // ============================================================
            report.AppendLine("Step 1: Deleting existing data...");

            db.PromotionRedemptions.RemoveRange(db.PromotionRedemptions.ToList());
            db.CustomerPoints.RemoveRange(db.CustomerPoints.ToList());
            db.OrderItems.RemoveRange(db.OrderItems.ToList());
            db.Transactions.RemoveRange(db.Transactions.ToList());
            db.Complaints.RemoveRange(db.Complaints.ToList());
            db.CustomerOutreaches.RemoveRange(db.CustomerOutreaches.ToList());
            db.RetentionOffers.RemoveRange(db.RetentionOffers.ToList());
            db.CustomerFeedbacks.RemoveRange(db.CustomerFeedbacks.ToList());
            db.TermsAcceptances.RemoveRange(db.TermsAcceptances.ToList());
            db.UserTermsAcceptances.RemoveRange(db.UserTermsAcceptances.ToList());
            db.ActivityLogs.RemoveRange(db.ActivityLogs.ToList());
            db.Orders.RemoveRange(db.Orders.ToList());
            db.Customers.RemoveRange(db.Customers.ToList());
            db.Branches.RemoveRange(db.Branches.ToList());
            db.Promotions.RemoveRange(db.Promotions.ToList());
            db.PromotionCategories.RemoveRange(db.PromotionCategories.ToList());

            db.SaveChanges();

            // Delete users except admin_c (UserId 1002)
            var nonAdminUsers = db.Users.Where(u => u.Username != "admin_c").ToList();
            db.Users.RemoveRange(nonAdminUsers);
            db.SaveChanges();

            // Ensure admin_c has no branch
            var admin = db.Users.FirstOrDefault(u => u.Username == "admin_c");
            if (admin != null)
            {
                admin.BranchId = null;
                db.SaveChanges();
            }

            report.AppendLine("  ✓ All business data cleared");
            report.AppendLine($"  ✓ Deleted {nonAdminUsers.Count} non-admin users");
            report.AppendLine($"  ✓ Kept admin_c (UserId = {admin?.UserId})");
            report.AppendLine();

            // ============================================================
            // STEP 2 — Reset inventories to 100 units each
            // ============================================================
            report.AppendLine("Step 2: Resetting inventories...");

            foreach (var inv in db.Inventories.ToList())
            {
                inv.QuantityOnHand = 100;
                inv.LastUpdatedAt = DateTime.UtcNow;
            }
            db.SaveChanges();

            report.AppendLine($"  ✓ Reset {db.Inventories.Count()} inventories to 100 units");
            report.AppendLine();

            // ============================================================
            // STEP 3 — Insert 2 branches
            // ============================================================
            report.AppendLine("Step 3: Seeding branches...");

            var branch1 = new Branch
            {
                BranchCode = "BR-001",
                BranchName = "Main Branch",
                Address = "123 Rizal Avenue, Manila",
                ContactNumber = "555-0001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var branch2 = new Branch
            {
                BranchCode = "BR-002",
                BranchName = "Second Branch",
                Address = "456 Ayala Avenue, Makati",
                ContactNumber = "555-0002",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.Branches.Add(branch1);
            db.Branches.Add(branch2);
            db.SaveChanges();

            report.AppendLine($"  ✓ BR-001 (BranchId={branch1.BranchId})");
            report.AppendLine($"  ✓ BR-002 (BranchId={branch2.BranchId})");
            report.AppendLine();

            // ============================================================
            // STEP 4 — Insert 6 users (2 managers, 4 staff)
            // ============================================================
            report.AppendLine("Step 4: Seeding users...");

            var managerRole = db.Roles.First(r => r.RoleCode == "MANAGER");
            var staffRole = db.Roles.First(r => r.RoleCode == "STAFF");

            var password = "123456789";
            var passwordHash = PasswordHasher.Hash(password);

            var users = new[]
            {
                new { Username = "manager_c1", FullName = "Jose Rizal",       RoleId = managerRole.RoleId, BranchId = (int?)branch1.BranchId },
                new { Username = "staff_c1",   FullName = "Andres Bonifacio", RoleId = staffRole.RoleId,   BranchId = (int?)branch1.BranchId },
                new { Username = "staff_c2",   FullName = "Emilio Aguinaldo", RoleId = staffRole.RoleId,   BranchId = (int?)branch1.BranchId },
                new { Username = "manager_c2", FullName = "Gabriela Silang",  RoleId = managerRole.RoleId, BranchId = (int?)branch2.BranchId },
                new { Username = "staff_c3",   FullName = "Lapu-Lapu",        RoleId = staffRole.RoleId,   BranchId = (int?)branch2.BranchId },
                new { Username = "staff_c4",   FullName = "Melchora Aquino",  RoleId = staffRole.RoleId,   BranchId = (int?)branch2.BranchId },
            };

            foreach (var u in users)
            {
                db.Users.Add(new User
                {
                    Username = u.Username,
                    PasswordHash = passwordHash,
                    FullName = u.FullName,
                    RoleId = u.RoleId,
                    BranchId = u.BranchId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }

            db.SaveChanges();

            report.AppendLine($"  ✓ Inserted {users.Length} users (password = {password})");
            report.AppendLine();

            // ============================================================
            // STEP 5 — Insert 3 promotion categories + 3 promotions
            // ============================================================
            report.AppendLine("Step 5: Seeding promotions...");

            var catNew = new PromotionCategory
            {
                CategoryName = "New Customer",
                Description = "Promotions for first-time customers",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var catWin = new PromotionCategory
            {
                CategoryName = "Win-Back",
                Description = "Promotions for dormant customers",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var catRisk = new PromotionCategory
            {
                CategoryName = "At Risk",
                Description = "Promotions for at-risk customers",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.PromotionCategories.AddRange(catNew, catWin, catRisk);
            db.SaveChanges();

            var now = DateTime.UtcNow;

            db.Promotions.AddRange(
                new Promotion
                {
                    PromotionCode = "NEW-01",
                    PromotionName = "Welcome Promo",
                    Description = "10% off for new customers",
                    DiscountType = "Percent",
                    DiscountValue = 10,
                    MinimumPurchase = 200,
                    StartDate = now.AddDays(-30),
                    EndDate = now.AddDays(60),
                    IsActive = true,
                    PromotionCategoryId = catNew.PromotionCategoryId,
                    CreatedAt = now
                },
                new Promotion
                {
                    PromotionCode = "WIN-01",
                    PromotionName = "Win-Back Offer",
                    Description = "15% off to bring dormant customers back",
                    DiscountType = "Percent",
                    DiscountValue = 15,
                    MinimumPurchase = 300,
                    StartDate = now.AddDays(-30),
                    EndDate = now.AddDays(60),
                    IsActive = true,
                    PromotionCategoryId = catWin.PromotionCategoryId,
                    CreatedAt = now
                },
                new Promotion
                {
                    PromotionCode = "RISK-01",
                    PromotionName = "Loyalty Recovery",
                    Description = "20% off for at-risk customers",
                    DiscountType = "Percent",
                    DiscountValue = 20,
                    MinimumPurchase = 400,
                    StartDate = now.AddDays(-30),
                    EndDate = now.AddDays(60),
                    IsActive = true,
                    PromotionCategoryId = catRisk.PromotionCategoryId,
                    CreatedAt = now
                }
            );
            db.SaveChanges();

            report.AppendLine("  ✓ 3 promotion categories + 3 promotions inserted");
            report.AppendLine();

            // ============================================================
            // STEP 6 — Insert 40 customers (20 per branch)
            // ============================================================
            report.AppendLine("Step 6: Seeding customers...");

            var rng = new Random(42);   // fixed seed for reproducibility

            var customers = new List<Customer>();
            var usedCodes = new HashSet<string>();

            for (int i = 1; i <= 40; i++)
            {
                int branchId = i <= 20 ? branch1.BranchId : branch2.BranchId;

                string code;
                do
                {
                    code = $"CUST-{i:D4}";
                } while (usedCodes.Contains(code));
                usedCodes.Add(code);

                var firstName = FilipinoFirstNames[rng.Next(FilipinoFirstNames.Length)];
                var lastName = FilipinoLastNames[rng.Next(FilipinoLastNames.Length)];
                var middleName = rng.Next(2) == 0 ? FilipinoFirstNames[rng.Next(FilipinoFirstNames.Length)] : null;

                var fullName = string.IsNullOrEmpty(middleName)
                    ? $"{firstName} {lastName}"
                    : $"{firstName} {middleName} {lastName}";

                var street = Streets[rng.Next(Streets.Length)];
                var city = Cities[rng.Next(Cities.Length)];

                customers.Add(new Customer
                {
                    CustomerCode = code,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    CustomerName = fullName,
                    ContactNumber = $"09{rng.Next(100000000, 999999999)}",
                    EmailAddress = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com",
                    Address = $"{rng.Next(1, 999)} {street}, {city}",
                    Birthday = new DateTime(rng.Next(1960, 2005), rng.Next(1, 13), rng.Next(1, 28)),
                    IsActive = true,
                    CreatedAt = now.AddDays(-rng.Next(30, 365)),
                    CurrentPoints = 0,
                    BranchId = branchId
                });
            }

            db.Customers.AddRange(customers);
            db.SaveChanges();

            report.AppendLine($"  ✓ {customers.Count} customers inserted");
            report.AppendLine($"    - Branch 1: {customers.Count(c => c.BranchId == branch1.BranchId)}");
            report.AppendLine($"    - Branch 2: {customers.Count(c => c.BranchId == branch2.BranchId)}");
            report.AppendLine();

            // ============================================================
            // STEP 7 — Insert orders + order items + transactions
            //   - 6 customers with no orders (first 6 of Branch 1)
            //   - 8 customers with 1 order (next 8)
            //   - 26 customers with 5-15 orders each (the rest)
            //   Total target: ~400 orders (~200 per branch)
            // ============================================================
            report.AppendLine("Step 7: Seeding orders + transactions...");

            var products = db.Products.Include(p => p.Inventory).ToList();
            if (products.Count == 0)
            {
                report.AppendLine("  ✗ No products found — cannot generate orders.");
                ShowReport(report.ToString());
                return;
            }

            // Split customers into categories
            var branch1Customers = customers.Where(c => c.BranchId == branch1.BranchId).ToList();
            var branch2Customers = customers.Where(c => c.BranchId == branch2.BranchId).ToList();

            // Branch 1: 6 no-order, 8 one-order, 6 repeat
            var b1NoOrders = branch1Customers.Take(6).ToList();
            var b1OneOrder = branch1Customers.Skip(6).Take(8).ToList();
            var b1Repeat = branch1Customers.Skip(14).ToList();

            // Branch 2: mirror for symmetry (but 200 orders / 20 customers = 10 avg)
            var b2NoOrders = branch2Customers.Take(6).ToList();
            var b2OneOrder = branch2Customers.Skip(6).Take(8).ToList();
            var b2Repeat = branch2Customers.Skip(14).ToList();

            int orderCounter = 1;
            int totalOrders = 0;
            int totalItems = 0;
            int totalTransactions = 0;

            var staffByBranch = new Dictionary<int, int>
            {
                { branch1.BranchId, db.Users.First(u => u.Username == "staff_c1").UserId },
                { branch2.BranchId, db.Users.First(u => u.Username == "staff_c3").UserId },
            };

            // Helper to create one order
            void CreateOrder(Customer customer, int branchId, DateTime when)
            {
                // 1-3 products
                int itemCount = rng.Next(1, 4);
                var pickedProducts = products.OrderBy(x => rng.Next()).Take(itemCount).ToList();

                decimal subTotal = 0;
                var items = new List<OrderItem>();

                foreach (var p in pickedProducts)
                {
                    int qty = rng.Next(1, 4);
                    decimal lineTotal = p.UnitPrice * qty;
                    subTotal += lineTotal;

                    items.Add(new OrderItem
                    {
                        ProductId = p.ProductId,
                        Quantity = qty,
                        UnitPrice = p.UnitPrice,
                        LineTotal = lineTotal
                    });
                }

                decimal discount = 0;
                decimal total = subTotal - discount;

                var orderCode = $"ORD-{when:yyyyMMddHHmmss}-{orderCounter++:D4}";

                var order = new Order
                {
                    OrderCode = orderCode,
                    CustomerId = customer.CustomerId,
                    StaffUserId = staffByBranch[branchId],
                    OrderType = rng.Next(2) == 0 ? "DineIn" : "TakeOut",
                    SubTotal = subTotal,
                    DiscountAmount = discount,
                    PointsRedeemed = 0,
                    PointsEarned = (int)(total / 100m),
                    TotalAmount = total,
                    Status = "Paid",
                    OrderDate = when,
                    BranchId = branchId
                };

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in items)
                {
                    item.OrderId = order.OrderId;
                    db.OrderItems.Add(item);
                }

                string method = rng.Next(10) < 7 ? "Cash" : "GCash";
                decimal paid = method == "Cash"
                    ? Math.Ceiling(total / 50) * 50
                    : total;

                db.Transactions.Add(new Transaction
                {
                    OrderId = order.OrderId,
                    PaymentMethod = method,
                    AmountPaid = paid,
                    ChangeDue = paid - total,
                    ReferenceNumber = method == "GCash" ? $"GC{rng.Next(1000000, 9999999)}" : null,
                    PaidAt = when.AddMinutes(rng.Next(5, 45)),
                    BranchId = branchId
                });

                customer.CurrentPoints += order.PointsEarned;

                db.CustomerPoints.Add(new CustomerPoint
                {
                    CustomerId = customer.CustomerId,
                    TransactionType = "Earned",
                    Points = order.PointsEarned,
                    OrderId = order.OrderId,
                    Notes = $"Earned from {orderCode}",
                    PerformedByUserId = staffByBranch[branchId],
                    PerformedAt = when
                });

                totalOrders++;
                totalItems += items.Count;
                totalTransactions++;
            }

            // Branch 1 — 1-order customers
            foreach (var c in b1OneOrder)
                CreateOrder(c, branch1.BranchId, now.AddDays(-rng.Next(1, 90)));

            // Branch 1 — repeat customers: 5-15 orders each, total ~180
            foreach (var c in b1Repeat)
            {
                int orderCount = rng.Next(5, 16);
                for (int i = 0; i < orderCount; i++)
                    CreateOrder(c, branch1.BranchId, now.AddDays(-rng.Next(1, 90)));
            }

            // Branch 2 — 1-order customers
            foreach (var c in b2OneOrder)
                CreateOrder(c, branch2.BranchId, now.AddDays(-rng.Next(1, 90)));

            // Branch 2 — repeat customers: 5-15 orders each
            foreach (var c in b2Repeat)
            {
                int orderCount = rng.Next(5, 16);
                for (int i = 0; i < orderCount; i++)
                    CreateOrder(c, branch2.BranchId, now.AddDays(-rng.Next(1, 90)));
            }

            db.SaveChanges();

            report.AppendLine($"  ✓ {totalOrders} orders");
            report.AppendLine($"  ✓ {totalItems} order items");
            report.AppendLine($"  ✓ {totalTransactions} transactions");
            report.AppendLine($"  ✓ CustomerPoints entries updated");
            report.AppendLine();

            // ============================================================
            // STEP 8 — Summary
            // ============================================================
            report.AppendLine("=== FINAL STATE ===");
            report.AppendLine($"  Branches:        {db.Branches.Count()}");
            report.AppendLine($"  Users:           {db.Users.Count()}");
            report.AppendLine($"  Customers:       {db.Customers.Count()}");
            report.AppendLine($"  Promotions:      {db.Promotions.Count()}");
            report.AppendLine($"  Orders:          {db.Orders.Count()}");
            report.AppendLine($"  Transactions:    {db.Transactions.Count()}");
            report.AppendLine($"  OrderItems:      {db.OrderItems.Count()}");
            report.AppendLine($"  CustomerPoints:  {db.CustomerPoints.Count()}");
            report.AppendLine();
            report.AppendLine("Login credentials:");
            report.AppendLine($"  manager_c1 / {password}  (Branch 1 — MANAGER)");
            report.AppendLine($"  staff_c1   / {password}  (Branch 1 — STAFF)");
            report.AppendLine($"  staff_c2   / {password}  (Branch 1 — STAFF)");
            report.AppendLine($"  manager_c2 / {password}  (Branch 2 — MANAGER)");
            report.AppendLine($"  staff_c3   / {password}  (Branch 2 — STAFF)");
            report.AppendLine($"  staff_c4   / {password}  (Branch 2 — STAFF)");
            report.AppendLine($"  admin_c    / (unchanged)");
        }
        catch (Exception ex)
        {
            report.AppendLine();
            report.AppendLine("=== ERROR ===");
            report.AppendLine(ex.GetType().Name);
            report.AppendLine(ex.Message);
            if (ex.InnerException != null)
            {
                report.AppendLine("--- Inner ---");
                report.AppendLine(ex.InnerException.Message);
            }
        }

        ShowReport(report.ToString());
    }

    private static void ShowReport(string text)
    {
        MessageBox.Show(text, "Seed TenantC Reseed Result",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}