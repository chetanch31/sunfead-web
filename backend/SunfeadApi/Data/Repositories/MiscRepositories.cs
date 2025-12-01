using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.User)
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Product)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetVerifiedReviewsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.User)
            .Where(r => r.ProductId == productId && r.VerifiedPurchase)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<double> GetAverageRatingAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var reviews = await _dbSet
            .Where(r => r.ProductId == productId)
            .ToListAsync(cancellationToken);
        
        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }
}

public class BulkInquiryRepository : Repository<BulkInquiry>, IBulkInquiryRepository
{
    public BulkInquiryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<BulkInquiry>> GetByStatusAsync(BulkInquiryStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(bi => bi.Status == status)
            .OrderByDescending(bi => bi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BulkInquiry>> GetPendingInquiriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(bi => bi.Status == BulkInquiryStatus.New)
            .OrderBy(bi => bi.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        // Warehouse doesn't have Code property, use Name
        return await _dbSet
            .FirstOrDefaultAsync(w => w.Name == code, cancellationToken);
    }

    public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default)
    {
        // Warehouse doesn't have IsActive, return all
        return await _dbSet
            .OrderBy(w => w.Name)
            .ToListAsync(cancellationToken);
    }
}

public class WarehouseLocationRepository : Repository<WarehouseLocation>, IWarehouseLocationRepository
{
    public WarehouseLocationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<WarehouseLocation>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(wl => wl.WarehouseId == warehouseId)
            .OrderBy(wl => wl.Pincode)
            .ToListAsync(cancellationToken);
    }

    public async Task<WarehouseLocation?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
    {
        // WarehouseLocation doesn't have Barcode, use Pincode
        return await _dbSet
            .Include(wl => wl.Warehouse)
            .FirstOrDefaultAsync(wl => wl.Pincode == barcode, cancellationToken);
    }
}

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(al => al.EntityName == entityType && al.EntityId == entityId.ToString())
            .OrderByDescending(al => al.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(al => al.PerformedBy == userId)
            .OrderByDescending(al => al.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditLog>> GetByActionAsync(string action, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(al => al.Action == action)
            .OrderByDescending(al => al.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(al => al.CreatedAt >= from && al.CreatedAt <= to)
            .OrderByDescending(al => al.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
