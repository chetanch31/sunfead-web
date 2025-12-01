using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for address entity
/// normalization: separated from user for reusability and guest checkout support
/// todo: enforce one default address per user via application-level transaction or db check constraint
/// </summary>
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(a => a.Phone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(a => a.AddressLine1)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(a => a.AddressLine2)
            .HasMaxLength(500);
        
        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.State)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Pincode)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("India");
        
        builder.Property(a => a.Gstin)
            .HasMaxLength(15);
        
        builder.Property(a => a.Lat)
            .HasPrecision(10, 8);
        
        builder.Property(a => a.Lng)
            .HasPrecision(11, 8);
        
        // indexes for pincode-based queries
        builder.HasIndex(a => a.Pincode);
        
        builder.HasIndex(a => new { a.UserId, a.IsDefault });
        
        // relationship configured in UserConfiguration
    }
}
