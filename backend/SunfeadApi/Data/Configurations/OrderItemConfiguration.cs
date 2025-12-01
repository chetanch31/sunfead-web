using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for order item entity
/// normalization: stores immutable snapshots (name, sku, price, tax) for audit compliance
/// retains variant_id for post-order analytics while keeping order history intact
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        
        builder.HasKey(oi => oi.Id);
        
        builder.Property(oi => oi.SkuSnapshot)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(oi => oi.ProductNameSnapshot)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(oi => oi.VariantNameSnapshot)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(oi => oi.Qty)
            .IsRequired();
        
        builder.Property(oi => oi.UnitPriceSnapshot)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(oi => oi.MrpSnapshot)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(oi => oi.TaxAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(oi => oi.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(oi => oi.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        // indexes
        builder.HasIndex(oi => oi.OrderId);
        
        builder.HasIndex(oi => oi.VariantId);
        
        // relationships
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(oi => oi.Variant)
            .WithMany(v => v.OrderItems)
            .HasForeignKey(oi => oi.VariantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(oi => oi.TaxRateSnapshot)
            .WithMany()
            .HasForeignKey(oi => oi.TaxRateSnapshotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
