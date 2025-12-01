using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for price history entity
/// normalization: complete audit trail of all price changes for compliance and analytics
/// </summary>
public class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.ToTable("price_histories");
        
        builder.HasKey(ph => ph.Id);
        
        builder.Property(ph => ph.Mrp)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(ph => ph.SellingPrice)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(ph => ph.EffectiveFrom)
            .IsRequired();
        
        // indexes for historical queries
        builder.HasIndex(ph => ph.VariantId);
        
        builder.HasIndex(ph => new { ph.VariantId, ph.EffectiveFrom });
        
        // relationships
        builder.HasOne(ph => ph.Variant)
            .WithMany(v => v.PriceHistories)
            .HasForeignKey(ph => ph.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ph => ph.GstRate)
            .WithMany(t => t.PriceHistories)
            .HasForeignKey(ph => ph.GstRateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
