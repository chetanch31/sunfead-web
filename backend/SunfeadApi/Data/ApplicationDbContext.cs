using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Common;
using SunfeadApi.Data.Configurations;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data;

/// <summary>
/// application database context for snacks ecommerce
/// normalization approach: 3nf where practical - pricing, inventory, tax separated from products
/// global query filter for soft delete on entities implementing IAuditable
/// snake_case naming convention for postgres compatibility
/// todo: configure connection string via appsettings or environment variable
/// recommended: postgres for production with jsonb support
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // configure all enums to be stored as strings for SQL Server compatibility
        configurationBuilder.Properties<Enums.OrderStatus>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.PaymentStatus>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.PaymentMethod>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.TransactionType>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.TransactionStatus>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.ReconciliationStatus>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.ShipmentStatus>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.ReturnStatus>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.CouponType>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.InventoryReason>().HaveConversion<string>();
        configurationBuilder.Properties<Enums.BulkInquiryStatus>().HaveConversion<string>();
    }
    
    // user and auth
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Address> Addresses => Set<Address>();
    
    // catalog
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    
    // pricing (normalized)
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<PriceTier> PriceTiers => Set<PriceTier>();
    
    // inventory (normalized)
    public DbSet<InventoryBatch> InventoryBatches => Set<InventoryBatch>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<InventorySnapshot> InventorySnapshots => Set<InventorySnapshot>();
    
    // cart
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    
    // orders (with snapshots for immutability)
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderAddressSnapshot> OrderAddressSnapshots => Set<OrderAddressSnapshot>();
    
    // payments and transactions (immutable financial ledger)
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    
    // promotions
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
    
    // shipping
    public DbSet<Shipment> Shipments => Set<Shipment>();
    
    // returns
    public DbSet<ReturnRequest> ReturnRequests => Set<ReturnRequest>();
    public DbSet<ReturnItem> ReturnItems => Set<ReturnItem>();
    
    // reviews
    public DbSet<Review> Reviews => Set<Review>();
    
    // bulk orders
    public DbSet<BulkInquiry> BulkInquiries => Set<BulkInquiry>();
    
    // warehouses
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    
    // audit
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // apply all fluent configurations from Configurations folder
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
        
        modelBuilder.ApplyConfiguration(new PriceConfiguration());
        modelBuilder.ApplyConfiguration(new PriceHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new TaxRateConfiguration());
        modelBuilder.ApplyConfiguration(new PriceTierConfiguration());
        
        modelBuilder.ApplyConfiguration(new InventoryBatchConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new InventorySnapshotConfiguration());
        
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new OrderAddressSnapshotConfiguration());
        
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
        
        modelBuilder.ApplyConfiguration(new CouponConfiguration());
        modelBuilder.ApplyConfiguration(new CouponUsageConfiguration());
        
        modelBuilder.ApplyConfiguration(new ShipmentConfiguration());
        
        modelBuilder.ApplyConfiguration(new ReturnRequestConfiguration());
        modelBuilder.ApplyConfiguration(new ReturnItemConfiguration());
        
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        
        modelBuilder.ApplyConfiguration(new BulkInquiryConfiguration());
        
        modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
        modelBuilder.ApplyConfiguration(new WarehouseLocationConfiguration());
        
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        
        // global query filter for soft delete
        // applies to all entities implementing IAuditable
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(IAuditable.IsDeleted));
                var filterExpression = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false)),
                    parameter
                );
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filterExpression);
            }
        }
    }
    
    /// <summary>
    /// override SaveChangesAsync to automatically set audit fields
    /// todo: implement user context service to populate created_by/updated_by
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditable>();
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                // todo: set entry.Entity.CreatedBy from current user context
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                // todo: set entry.Entity.UpdatedBy from current user context
            }
            else if (entry.State == EntityState.Deleted)
            {
                // soft delete
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
                // todo: set entry.Entity.DeletedBy from current user context
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}
