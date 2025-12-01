using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Repositories.Interfaces;

// order repositories
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<Order?> GetWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default);
}

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
}

public interface IOrderAddressSnapshotRepository : IRepository<OrderAddressSnapshot>
{
    Task<OrderAddressSnapshot?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
