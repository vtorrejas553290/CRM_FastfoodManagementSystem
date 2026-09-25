using Microsoft.EntityFrameworkCore;
using CRM.domain.Entities;
using CRM.infrastructure.Data;
using CRM.infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MasterCrmDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MasterCrm")));

builder.Services.AddDbContext<TenantCrmDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TenantCrm")));

// FIX #3 — Prevents JSON cycle errors from any endpoint
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.MapGet("/", () => "CRM FastFood Management API is running.");

// ==================== MASTER: COMPANIES ====================

app.MapPost("/companies", async (
    Company company,
    MasterCrmDbContext db) =>
{
    db.Companies.Add(company);
    await db.SaveChangesAsync();

    return Results.Created($"/companies/{company.CompanyId}", company);
});

app.MapGet("/companies", async (MasterCrmDbContext db) =>
{
    var companies = await db.Companies
        .AsNoTracking()
        .OrderBy(x => x.CompanyId)
        .ToListAsync();

    return Results.Ok(companies);
});

app.MapPost("/company-databases", async (
    CompanyDatabase companyDatabase,
    MasterCrmDbContext db) =>
{
    db.CompanyDatabases.Add(companyDatabase);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/company-databases/{companyDatabase.CompanyDatabaseId}",
        companyDatabase);
});

app.MapGet("/company-databases", async (MasterCrmDbContext db) =>
{
    var rows = await db.CompanyDatabases
        .AsNoTracking()
        .OrderBy(x => x.CompanyDatabaseId)
        .ToListAsync();

    return Results.Ok(rows);
});

// ==================== ROLES ====================

app.MapPost("/roles", async (
    Role role,
    TenantCrmDbContext db) =>
{
    db.Roles.Add(role);
    await db.SaveChangesAsync();

    return Results.Created($"/roles/{role.RoleId}", role);
});

app.MapGet("/roles", async (TenantCrmDbContext db) =>
{
    var roles = await db.Roles
        .AsNoTracking()
        .OrderBy(x => x.RoleId)
        .ToListAsync();

    return Results.Ok(roles);
});

// ==================== USERS ====================

app.MapPost("/users", async (
    User user,
    TenantCrmDbContext db) =>
{
    user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
    user.CreatedAt = DateTime.UtcNow;

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.UserId}", user);
});

app.MapGet("/users", async (TenantCrmDbContext db) =>
{
    var users = await db.Users
        .AsNoTracking()
        .OrderBy(x => x.UserId)
        .Select(x => new
        {
            x.UserId,
            x.Username,
            x.FullName,
            x.Email,
            x.ContactNumber,
            x.RoleId,
            RoleCode = x.Role != null ? x.Role.RoleCode : null,
            RoleName = x.Role != null ? x.Role.RoleName : null,
            x.IsActive,
            x.CreatedAt
        })
        .ToListAsync();

    return Results.Ok(users);
});

// ==================== AUTH ====================

