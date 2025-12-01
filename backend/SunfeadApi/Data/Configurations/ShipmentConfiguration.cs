using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for shipment entity
/// normalization: separated from order to support split shipments
/// </summary>
public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("shipments");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Provider)
            .HasMaxLength(100);
        
        builder.Property(s => s.ServiceType)
            .HasMaxLength(100);
        
        builder.Property(s => s.AwbNumber)
            .HasMaxLength(100);
        
        builder.Property(s => s.ManifestUrl)
            .HasMaxLength(1000);
        
        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ShipmentStatus.Pending);
        
        builder.Property(s => s.TrackingUrl)
            .HasMaxLength(1000);
        
        // indexes
        builder.HasIndex(s => s.OrderId);
        
        builder.HasIndex(s => s.AwbNumber);
        
        builder.HasIndex(s => new { s.Status, s.ShippedAt });
        
        // relationships
        builder.HasOne(s => s.Order)
            .WithMany(o => o.Shipments)
            .HasForeignKey(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
