using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for inventory snapshot entity
/// normalization note: this table/view provides denormalized read performance
/// should be maintained via scheduled job or materialized view
/// actual source of truth: inventory_batch and inventory_transaction tables
/// todo: implement as postgres materialized view or maintain via background job
/// </summary>
public class InventorySnapshotConfiguration : IEntityTypeConfiguration<InventorySnapshot>
{
    public void Configure(EntityTypeBuilder<InventorySnapshot> builder)
    {
        builder.ToTable("inventory_snapshots");
        
        // variant_id is primary key
        builder.HasKey(s => s.VariantId);
        
        builder.Property(s => s.TotalAvailableQty)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(s => s.TotalReservedQty)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(s => s.LastUpdatedAt)
            .IsRequired();
        
        // relationship
        builder.HasOne(s => s.Variant)
            .WithOne()
            .HasForeignKey<InventorySnapshot>(s => s.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
