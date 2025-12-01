using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for return item entity
/// normalization: allows partial returns at item level
/// </summary>
public class ReturnItemConfiguration : IEntityTypeConfiguration<ReturnItem>
{
    public void Configure(EntityTypeBuilder<ReturnItem> builder)
    {
        builder.ToTable("return_items");
        
        builder.HasKey(ri => ri.Id);
        
        builder.Property(ri => ri.Qty)
            .IsRequired();
        
        builder.Property(ri => ri.Reason)
            .HasMaxLength(1000);
        
        // indexes
        builder.HasIndex(ri => ri.ReturnRequestId);
        
        builder.HasIndex(ri => ri.OrderItemId);
        
        builder.HasIndex(ri => ri.VariantId);
        
        // relationships
        builder.HasOne(ri => ri.ReturnRequest)
            .WithMany(rr => rr.ReturnItems)
            .HasForeignKey(ri => ri.ReturnRequestId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ri => ri.OrderItem)
            .WithMany(oi => oi.ReturnItems)
            .HasForeignKey(ri => ri.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ri => ri.Variant)
            .WithMany()
            .HasForeignKey(ri => ri.VariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
