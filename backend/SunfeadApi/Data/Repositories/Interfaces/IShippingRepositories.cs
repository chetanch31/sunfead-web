using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Repositories.Interfaces;

// shipping & returns repositories
public interface IShipmentRepository : IRepository<Shipment>
{
    Task<IEnumerable<Shipment>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Shipment>> GetByStatusAsync(ShipmentStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Shipment>> GetByCarrierAsync(string carrier, CancellationToken cancellationToken = default);
}

public interface IReturnRequestRepository : IRepository<ReturnRequest>
{
    Task<IEnumerable<ReturnRequest>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReturnRequest>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReturnRequest>> GetByStatusAsync(ReturnStatus status, CancellationToken cancellationToken = default);
    Task<ReturnRequest?> GetWithItemsAsync(Guid returnId, CancellationToken cancellationToken = default);
}

public interface IReturnItemRepository : IRepository<ReturnItem>
{
    Task<IEnumerable<ReturnItem>> GetByReturnRequestIdAsync(Guid returnRequestId, CancellationToken cancellationToken = default);
}
