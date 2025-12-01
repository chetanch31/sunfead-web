using SunfeadApi.Data.Repositories;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Extensions;

/// <summary>
/// extension methods for registering repository services
/// </summary>
public static class RepositoryServiceExtensions
{
    /// <summary>
    /// registers all repositories and unit of work with dependency injection
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // register user & auth repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        // register catalog repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();

        // register pricing repositories
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
        services.AddScoped<ITaxRateRepository, TaxRateRepository>();
        services.AddScoped<IPriceTierRepository, PriceTierRepository>();

        // register inventory repositories
        services.AddScoped<IInventoryBatchRepository, InventoryBatchRepository>();
        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
        services.AddScoped<IInventorySnapshotRepository, InventorySnapshotRepository>();

        // register cart repositories
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();

        // register order repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderAddressSnapshotRepository, OrderAddressSnapshotRepository>();

        // register payment repositories
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

        // register promotion repositories
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ICouponUsageRepository, CouponUsageRepository>();

        // register shipping & returns repositories
        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
        services.AddScoped<IReturnItemRepository, ReturnItemRepository>();

        // register misc repositories
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IBulkInquiryRepository, BulkInquiryRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IWarehouseLocationRepository, WarehouseLocationRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // register unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
