using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class CouponRepository : Repository<Coupon>, ICouponRepository
{
    public CouponRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Coupon>> GetActiveCouponsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(c => c.IsActive 
                && c.StartsAt <= now 
                && c.ExpiresAt >= now)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsCouponValidAsync(string code, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var coupon = await _dbSet
            .Include(c => c.CouponUsages)
            .FirstOrDefaultAsync(c => c.Code == code 
                && c.IsActive 
                && c.StartsAt <= now 
                && c.ExpiresAt >= now, 
                cancellationToken);

        if (coupon == null) return false;

        // Check usage limit
        if (coupon.UsageLimit.HasValue)
        {
            var usageCount = coupon.CouponUsages.Count;
            if (usageCount >= coupon.UsageLimit.Value)
                return false;
        }

        return true;
    }
}

public class CouponUsageRepository : Repository<CouponUsage>, ICouponUsageRepository
{
    public CouponUsageRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<CouponUsage>> GetByCouponIdAsync(Guid couponId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(cu => cu.User)
            .Include(cu => cu.Order)
            .Where(cu => cu.CouponId == couponId)
            .OrderByDescending(cu => cu.UsedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CouponUsage>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(cu => cu.Coupon)
            .Where(cu => cu.UserId == userId)
            .OrderByDescending(cu => cu.UsedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUsageCountAsync(Guid couponId, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(cu => cu.CouponId == couponId);
        
        if (userId.HasValue)
        {
            query = query.Where(cu => cu.UserId == userId.Value);
        }
        
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> HasUserUsedCouponAsync(Guid userId, Guid couponId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(cu => cu.UserId == userId && cu.CouponId == couponId, cancellationToken);
    }
}
