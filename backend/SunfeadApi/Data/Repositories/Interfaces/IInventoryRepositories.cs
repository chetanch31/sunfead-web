using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Repositories.Interfaces;

// inventory repositories
public interface IInventoryBatchRepository : IRepository<InventoryBatch>
{
    Task<IEnumerable<InventoryBatch>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryBatch>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryBatch>> GetAvailableBatchesAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<int> GetTotalAvailableQuantityAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<InventoryBatch?> GetBatchByLotNumberAsync(string lotNumber, CancellationToken cancellationToken = default);
}

public interface IInventoryTransactionRepository : IRepository<InventoryTransaction>
{
    Task<IEnumerable<InventoryTransaction>> GetByBatchIdAsync(Guid batchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryTransaction>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryTransaction>> GetByReasonAsync(InventoryReason reason, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryTransaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

public interface IInventorySnapshotRepository : IRepository<InventorySnapshot>
{
    Task<InventorySnapshot?> GetLatestSnapshotAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventorySnapshot>> GetSnapshotHistoryAsync(Guid variantId, CancellationToken cancellationToken = default);
}
