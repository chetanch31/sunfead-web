using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class InventoryBatchRepository : Repository<InventoryBatch>, IInventoryBatchRepository
{
    public InventoryBatchRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<InventoryBatch>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ib => ib.VariantId == variantId)
            .OrderBy(ib => ib.ExpiryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryBatch>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ib => ib.WarehouseId == warehouseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryBatch>> GetAvailableBatchesAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ib => ib.VariantId == variantId && ib.QuantityAvailable > 0)
            .OrderBy(ib => ib.ExpiryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalAvailableQuantityAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ib => ib.VariantId == variantId)
            .SumAsync(ib => ib.QuantityAvailable, cancellationToken);
    }

    public async Task<InventoryBatch?> GetBatchByLotNumberAsync(string lotNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ib => ib.BatchNumber == lotNumber, cancellationToken);
    }
}

public class InventoryTransactionRepository : Repository<InventoryTransaction>, IInventoryTransactionRepository
{
    public InventoryTransactionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<InventoryTransaction>> GetByBatchIdAsync(Guid batchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(it => it.BatchId == batchId)
            .OrderByDescending(it => it.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(it => it.Batch)
            .Where(it => it.VariantId == variantId)
            .OrderByDescending(it => it.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByReasonAsync(InventoryReason reason, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(it => it.Reason == reason)
            .OrderByDescending(it => it.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(it => it.CreatedAt >= from && it.CreatedAt <= to)
            .OrderByDescending(it => it.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class InventorySnapshotRepository : Repository<InventorySnapshot>, IInventorySnapshotRepository
{
    public InventorySnapshotRepository(ApplicationDbContext context) : base(context) { }

    public async Task<InventorySnapshot?> GetLatestSnapshotAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.VariantId == variantId)
            .OrderByDescending(s => s.LastUpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventorySnapshot>> GetSnapshotHistoryAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.VariantId == variantId)
            .OrderByDescending(s => s.LastUpdatedAt)
            .ToListAsync(cancellationToken);
    }
}
