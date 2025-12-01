using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for inventory transaction entity
/// normalization: all stock movements recorded as signed transactions for complete audit trail
/// actual inventory derived from batches + transactions; no denormalized count in variant
/// </summary>
public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("inventory_transactions");
        
        builder.HasKey(it => it.Id);
        
        builder.Property(it => it.ChangeQty)
            .IsRequired();
        
        builder.Property(it => it.Reason)
            .IsRequired()
            .HasConversion<string>(); // store enum as string for readability
        
        builder.Property(it => it.ReferenceId)
            .HasMaxLength(100);
        
        builder.Property(it => it.Notes)
            .HasMaxLength(1000);
        
        // indexes for audit queries
        builder.HasIndex(it => it.VariantId);
        
        builder.HasIndex(it => it.BatchId);
        
        builder.HasIndex(it => new { it.Reason, it.CreatedAt });
        
        builder.HasIndex(it => it.ReferenceId);
        
        // relationships
        builder.HasOne(it => it.Variant)
            .WithMany(v => v.InventoryTransactions)
            .HasForeignKey(it => it.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(it => it.Batch)
            .WithMany(b => b.Transactions)
            .HasForeignKey(it => it.BatchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
