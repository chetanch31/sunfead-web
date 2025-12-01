using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
{
    public ShipmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Shipment>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.OrderId == orderId)
            .OrderByDescending(s => s.ShippedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Order)
            .FirstOrDefaultAsync(s => s.AwbNumber == trackingNumber, cancellationToken);
    }

    public async Task<IEnumerable<Shipment>> GetByStatusAsync(ShipmentStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Order)
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.ShippedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Shipment>> GetByCarrierAsync(string carrier, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Order)
            .Where(s => s.Provider == carrier)
            .OrderByDescending(s => s.ShippedAt)
            .ToListAsync(cancellationToken);
    }
}

public class ReturnRequestRepository : Repository<ReturnRequest>, IReturnRequestRepository
{
    public ReturnRequestRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ReturnRequest>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rr => rr.ReturnItems)
            .Where(rr => rr.OrderId == orderId)
            .OrderByDescending(rr => rr.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnRequest>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rr => rr.ReturnItems)
            .Include(rr => rr.Order)
            .Where(rr => rr.UserId == userId)
            .OrderByDescending(rr => rr.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnRequest>> GetByStatusAsync(ReturnStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rr => rr.ReturnItems)
            .Where(rr => rr.Status == status)
            .OrderByDescending(rr => rr.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReturnRequest?> GetWithItemsAsync(Guid returnId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rr => rr.ReturnItems)
            .Include(rr => rr.Order)
            .Include(rr => rr.User)
            .FirstOrDefaultAsync(rr => rr.Id == returnId, cancellationToken);
    }
}

public class ReturnItemRepository : Repository<ReturnItem>, IReturnItemRepository
{
    public ReturnItemRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ReturnItem>> GetByReturnRequestIdAsync(Guid returnRequestId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(ri => ri.OrderItem)
            .Where(ri => ri.ReturnRequestId == returnRequestId)
            .ToListAsync(cancellationToken);
    }
}
