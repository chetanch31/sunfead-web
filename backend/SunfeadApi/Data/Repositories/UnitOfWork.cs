using Microsoft.EntityFrameworkCore.Storage;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

/// <summary>
/// unit of work implementation managing all repositories and database transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    // user & auth repositories
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IUserRoleRepository? _userRoles;
    private IAddressRepository? _addresses;

    // catalog repositories
    private ICategoryRepository? _categories;
    private IBrandRepository? _brands;
    private IProductRepository? _products;
    private IProductVariantRepository? _productVariants;

    // pricing repositories
    private IPriceRepository? _prices;
    private IPriceHistoryRepository? _priceHistories;
    private ITaxRateRepository? _taxRates;
    private IPriceTierRepository? _priceTiers;

    // inventory repositories
    private IInventoryBatchRepository? _inventoryBatches;
    private IInventoryTransactionRepository? _inventoryTransactions;
    private IInventorySnapshotRepository? _inventorySnapshots;

    // cart repositories
    private ICartRepository? _carts;
    private ICartItemRepository? _cartItems;

    // order repositories
    private IOrderRepository? _orders;
    private IOrderItemRepository? _orderItems;
    private IOrderAddressSnapshotRepository? _orderAddressSnapshots;

    // payment repositories
    private ITransactionRepository? _transactions;
    private IPaymentMethodRepository? _paymentMethods;

    // promotion repositories
    private ICouponRepository? _coupons;
    private ICouponUsageRepository? _couponUsages;

    // shipping & returns repositories
    private IShipmentRepository? _shipments;
    private IReturnRequestRepository? _returnRequests;
    private IReturnItemRepository? _returnItems;

    // misc repositories
    private IReviewRepository? _reviews;
    private IBulkInquiryRepository? _bulkInquiries;
    private IWarehouseRepository? _warehouses;
    private IWarehouseLocationRepository? _warehouseLocations;
    private IAuditLogRepository? _auditLogs;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // user & auth repository properties
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
    public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context);
    public IAddressRepository Addresses => _addresses ??= new AddressRepository(_context);

    // catalog repository properties
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    public IBrandRepository Brands => _brands ??= new BrandRepository(_context);
    public IProductRepository Products => _products ??= new ProductRepository(_context);
    public IProductVariantRepository ProductVariants => _productVariants ??= new ProductVariantRepository(_context);

    // pricing repository properties
    public IPriceRepository Prices => _prices ??= new PriceRepository(_context);
    public IPriceHistoryRepository PriceHistories => _priceHistories ??= new PriceHistoryRepository(_context);
    public ITaxRateRepository TaxRates => _taxRates ??= new TaxRateRepository(_context);
    public IPriceTierRepository PriceTiers => _priceTiers ??= new PriceTierRepository(_context);

    // inventory repository properties
    public IInventoryBatchRepository InventoryBatches => _inventoryBatches ??= new InventoryBatchRepository(_context);
    public IInventoryTransactionRepository InventoryTransactions => _inventoryTransactions ??= new InventoryTransactionRepository(_context);
    public IInventorySnapshotRepository InventorySnapshots => _inventorySnapshots ??= new InventorySnapshotRepository(_context);

    // cart repository properties
    public ICartRepository Carts => _carts ??= new CartRepository(_context);
    public ICartItemRepository CartItems => _cartItems ??= new CartItemRepository(_context);

    // order repository properties
    public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
    public IOrderItemRepository OrderItems => _orderItems ??= new OrderItemRepository(_context);
    public IOrderAddressSnapshotRepository OrderAddressSnapshots => _orderAddressSnapshots ??= new OrderAddressSnapshotRepository(_context);

    // payment repository properties
    public ITransactionRepository Transactions => _transactions ??= new TransactionRepository(_context);
    public IPaymentMethodRepository PaymentMethods => _paymentMethods ??= new PaymentMethodRepository(_context);

    // promotion repository properties
    public ICouponRepository Coupons => _coupons ??= new CouponRepository(_context);
    public ICouponUsageRepository CouponUsages => _couponUsages ??= new CouponUsageRepository(_context);

    // shipping & returns repository properties
    public IShipmentRepository Shipments => _shipments ??= new ShipmentRepository(_context);
    public IReturnRequestRepository ReturnRequests => _returnRequests ??= new ReturnRequestRepository(_context);
    public IReturnItemRepository ReturnItems => _returnItems ??= new ReturnItemRepository(_context);

    // misc repository properties
    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);
    public IBulkInquiryRepository BulkInquiries => _bulkInquiries ??= new BulkInquiryRepository(_context);
    public IWarehouseRepository Warehouses => _warehouses ??= new WarehouseRepository(_context);
    public IWarehouseLocationRepository WarehouseLocations => _warehouseLocations ??= new WarehouseLocationRepository(_context);
    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context);

    // transaction management
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
