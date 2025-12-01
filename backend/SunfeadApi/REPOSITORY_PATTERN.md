# Repository Pattern Implementation

This document describes the repository pattern implementation for the Sunfead API ecommerce database.

## Overview

The repository pattern provides an abstraction layer between the business logic and data access layers. This implementation includes:

- **Generic Repository**: Base repository with common CRUD operations
- **Specific Repositories**: 32 specialized repositories for each entity
- **Unit of Work**: Manages transactions and coordinates repository operations
- **Dependency Injection**: Clean registration via extension methods

## Architecture

### Generic Repository (`IRepository<TEntity>`)

Provides common operations for all entities:

```csharp
- GetByIdAsync(Guid id)
- GetAllAsync()
- FindAsync(Expression<Func<TEntity, bool>> predicate)
- FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
- AnyAsync(Expression<Func<TEntity, bool>> predicate)
- CountAsync(Expression<Func<TEntity, bool>>? predicate)
- AddAsync(TEntity entity)
- AddRangeAsync(IEnumerable<TEntity> entities)
- Update(TEntity entity)
- UpdateRange(IEnumerable<TEntity> entities)
- Remove(TEntity entity)
- RemoveRange(IEnumerable<TEntity> entities)
- GetPagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? predicate)
```

### Specific Repositories

Each entity has its own repository interface with domain-specific methods:

#### User & Auth Repositories
- `IUserRepository`: Email/phone lookups, role queries
- `IRoleRepository`: Role name lookups, user role queries
- `IUserRoleRepository`: Role assignment management
- `IAddressRepository`: User address queries with default handling

#### Catalog Repositories
- `ICategoryRepository`: Slug lookups, hierarchy queries (root/child categories)
- `IBrandRepository`: Active brand queries, slug lookups
- `IProductRepository`: Featured products, category/brand filtering, variant loading
- `IProductVariantRepository`: SKU lookups, active variants

#### Pricing Repositories
- `IPriceRepository`: Active price retrieval, price expiration
- `IPriceHistoryRepository`: Historical price tracking
- `ITaxRateRepository`: Tax rate lookups by category
- `IPriceTierRepository`: Bulk pricing tier calculations

#### Inventory Repositories
- `IInventoryBatchRepository`: Batch tracking, FIFO/FEFO queries, lot number lookups
- `IInventoryTransactionRepository`: Transaction history, reason filtering
- `IInventorySnapshotRepository`: Point-in-time inventory snapshots

#### Cart Repositories
- `ICartRepository`: Active cart retrieval, abandoned cart queries
- `ICartItemRepository`: Cart item management

#### Order Repositories
- `IOrderRepository`: Order number lookups, status filtering, date range queries
- `IOrderItemRepository`: Order line items
- `IOrderAddressSnapshotRepository`: Immutable address snapshots

#### Payment Repositories
- `ITransactionRepository`: Payment transaction queries, reconciliation
- `IPaymentMethodRepository`: Saved payment methods, default selection

#### Promotion Repositories
- `ICouponRepository`: Coupon validation, active coupon queries
- `ICouponUsageRepository`: Usage tracking, user redemption history

#### Shipping & Returns Repositories
- `IShipmentRepository`: Tracking number lookups, carrier filtering
- `IReturnRequestRepository`: Return management with status filtering
- `IReturnItemRepository`: Return line items

#### Misc Repositories
- `IReviewRepository`: Product reviews, rating calculations
- `IBulkInquiryRepository`: Wholesale inquiry management
- `IWarehouseRepository`: Warehouse management
- `IWarehouseLocationRepository`: Location-based inventory
- `IAuditLogRepository`: Audit trail queries

## Unit of Work Pattern

The `IUnitOfWork` interface provides:

1. **Repository Access**: Single point to access all repositories
2. **Transaction Management**: Atomic operations across multiple repositories
3. **SaveChanges Coordination**: Single commit point for all changes

### Transaction Example

```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // create order
            var order = new Order { /* ... */ };
            await _unitOfWork.Orders.AddAsync(order);

            // add order items
            foreach (var item in dto.Items)
            {
                var orderItem = new OrderItem { /* ... */ };
                await _unitOfWork.OrderItems.AddAsync(orderItem);

                // reserve inventory
                var batch = await _unitOfWork.InventoryBatches
                    .GetAvailableBatchesAsync(item.VariantId);
                // ... inventory logic
            }

            // clear cart
            var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(dto.UserId);
            await _unitOfWork.CartItems.RemoveItemsByCartIdAsync(cart.Id);

            await _unitOfWork.CommitTransactionAsync();
            return order;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

## Dependency Injection Setup

In `Program.cs`, register all repositories:

```csharp
using SunfeadApi.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// add dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// add all repositories and unit of work
builder.Services.AddRepositories();

var app = builder.Build();
```

## Usage Examples

### Direct Repository Usage

```csharp
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepo;
    private readonly IProductVariantRepository _variantRepo;

    public ProductController(
        IProductRepository productRepo,
        IProductVariantRepository variantRepo)
    {
        _productRepo = productRepo;
        _variantRepo = variantRepo;
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ProductDto>> GetBySlug(string slug)
    {
        var product = await _productRepo.GetBySlugAsync(slug);
        if (product == null) return NotFound();

        var variants = await _variantRepo.GetActiveVariantsAsync(product.Id);
        
        return Ok(new ProductDto(product, variants));
    }
}
```

### Unit of Work Usage

```csharp
public class CartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddToCartAsync(Guid userId, Guid variantId, int quantity)
    {
        // get or create cart
        var cart = await _unitOfWork.Carts.GetActiveCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        // check if item already in cart
        var existingItem = await _unitOfWork.CartItems
            .GetByCartAndVariantAsync(cart.Id, variantId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            _unitOfWork.CartItems.Update(existingItem);
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                VariantId = variantId,
                Quantity = quantity
            };
            await _unitOfWork.CartItems.AddAsync(newItem);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
```

## Benefits

1. **Separation of Concerns**: Business logic separated from data access
2. **Testability**: Easy to mock repositories for unit testing
3. **Maintainability**: Changes to data access don't affect business logic
4. **Reusability**: Common queries encapsulated in repository methods
5. **Transaction Management**: Coordinated multi-repository operations
6. **Query Optimization**: Include/eager loading handled in repositories

## Best Practices

1. **Keep repositories focused**: Each repository handles only its entity
2. **Use Unit of Work for transactions**: Don't mix direct DbContext with UoW
3. **Leverage specific methods**: Use domain-specific methods instead of generic Find
4. **Include related data**: Use `.Include()` in repository methods, not in services
5. **Return IEnumerable**: Use `ToListAsync()` in repositories to execute queries
6. **Async all the way**: All repository methods are async for scalability

## Testing

Mock repositories for unit testing:

```csharp
public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrder_WithValidData_CreatesOrder()
    {
        // arrange
        var mockUoW = new Mock<IUnitOfWork>();
        var mockOrderRepo = new Mock<IOrderRepository>();
        
        mockUoW.Setup(u => u.Orders).Returns(mockOrderRepo.Object);
        
        var service = new OrderService(mockUoW.Object);
        
        // act
        var result = await service.CreateOrderAsync(new CreateOrderDto { /* ... */ });
        
        // assert
        mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        mockUoW.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}
```

## Next Steps

1. Implement service layer using repositories
2. Add validation logic in services
3. Implement caching for frequently accessed data
4. Add specification pattern for complex queries (optional)
5. Implement CQRS with read/write repositories (optional)