app.MapPost("/auth/register", async (
    RegisterRequest request,
    TenantCrmDbContext db) =>
{
    var exists = await db.Users
        .AnyAsync(x => x.Username == request.Username);

    if (exists)
        return Results.Conflict($"Username '{request.Username}' already exists.");

    var user = new User
    {
        Username = request.Username,
        PasswordHash = PasswordHasher.Hash(request.Password),
        FullName = request.FullName,
        Email = request.Email,
        ContactNumber = request.ContactNumber,
        RoleId = request.RoleId,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();   // FIX #1 — was commented out

    return Results.Created($"/users/{user.UserId}", new
    {
        user.UserId,
        user.Username,
        user.FullName,
        user.RoleId
    });
});

app.MapPost("/auth/login", async (
    LoginRequest request,
    TenantCrmDbContext db) =>
{
    var user = await db.Users
        .Include(x => x.Role)
        .AsNoTracking()            // FIX #2 — breaks the cycle
        .FirstOrDefaultAsync(x => x.Username == request.Username);

    if (user is null || !user.IsActive)
        return Results.Unauthorized();

    bool valid = PasswordHasher.Verify(request.Password, user.PasswordHash);

    if (!valid)
        return Results.Unauthorized();

    return Results.Ok(new
    {
        user.UserId,
        user.Username,
        user.FullName,
        RoleCode = user.Role?.RoleCode,
        RoleName = user.Role?.RoleName
    });
});

// ==================== TERMS AND CONDITIONS ====================

app.MapPost("/terms", async (
    TermsAndCondition terms,
    TenantCrmDbContext db) =>
{
    db.TermsAndConditions.Add(terms);
    await db.SaveChangesAsync();

    return Results.Created($"/terms/{terms.TermsAndConditionId}", terms);
});

app.MapGet("/terms/active", async (TenantCrmDbContext db) =>
{
    var terms = await db.TermsAndConditions
        .AsNoTracking()
        .Where(x => x.IsActive)
        .OrderByDescending(x => x.EffectiveFrom)
        .FirstOrDefaultAsync();

    return terms is null ? Results.NotFound() : Results.Ok(terms);
});

app.MapGet("/terms", async (TenantCrmDbContext db) =>
{
    var terms = await db.TermsAndConditions
        .AsNoTracking()
        .OrderByDescending(x => x.EffectiveFrom)
        .ToListAsync();

    return Results.Ok(terms);
});

app.MapPost("/terms/{termsId:int}/accept/{customerId:int}", async (
    int termsId,
    int customerId,
    TermsAcceptance acceptance,
    TenantCrmDbContext db) =>
{
    acceptance.TermsAndConditionId = termsId;
    acceptance.CustomerId = customerId;
    acceptance.AcceptedAt = DateTime.UtcNow;

    db.TermsAcceptances.Add(acceptance);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/terms/{termsId}/accept/{customerId}",
        acceptance);
});

// ==================== CUSTOMERS ====================

app.MapPost("/customers", async (
    Customer customer,
    TenantCrmDbContext db) =>
{
    db.Customers.Add(customer);
    await db.SaveChangesAsync();

    return Results.Created($"/customers/{customer.CustomerId}", customer);
});

app.MapGet("/customers", async (TenantCrmDbContext db) =>
{
    var customers = await db.Customers
        .AsNoTracking()
        .OrderBy(x => x.CustomerId)
        .ToListAsync();

    return Results.Ok(customers);
});

app.MapGet("/customers/{customerId:int}", async (
    int customerId,
    TenantCrmDbContext db) =>
{
    var customer = await db.Customers
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.CustomerId == customerId);

    return customer is null ? Results.NotFound() : Results.Ok(customer);
});

// ==================== CUSTOMER FEEDBACK ====================

app.MapPost("/customers/{customerId:int}/feedback", async (
    int customerId,
    CustomerFeedback feedback,
    TenantCrmDbContext db) =>
{
    feedback.CustomerId = customerId;
    feedback.SubmittedAt = DateTime.UtcNow;

    db.CustomerFeedbacks.Add(feedback);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/customers/{customerId}/feedback/{feedback.CustomerFeedbackId}",
        feedback);
});

app.MapGet("/feedback", async (TenantCrmDbContext db) =>
{
    var feedbacks = await db.CustomerFeedbacks
        .AsNoTracking()
        .OrderByDescending(x => x.SubmittedAt)
        .Select(x => new
        {
            x.CustomerFeedbackId,
            x.CustomerId,
            CustomerName = x.Customer != null ? x.Customer.CustomerName : null,
            CustomerCode = x.Customer != null ? x.Customer.CustomerCode : null,
            x.Rating,
            x.Comments,
            x.Category,
            x.Status,
            x.SubmittedAt
        })
        .ToListAsync();

    return Results.Ok(feedbacks);
});

