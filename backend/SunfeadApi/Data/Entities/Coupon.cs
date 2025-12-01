using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// promotional coupon/discount code
/// normalization: separated usage tracking to coupon_usage table
/// </summary>
public class Coupon : BaseEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!; // unique coupon code
    public string? Description { get; set; }
    public CouponType Type { get; set; }
    
    // value interpretation depends on type: percentage (0-100) or fixed amount
    public decimal Value { get; set; }
    
    public decimal? MinOrderValue { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    // null means unlimited
    public int? UsageLimit { get; set; }
    public int? PerUserLimit { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // navigation properties
    public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
}
