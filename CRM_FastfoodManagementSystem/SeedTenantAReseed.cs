using CRM.domain.Entities;
using CRM.infrastructure;
using CRM.infrastructure.Data;
using CRM.infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CRM.winForms;

/// <summary>
/// One-time reset + reseed for Tenant A (BASIC plan — no branching).
/// Run via: CRM.winforms.exe --reseedA
/// Deletes all tenant data except admin_a, then inserts BASIC-appropriate demo data.
/// </summary>
public static class SeedTenantAReseed
{
    private const string TenantAServer = "(localdb)\\TenantA";
    private const string TenantADatabase = "DB_TenantCRM";

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
        "Torres", "Flores", "Ramos", "Gonzales", "Villanueva",
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
                $"Server={TenantAServer};Database={TenantADatabase};" +
                "Trusted_Connection=True;TrustServerCertificate=True;" +
                "MultipleActiveResultSets=True;";

            var options = new DbContextOptionsBuilder<TenantCrmDbContext>()
                .UseSqlServer(cs)
                .Options;

            using var db = new TenantCrmDbContext(options);

            report.AppendLine("=== TenantA Reset + Reseed (BASIC plan) ===");
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
            db.Branches.RemoveRange(db.Branches.ToList());       // will remain empty
            db.Promotions.RemoveRange(db.Promotions.ToList());
            db.PromotionCategories.RemoveRange(db.PromotionCategories.ToList());

            db.SaveChanges();

            // Delete users except admin_a
            var nonAdminUsers = db.Users.Where(u => u.Username != "admin_a").ToList();
            db.Users.RemoveRange(nonAdminUsers);
            db.SaveChanges();

            var admin = db.Users.FirstOrDefault(u => u.Username == "admin_a");
            if (admin != null)
            {
                admin.BranchId = null;
                db.SaveChanges();
            }

            report.AppendLine("  ✓ All business data cleared");
            report.AppendLine($"  ✓ Deleted {nonAdminUsers.Count} non-admin users");
            report.AppendLine($"  ✓ Kept admin_a (UserId = {admin?.UserId})");
            report.AppendLine("  ✓ No branches created (BASIC plan)");
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
            // STEP 3 — Insert 6 users (2 managers, 4 staff, all BranchId = NULL)
            // ============================================================
            report.AppendLine("Step 3: Seeding users...");

            var managerRole = db.Roles.First(r => r.RoleCode == "MANAGER");
            var staffRole = db.Roles.First(r => r.RoleCode == "STAFF");

            var password = "123456789";
            var passwordHash = PasswordHasher.Hash(password);

            var users = new[]
            {
                new { Username = "manager_a1", FullName = "Manuel Quezon",    RoleId = managerRole.RoleId },
                new { Username = "staff_a1",   FullName = "Sergio Osmeña",    RoleId = staffRole.RoleId },
                new { Username = "staff_a2",   FullName = "Elpidio Quirino",  RoleId = staffRole.RoleId },
                new { Username = "manager_a2", FullName = "Ramon Magsaysay",  RoleId = managerRole.RoleId },
                new { Username = "staff_a3",   FullName = "Carlos Garcia",    RoleId = staffRole.RoleId },
                new { Username = "staff_a4",   FullName = "Diosdado Macapagal", RoleId = staffRole.RoleId },
            };

