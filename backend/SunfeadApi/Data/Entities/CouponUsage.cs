using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// coupon usage tracking
/// normalization: separated from coupon to track per-user and per-order usage
/// </summary>
public class CouponUsage : BaseEntity
{
    public Guid Id { get; set; }
    public Guid CouponId { get; set; }
    
    // nullable for guest usage tracking
    public Guid? UserId { get; set; }
    public Guid? OrderId { get; set; }
    
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;
    public decimal DiscountAmount { get; set; }
    
    // navigation properties
    public Coupon Coupon { get; set; } = null!;
    public User? User { get; set; }
    public Order? Order { get; set; }
}
