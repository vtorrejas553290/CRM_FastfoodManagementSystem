using CRM.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.infrastructure.Data;

public class TenantCrmDbContext : DbContext
{
    public TenantCrmDbContext(DbContextOptions<TenantCrmDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<TermsAndCondition> TermsAndConditions => Set<TermsAndCondition>();
    public DbSet<TermsAcceptance> TermsAcceptances => Set<TermsAcceptance>();
    public DbSet<UserTermsAcceptance> UserTermsAcceptances => Set<UserTermsAcceptance>();
    public DbSet<CustomerFeedback> CustomerFeedbacks => Set<CustomerFeedback>();

    // ==================== ACTIVITY LOGS ====================
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    // ==================== PRODUCT & INVENTORY ====================
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Inventory> Inventories => Set<Inventory>();

    // ==================== ORDERS & TRANSACTIONS ====================
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    // ==================== PROMOTIONS & LOYALTY POINTS ====================
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<PromotionRedemption> PromotionRedemptions => Set<PromotionRedemption>();
    public DbSet<CustomerPoint> CustomerPoints => Set<CustomerPoint>();

    // ==================== RETENTION OFFERS ====================
    public DbSet<RetentionOffer> RetentionOffers => Set<RetentionOffer>();

    // ==================== COMPLAINTS ====================
    public DbSet<Complaint> Complaints => Set<Complaint>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ==================== ROLES ====================

        builder.Entity<Role>(entity =>
        {
            entity.HasKey(x => x.RoleId);
            entity.Property(x => x.RoleCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.RoleCode).IsUnique();
        });

        // ==================== USERS ====================

        builder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.Username).HasMaxLength(100).IsRequired();
            entity.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(200);
            entity.Property(x => x.ContactNumber).HasMaxLength(50);
            entity.HasIndex(x => x.Username).IsUnique();

