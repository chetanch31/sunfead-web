using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for coupon entity
/// normalization: usage tracking separated to coupon_usage table
/// </summary>
public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupons");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.Description)
            .HasMaxLength(500);
        
        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(c => c.Value)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(c => c.MinOrderValue)
            .HasPrecision(18, 2);
        
        builder.Property(c => c.MaxDiscountAmount)
            .HasPrecision(18, 2);
        
        builder.Property(c => c.StartsAt)
            .IsRequired();
        
        builder.Property(c => c.ExpiresAt)
            .IsRequired();
        
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);
        
        // indexes
        builder.HasIndex(c => c.Code)
            .IsUnique();
        
        builder.HasIndex(c => new { c.IsActive, c.ExpiresAt });
    }
}
