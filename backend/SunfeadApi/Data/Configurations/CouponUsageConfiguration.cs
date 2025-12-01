using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for coupon usage entity
/// normalization: tracks per-user and per-order coupon usage separately from coupon master
/// </summary>
public class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
{
    public void Configure(EntityTypeBuilder<CouponUsage> builder)
    {
        builder.ToTable("coupon_usages");
        
        builder.HasKey(cu => cu.Id);
        
        builder.Property(cu => cu.UsedAt)
            .IsRequired();
        
        builder.Property(cu => cu.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        // indexes
        builder.HasIndex(cu => cu.CouponId);
        
        builder.HasIndex(cu => cu.UserId);
        
        builder.HasIndex(cu => cu.OrderId);
        
        builder.HasIndex(cu => new { cu.CouponId, cu.UserId });
        
        // relationships
        builder.HasOne(cu => cu.Coupon)
            .WithMany(c => c.CouponUsages)
            .HasForeignKey(cu => cu.CouponId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(cu => cu.User)
            .WithMany()
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(cu => cu.Order)
            .WithMany(o => o.CouponUsages)
            .HasForeignKey(cu => cu.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