            foreach (var u in users)
            {
                db.Users.Add(new User
                {
                    Username = u.Username,
                    PasswordHash = passwordHash,
                    FullName = u.FullName,
                    RoleId = u.RoleId,
                    BranchId = null,      // BASIC plan has no branching
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }

            db.SaveChanges();

            report.AppendLine($"  ✓ Inserted {users.Length} users (password = {password})");
            report.AppendLine();

            // ============================================================
            // STEP 4 — Insert 1 promotion category + 1 promotion
            // ============================================================
            report.AppendLine("Step 4: Seeding promotion...");

            var catNew = new PromotionCategory
            {
                CategoryName = "New Customer",
                Description = "Promotions for first-time customers",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.PromotionCategories.Add(catNew);
            db.SaveChanges();

            var now = DateTime.UtcNow;

            db.Promotions.Add(new Promotion
            {
                PromotionCode = "WELCOME-01",
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
            });
            db.SaveChanges();

            report.AppendLine("  ✓ 1 promotion category + 1 promotion inserted");
            report.AppendLine();

            // ============================================================
            // STEP 5 — Insert 30 customers (no branch — BranchId = NULL)
            // ============================================================
            report.AppendLine("Step 5: Seeding customers...");

            var rng = new Random(7);   // different seed from Tenant C

            var customers = new List<Customer>();

            for (int i = 1; i <= 30; i++)
            {
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
                    CustomerCode = $"CUST-A{i:D4}",
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
                    BranchId = null     // BASIC plan has no branching
                });
            }

            db.Customers.AddRange(customers);
            db.SaveChanges();

            report.AppendLine($"  ✓ {customers.Count} customers inserted (no branch assignment)");
            report.AppendLine();

            // ============================================================
            // STEP 6 — Insert 250 orders + transactions
            //   - 6 no-order customers (recent sign-ups)
            //   - 6 one-order customers
            //   - remaining 18 customers: 5-15 orders each
            // ============================================================
            report.AppendLine("Step 6: Seeding orders + transactions...");

            var products = db.Products.Include(p => p.Inventory).ToList();
            if (products.Count == 0)
            {
                report.AppendLine("  ✗ No products found — cannot generate orders.");
                ShowReport(report.ToString());
                return;
            }

            var noOrderCustomers = customers.Take(6).ToList();
            var oneOrderCustomers = customers.Skip(6).Take(6).ToList();
            var repeatCustomers = customers.Skip(12).ToList();

            // All staff can process orders in a BASIC tenant (no branch restriction)
            var staffUserIds = db.Users
                .Where(u => u.Username.StartsWith("staff_a"))
                .Select(u => u.UserId)
                .ToList();

            int orderCounter = 1;
            int totalOrders = 0;
            int totalItems = 0;
            int totalTransactions = 0;

            void CreateOrder(Customer customer, DateTime when)
            {
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

                var orderCode = $"ORD-A{when:yyyyMMddHHmmss}-{orderCounter++:D4}";

                var order = new Order
                {
                    OrderCode = orderCode,
                    CustomerId = customer.CustomerId,
                    StaffUserId = staffUserIds[rng.Next(staffUserIds.Count)],
                    OrderType = rng.Next(2) == 0 ? "DineIn" : "TakeOut",
                    SubTotal = subTotal,
                    DiscountAmount = discount,
                    PointsRedeemed = 0,
                    PointsEarned = (int)(total / 100m),
                    TotalAmount = total,
                    Status = "Paid",
                    OrderDate = when,
                    BranchId = null     // BASIC plan has no branching
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
                    BranchId = null
                });

                customer.CurrentPoints += order.PointsEarned;

                db.CustomerPoints.Add(new CustomerPoint
                {
                    CustomerId = customer.CustomerId,
                    TransactionType = "Earned",
                    Points = order.PointsEarned,
                    OrderId = order.OrderId,
                    Notes = $"Earned from {orderCode}",
                    PerformedByUserId = order.StaffUserId,
                    PerformedAt = when
                });

                totalOrders++;
                totalItems += items.Count;
                totalTransactions++;
            }

            // One-order customers
            foreach (var c in oneOrderCustomers)
                CreateOrder(c, now.AddDays(-rng.Next(1, 90)));

            // Repeat customers — target ~250 total
            int targetTotal = 250;
            int remaining = targetTotal - totalOrders;
            int perCustomer = Math.Max(1, remaining / repeatCustomers.Count);

            foreach (var c in repeatCustomers)
            {
                for (int i = 0; i < perCustomer; i++)
                    CreateOrder(c, now.AddDays(-rng.Next(1, 90)));
            }

            db.SaveChanges();

            report.AppendLine($"  ✓ {totalOrders} orders");
            report.AppendLine($"  ✓ {totalItems} order items");
            report.AppendLine($"  ✓ {totalTransactions} transactions");
            report.AppendLine($"  ✓ {noOrderCustomers.Count} customers have no orders (for 'Never Ordered' demo)");
            report.AppendLine();

            // ============================================================
            // STEP 7 — Summary
            // ============================================================
            report.AppendLine("=== FINAL STATE ===");
            report.AppendLine($"  Branches:        {db.Branches.Count()}  (BASIC — no branching)");
            report.AppendLine($"  Users:           {db.Users.Count()}");
            report.AppendLine($"  Customers:       {db.Customers.Count()}");
            report.AppendLine($"  PromotionCategories: {db.PromotionCategories.Count()}");
            report.AppendLine($"  Promotions:      {db.Promotions.Count()}");
            report.AppendLine($"  Orders:          {db.Orders.Count()}");
            report.AppendLine($"  Transactions:    {db.Transactions.Count()}");
            report.AppendLine($"  OrderItems:      {db.OrderItems.Count()}");
            report.AppendLine($"  CustomerPoints:  {db.CustomerPoints.Count()}");
            report.AppendLine();
            report.AppendLine("Login credentials:");
            report.AppendLine($"  manager_a1 / {password}  (MANAGER)");
            report.AppendLine($"  staff_a1   / {password}  (STAFF)");
            report.AppendLine($"  staff_a2   / {password}  (STAFF)");
            report.AppendLine($"  manager_a2 / {password}  (MANAGER)");
            report.AppendLine($"  staff_a3   / {password}  (STAFF)");
            report.AppendLine($"  staff_a4   / {password}  (STAFF)");
            report.AppendLine($"  admin_a    / (unchanged)");
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
        MessageBox.Show(text, "Seed TenantA Reseed Result",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}