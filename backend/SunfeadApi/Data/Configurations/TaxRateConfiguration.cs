using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for tax rate entity
/// normalization: centralized tax configuration with effective date ranges
/// supports historical tax rate changes and hsn/sac code tracking
/// </summary>
public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.ToTable("tax_rates");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(t => t.RatePercent)
            .HasPrecision(5, 2)
            .IsRequired();
        
        builder.Property(t => t.HsnSacCode)
            .HasMaxLength(20);
        
        builder.Property(t => t.EffectiveFrom)
            .IsRequired();
        
        // indexes
        builder.HasIndex(t => t.Name);
        
        builder.HasIndex(t => new { t.EffectiveFrom, t.EffectiveTo });
    }
}
