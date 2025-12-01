using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for cart item entity
/// normalization: references price snapshot to avoid price changes during checkout
/// todo: implement atomic inventory reservation in service layer
/// </summary>
public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_items");
        
        builder.HasKey(ci => ci.Id);
        
        builder.Property(ci => ci.Qty)
            .IsRequired();
        
        builder.Property(ci => ci.AddedAt)
            .IsRequired();
        
        // indexes
        builder.HasIndex(ci => ci.CartId);
        
        builder.HasIndex(ci => ci.VariantId);
        
        // relationships
        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ci => ci.Variant)
            .WithMany(v => v.CartItems)
            .HasForeignKey(ci => ci.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ci => ci.UnitPriceSnapshot)
            .WithMany()
            .HasForeignKey(ci => ci.UnitPriceSnapshotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
