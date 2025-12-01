using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for price tier entity
/// normalization: separated bulk pricing tiers from base price for quantity-based discounts
/// </summary>
public class PriceTierConfiguration : IEntityTypeConfiguration<PriceTier>
{
    public void Configure(EntityTypeBuilder<PriceTier> builder)
    {
        builder.ToTable("price_tiers");
        
        builder.HasKey(pt => pt.Id);
        
        builder.Property(pt => pt.MinQty)
            .IsRequired();
        
        builder.Property(pt => pt.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        // indexes
        builder.HasIndex(pt => pt.VariantId);
        
        builder.HasIndex(pt => new { pt.VariantId, pt.MinQty });
        
        // relationships
        builder.HasOne(pt => pt.Variant)
            .WithMany(v => v.PriceTiers)
            .HasForeignKey(pt => pt.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
