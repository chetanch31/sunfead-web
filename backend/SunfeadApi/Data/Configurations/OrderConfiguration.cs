using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for order entity
/// normalization: preserves history via snapshots in order_item and order_address_snapshot
/// retains foreign keys for analytics while keeping order data immutable
/// optimistic concurrency for concurrent order updates
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>(); // store enum as string
        
        builder.Property(o => o.SubtotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(o => o.TaxAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(o => o.ShippingAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(o => o.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(o => o.PaymentStatus)
            .IsRequired()
            .HasConversion<string>();
        
        // optimistic concurrency
        builder.Property(o => o.RowVersion)
            .IsRowVersion()
            .IsRequired();
        
        // indexes
        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();
        
        builder.HasIndex(o => o.UserId);
        
        builder.HasIndex(o => new { o.Status, o.PlacedAt });
        
        builder.HasIndex(o => o.PaymentStatus);
        
        // relationships - do not cascade delete for history preservation
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(o => o.AddressSnapshot)
            .WithOne(a => a.Order)
            .HasForeignKey<OrderAddressSnapshot>(a => a.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
