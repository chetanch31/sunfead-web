using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Include(o => o.AddressSnapshot)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<Order?> GetWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Include(o => o.AddressSnapshot)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(oi => oi.Order)
            .Where(oi => oi.VariantId == variantId)
            .ToListAsync(cancellationToken);
    }
}

public class OrderAddressSnapshotRepository : Repository<OrderAddressSnapshot>, IOrderAddressSnapshotRepository
{
    public OrderAddressSnapshotRepository(ApplicationDbContext context) : base(context) { }

    public async Task<OrderAddressSnapshot?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(oas => oas.OrderId == orderId, cancellationToken);
    }
}
