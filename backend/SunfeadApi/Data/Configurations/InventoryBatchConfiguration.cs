using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for inventory batch entity
/// normalization: batch-level tracking enables expiry management and fifo/fefo allocation
/// optimistic concurrency for concurrent inventory updates
/// </summary>
public class InventoryBatchConfiguration : IEntityTypeConfiguration<InventoryBatch>
{
    public void Configure(EntityTypeBuilder<InventoryBatch> builder)
    {
        builder.ToTable("inventory_batches");
        
        builder.HasKey(ib => ib.Id);
        
        builder.Property(ib => ib.BatchNumber)
            .HasMaxLength(100);
        
        builder.Property(ib => ib.QuantityReceived)
            .IsRequired();
        
        builder.Property(ib => ib.QuantityAvailable)
            .IsRequired();
        
        builder.Property(ib => ib.QuantityReserved)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(ib => ib.ReceivedAt)
            .IsRequired();
        
        // optimistic concurrency
        builder.Property(ib => ib.RowVersion)
            .IsRowVersion()
            .IsRequired();
        
        // indexes for inventory queries
        builder.HasIndex(ib => ib.VariantId);
        
        builder.HasIndex(ib => new { ib.VariantId, ib.ExpiryDate });
        
        builder.HasIndex(ib => ib.WarehouseId);
        
        // relationships
        builder.HasOne(ib => ib.Variant)
            .WithMany(v => v.InventoryBatches)
            .HasForeignKey(ib => ib.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ib => ib.Warehouse)
            .WithMany(w => w.InventoryBatches)
            .HasForeignKey(ib => ib.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