            entity.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== CUSTOMERS ====================

        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.CustomerId);

            entity.Property(x => x.CustomerCode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.MiddleName)
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.ContactNumber)
                .HasMaxLength(50);

            entity.Property(x => x.EmailAddress)
                .HasMaxLength(200);

            entity.Property(x => x.Address)
                .HasMaxLength(500);

            entity.HasIndex(x => x.CustomerCode)
                .IsUnique();
        });

        // ==================== TERMS & CONDITIONS ====================

        builder.Entity<TermsAndCondition>(entity =>
        {
            entity.HasKey(x => x.TermsAndConditionId);
            entity.Property(x => x.Version).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.CreatedByRoleCode).HasMaxLength(50).IsRequired();
            entity.HasIndex(x => new { x.CreatedByRoleCode, x.Version }).IsUnique();
        });

        builder.Entity<TermsAcceptance>(entity =>
        {
            entity.HasKey(x => x.TermsAcceptanceId);
            entity.Property(x => x.IpAddress).HasMaxLength(45);

            entity.HasOne(x => x.TermsAndCondition)
                .WithMany(x => x.Acceptances)
                .HasForeignKey(x => x.TermsAndConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.TermsAcceptances)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<UserTermsAcceptance>(entity =>
        {
            entity.HasKey(x => x.UserTermsAcceptanceId);
            entity.Property(x => x.IpAddress).HasMaxLength(45);

            entity.HasOne(x => x.TermsAndCondition)
                .WithMany()
                .HasForeignKey(x => x.TermsAndConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.UserId, x.TermsAndConditionId }).IsUnique();
        });

        // ==================== CUSTOMER FEEDBACK ====================

        builder.Entity<CustomerFeedback>(entity =>
        {
            entity.HasKey(x => x.CustomerFeedbackId);
            entity.Property(x => x.Comments).HasColumnType("nvarchar(max)");
            entity.Property(x => x.Category).HasMaxLength(50);
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== PRODUCT & INVENTORY ====================

        builder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.CategoryId);
            entity.Property(x => x.CategoryCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.CategoryCode).IsUnique();
        });

        builder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.ProductId);
            entity.Property(x => x.ProductCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.HasIndex(x => x.ProductCode).IsUnique();

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Inventory>(entity =>
        {
            entity.HasKey(x => x.InventoryId);
            entity.Property(x => x.QuantityOnHand).HasPrecision(18, 2);
            entity.Property(x => x.ReorderLevel).HasPrecision(18, 2);

            entity.HasOne(x => x.Product)
                .WithOne(x => x.Inventory)
                .HasForeignKey<Inventory>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== ORDERS & TRANSACTIONS ====================

        builder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.OrderId);
            entity.Property(x => x.OrderCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.OrderType).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
            entity.Property(x => x.SubTotal).HasPrecision(18, 2);
            entity.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            entity.Property(x => x.TotalAmount).HasPrecision(18, 2);
            entity.HasIndex(x => x.OrderCode).IsUnique();

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(x => x.OrderItemId);
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.Property(x => x.LineTotal).HasPrecision(18, 2);

            entity.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.TransactionId);
            entity.Property(x => x.PaymentMethod).HasMaxLength(20).IsRequired();
            entity.Property(x => x.ReferenceNumber).HasMaxLength(100);
            entity.Property(x => x.AmountPaid).HasPrecision(18, 2);
            entity.Property(x => x.ChangeDue).HasPrecision(18, 2);

            entity.HasOne(x => x.Order)
                .WithOne(x => x.Transaction)
                .HasForeignKey<Transaction>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== ACTIVITY LOGS ====================

        builder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(x => x.ActivityLogId);
            entity.Property(x => x.Username).HasMaxLength(100);
            entity.Property(x => x.RoleCode).HasMaxLength(50);
            entity.Property(x => x.ActionType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000);

            entity.HasIndex(x => x.PerformedAt);
            entity.HasIndex(x => x.ActionType);

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ==================== PROMOTIONS ====================

        builder.Entity<Promotion>(entity =>
        {
            entity.HasKey(x => x.PromotionId);
            entity.Property(x => x.PromotionCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.PromotionName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(1000);
            entity.Property(x => x.DiscountType).HasMaxLength(20).IsRequired();
            entity.Property(x => x.DiscountValue).HasPrecision(18, 2);
            entity.Property(x => x.MinimumPurchase).HasPrecision(18, 2);
            entity.HasIndex(x => x.PromotionCode).IsUnique();
        });

        builder.Entity<PromotionRedemption>(entity =>
        {
            entity.HasKey(x => x.PromotionRedemptionId);
            entity.Property(x => x.DiscountApplied).HasPrecision(18, 2);

            entity.HasOne(x => x.Promotion)
                .WithMany(x => x.Redemptions)
                .HasForeignKey(x => x.PromotionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Redemptions)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Order)
                .WithMany(x => x.Redemptions)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== LOYALTY POINTS ====================

        builder.Entity<CustomerPoint>(entity =>
        {
            entity.HasKey(x => x.CustomerPointId);
            entity.Property(x => x.TransactionType).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Notes).HasColumnType("nvarchar(max)");

            entity.HasIndex(x => x.PerformedAt);
            entity.HasIndex(x => x.CustomerId);

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.PointTransactions)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ==================== RETENTION OFFERS ====================

        builder.Entity<RetentionOffer>(entity =>
        {
            entity.HasKey(x => x.RetentionOfferId);
            entity.Property(x => x.OfferText).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired();

            entity.HasIndex(x => x.CustomerId);
            entity.HasIndex(x => x.Status);

            entity.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Promotion)
                .WithMany()
                .HasForeignKey(x => x.PromotionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ==================== COMPLAINTS ====================

        builder.Entity<Complaint>(entity =>
        {
            entity.HasKey(x => x.ComplaintId);

            entity.Property(x => x.ComplaintCode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Category)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Severity)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.Subject)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.Property(x => x.ResolutionNotes)
                .HasColumnType("nvarchar(max)");

            entity.HasIndex(x => x.ComplaintCode).IsUnique();
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.CustomerId);
            entity.HasIndex(x => x.IsArchived);

            entity.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.AssignedToUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}