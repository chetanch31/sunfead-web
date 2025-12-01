using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for warehouse location entity
/// normalization: allows warehouse to service multiple locations/pincodes
/// </summary>
public class WarehouseLocationConfiguration : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        builder.ToTable("warehouse_locations");
        
        builder.HasKey(wl => wl.Id);
        
        builder.Property(wl => wl.Pincode)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(wl => wl.AddressText)
            .IsRequired()
            .HasMaxLength(1000);
        
        // indexes
        builder.HasIndex(wl => wl.WarehouseId);
        
        builder.HasIndex(wl => wl.Pincode);
        
        // relationships
        builder.HasOne(wl => wl.Warehouse)
            .WithMany(w => w.Locations)
            .HasForeignKey(wl => wl.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
