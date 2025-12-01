using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Rating)
            .IsRequired();
        
        builder.Property(r => r.Title)
            .HasMaxLength(200);
        
        builder.Property(r => r.Body)
            .HasMaxLength(2000);
        
        builder.Property(r => r.VerifiedPurchase)
            .HasDefaultValue(false);
        
        builder.Property(r => r.IsApproved)
            .HasDefaultValue(false);
        
        // indexes
        builder.HasIndex(r => r.ProductId);
        
        builder.HasIndex(r => r.UserId);
        
        builder.HasIndex(r => new { r.ProductId, r.IsApproved });
        
        // relationships
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
