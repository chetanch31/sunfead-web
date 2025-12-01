using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for product entity
/// normalization: contains only product identity and description
/// pricing and inventory live in separate normalized tables (price, inventory_batch)
/// todo: consider fulltext index on name/description for search (provider-specific)
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(550);
        
        builder.Property(p => p.ShortDescription)
            .HasMaxLength(1000);
        
        // html content - sanitize in application layer before saving
        builder.Property(p => p.FullDescription)
            .HasColumnType("text");
        
        builder.Property(p => p.MetaTitle)
            .HasMaxLength(200);
        
        builder.Property(p => p.MetaDescription)
            .HasMaxLength(500);
        
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);
        
        // indexes
        builder.HasIndex(p => p.Slug)
            .IsUnique();
        
        builder.HasIndex(p => p.Name);
        
        builder.HasIndex(p => p.CategoryId);
        
        builder.HasIndex(p => p.BrandId);
        
        builder.HasIndex(p => p.IsActive);
        
        // relationships
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
