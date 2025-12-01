using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for payment method entity
/// normalization: separated from user to support multiple saved payment methods
/// security: only tokenized references, never full card numbers
/// </summary>
public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods");
        
        builder.HasKey(pm => pm.Id);
        
        builder.Property(pm => pm.Provider)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(pm => pm.Token)
            .HasMaxLength(500);
        
        builder.Property(pm => pm.Last4)
            .HasMaxLength(4);
        
        builder.Property(pm => pm.CardType)
            .HasMaxLength(50);
        
        builder.Property(pm => pm.IsActive)
            .HasDefaultValue(true);
        
        // indexes
        builder.HasIndex(pm => pm.UserId);
        
        builder.HasIndex(pm => new { pm.UserId, pm.IsActive });
        
        // relationships
        builder.HasOne(pm => pm.User)
            .WithMany(u => u.PaymentMethods)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
