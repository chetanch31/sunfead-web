using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for cart entity
/// normalization: separated from user to support guest carts and cart expiry
/// </summary>
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.ExpiresAt)
            .IsRequired();
        
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);
        
        // indexes
        builder.HasIndex(c => c.UserId);
        
        builder.HasIndex(c => new { c.IsActive, c.ExpiresAt });
        
        // relationships
        builder.HasOne(c => c.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