app.MapGet("/customers/{customerId:int}/feedback", async (
    int customerId,
    TenantCrmDbContext db) =>
{
    var feedbacks = await db.CustomerFeedbacks
        .AsNoTracking()
        .Where(x => x.CustomerId == customerId)
        .OrderByDescending(x => x.SubmittedAt)
        .Select(x => new
        {
            x.CustomerFeedbackId,
            x.CustomerId,
            x.Rating,
            x.Comments,
            x.Category,
            x.Status,
            x.SubmittedAt
        })
        .ToListAsync();

    return Results.Ok(feedbacks);
});
// Auto-launch the WinForms desktop app when the API starts
try
{
    // Adjust net9.0 to net8.0 / net6.0 based on your target framework
    var winFormsPath = Path.GetFullPath(Path.Combine(
        AppContext.BaseDirectory,
        "..", "..", "..", "..",
        "CRM.winForms",
        "bin", "Debug", "net9.0",
        "CRM.winForms.exe"));

    if (File.Exists(winFormsPath))
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = winFormsPath,
            UseShellExecute = true
        });

        Console.WriteLine($"[Startup] Launched WinForms from: {winFormsPath}");
    }
    else
    {
        Console.WriteLine($"[Startup] WinForms not found at: {winFormsPath}");
        Console.WriteLine($"[Startup] Build CRM.winForms first, then run again.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[Startup] Failed to launch WinForms: {ex.Message}");
}

// ============================================================
//  UPDATE (Edit) ENDPOINTS
// ============================================================

// Customers — update personal info
app.MapPut("/customers/{id:int}", async (
    int id,
    Customer updated,
    TenantCrmDbContext db) =>
{
    var customer = await db.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
    if (customer is null) return Results.NotFound();

    customer.CustomerName = updated.CustomerName;
    customer.ContactNumber = updated.ContactNumber;
    customer.EmailAddress = updated.EmailAddress;
    customer.Address = updated.Address;
    customer.Birthday = updated.Birthday;

    await db.SaveChangesAsync();
    return Results.Ok(customer);
});

// Users — update profile info (not password)
app.MapPut("/users/{id:int}", async (
    int id,
    User updated,
    TenantCrmDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(x => x.UserId == id);
    if (user is null) return Results.NotFound();

    user.FullName = updated.FullName;
    user.Email = updated.Email;
    user.ContactNumber = updated.ContactNumber;
    user.RoleId = updated.RoleId;

    await db.SaveChangesAsync();
    return Results.Ok(user);
});

// Feedback — update status (New → Reviewed → Resolved)
app.MapPut("/feedback/{id:int}/status", async (
    int id,
    string status,
    TenantCrmDbContext db) =>
{
    var feedback = await db.CustomerFeedbacks.FirstOrDefaultAsync(x => x.CustomerFeedbackId == id);
    if (feedback is null) return Results.NotFound();

    feedback.Status = status;
    await db.SaveChangesAsync();
    return Results.Ok(feedback);
});

// ============================================================
//  ARCHIVE (Soft Delete) ENDPOINTS
// ============================================================

app.MapPost("/companies/{id:int}/archive", async (int id, MasterCrmDbContext db) =>
{
    var e = await db.Companies.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = false;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, archived = true });
});

app.MapPost("/roles/{id:int}/archive", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.Roles.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = false;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, archived = true });
});

app.MapPost("/users/{id:int}/archive", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.Users.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = false;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, archived = true });
});

app.MapPost("/customers/{id:int}/archive", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.Customers.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = false;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, archived = true });
});

app.MapPost("/terms/{id:int}/archive", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.TermsAndConditions.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = false;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, archived = true });
});

app.MapPost("/feedback/{id:int}/archive", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.CustomerFeedbacks.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.Status = "Archived";
    await db.SaveChangesAsync();
    return Results.Ok(new { id, archived = true });
});

// ============================================================
//  RESTORE (Un-archive) ENDPOINTS
// ============================================================

app.MapPost("/customers/{id:int}/restore", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.Customers.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = true;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, restored = true });
});

app.MapPost("/users/{id:int}/restore", async (int id, TenantCrmDbContext db) =>
{
    var e = await db.Users.FindAsync(id);
    if (e is null) return Results.NotFound();
    e.IsActive = true;
    await db.SaveChangesAsync();
    return Results.Ok(new { id, restored = true });
});
app.Run();