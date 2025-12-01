using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Repositories.Interfaces;

// promotion repositories
public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Coupon>> GetActiveCouponsAsync(CancellationToken cancellationToken = default);
    Task<bool> IsCouponValidAsync(string code, CancellationToken cancellationToken = default);
}

public interface ICouponUsageRepository : IRepository<CouponUsage>
{
    Task<IEnumerable<CouponUsage>> GetByCouponIdAsync(Guid couponId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CouponUsage>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetUsageCountAsync(Guid couponId, Guid? userId = null, CancellationToken cancellationToken = default);
    Task<bool> HasUserUsedCouponAsync(Guid userId, Guid couponId, CancellationToken cancellationToken = default);
}
