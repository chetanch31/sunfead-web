using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for product variant entity
/// normalization: pricing separated to price table, inventory to inventory_batch
/// optimistic concurrency for concurrent stock/price updates
/// </summary>
public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");
        
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Sku)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(v => v.VariantName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(v => v.WeightInGrams)
            .IsRequired();
        
        builder.Property(v => v.Barcode)
            .HasMaxLength(100);
        
        builder.Property(v => v.IsActive)
            .HasDefaultValue(true);
        
        // optimistic concurrency
        builder.Property(v => v.RowVersion)
            .IsRowVersion()
            .IsRequired();
        
        // indexes
        builder.HasIndex(v => v.Sku)
            .IsUnique();
        
        builder.HasIndex(v => v.ProductId);
        
        builder.HasIndex(v => v.Barcode);
        
        // relationships
        builder.HasOne(v => v.Product)
            .WithMany(p => p.Variants)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
