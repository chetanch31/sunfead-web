namespace SunfeadApi.Data.Repositories.Interfaces;

/// <summary>
/// unit of work pattern for managing repositories and transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // user & auth repositories
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }
    IAddressRepository Addresses { get; }
    
    // catalog repositories
    ICategoryRepository Categories { get; }
    IBrandRepository Brands { get; }
    IProductRepository Products { get; }
    IProductVariantRepository ProductVariants { get; }
    
    // pricing repositories
    IPriceRepository Prices { get; }
    IPriceHistoryRepository PriceHistories { get; }
    ITaxRateRepository TaxRates { get; }
    IPriceTierRepository PriceTiers { get; }
    
    // inventory repositories
    IInventoryBatchRepository InventoryBatches { get; }
    IInventoryTransactionRepository InventoryTransactions { get; }
    IInventorySnapshotRepository InventorySnapshots { get; }
    
    // cart repositories
    ICartRepository Carts { get; }
    ICartItemRepository CartItems { get; }
    
    // order repositories
    IOrderRepository Orders { get; }
    IOrderItemRepository OrderItems { get; }
    IOrderAddressSnapshotRepository OrderAddressSnapshots { get; }
    
    // payment repositories
    ITransactionRepository Transactions { get; }
    IPaymentMethodRepository PaymentMethods { get; }
    
    // promotion repositories
    ICouponRepository Coupons { get; }
    ICouponUsageRepository CouponUsages { get; }
    
    // shipping & returns repositories
    IShipmentRepository Shipments { get; }
    IReturnRequestRepository ReturnRequests { get; }
    IReturnItemRepository ReturnItems { get; }
    
    // misc repositories
    IReviewRepository Reviews { get; }
    IBulkInquiryRepository BulkInquiries { get; }
    IWarehouseRepository Warehouses { get; }
    IWarehouseLocationRepository WarehouseLocations { get; }
    IAuditLogRepository AuditLogs { get; }
    
    // transaction management
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
