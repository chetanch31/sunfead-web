using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Repositories.Interfaces;

// misc repositories
public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetVerifiedReviewsAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingAsync(Guid productId, CancellationToken cancellationToken = default);
}

public interface IBulkInquiryRepository : IRepository<BulkInquiry>
{
    Task<IEnumerable<BulkInquiry>> GetByStatusAsync(BulkInquiryStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<BulkInquiry>> GetPendingInquiriesAsync(CancellationToken cancellationToken = default);
}

public interface IWarehouseRepository : IRepository<Warehouse>
{
    Task<Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default);
}

public interface IWarehouseLocationRepository : IRepository<WarehouseLocation>
{
    Task<IEnumerable<WarehouseLocation>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<WarehouseLocation?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default);
}

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByActionAsync(string action, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}
