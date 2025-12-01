using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for price entity
/// normalization: separated from variant to track pricing history and promotional changes
/// unique constraint: only one active price per variant (where effective_to is null)
/// </summary>
public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ToTable("prices");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("INR");
        
        builder.Property(p => p.Mrp)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(p => p.SellingPrice)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(p => p.EffectiveFrom)
            .IsRequired();
        
        // indexes
        builder.HasIndex(p => p.VariantId);
        
        // unique constraint: one active price per variant (EffectiveTo is null)
        builder.HasIndex(p => new { p.VariantId, p.EffectiveTo })
            .IsUnique()
            .HasFilter("[EffectiveTo] IS NULL");
        
        // relationships
        builder.HasOne(p => p.Variant)
            .WithMany(v => v.Prices)
            .HasForeignKey(p => p.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.GstRate)
            .WithMany(t => t.Prices)
            .HasForeignKey(p => p.GstRateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
