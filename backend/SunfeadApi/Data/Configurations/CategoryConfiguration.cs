using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for category entity
/// hierarchical self-referencing structure for nested categories
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(c => c.Description)
            .HasMaxLength(1000);
        
        builder.Property(c => c.DisplayOrder)
            .HasDefaultValue(0);
        
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);
        
        // indexes
        builder.HasIndex(c => c.Slug)
            .IsUnique();
        
        builder.HasIndex(c => c.Name);
        
        builder.HasIndex(c => new { c.ParentId, c.DisplayOrder });
        
        // self-referencing relationship
        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
