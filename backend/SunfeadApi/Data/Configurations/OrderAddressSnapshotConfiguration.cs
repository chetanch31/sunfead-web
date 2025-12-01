using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for order address snapshot entity
/// normalization: intentionally denormalized for immutability
/// order address must not change if customer updates their primary address
/// </summary>
public class OrderAddressSnapshotConfiguration : IEntityTypeConfiguration<OrderAddressSnapshot>
{
    public void Configure(EntityTypeBuilder<OrderAddressSnapshot> builder)
    {
        builder.ToTable("order_address_snapshots");
        
        builder.HasKey(oas => oas.Id);
        
        builder.Property(oas => oas.AddressText)
            .IsRequired()
            .HasMaxLength(1000);
        
        builder.Property(oas => oas.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(oas => oas.Phone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(oas => oas.Gstin)
            .HasMaxLength(15);
        
        // index
        builder.HasIndex(oas => oas.OrderId)
            .IsUnique();
        
        // relationship configured in OrderConfiguration
    }
}
