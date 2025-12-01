using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(r => r.NormalizedName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(r => r.Description)
            .HasMaxLength(500);
        
        // indexes
        builder.HasIndex(r => r.NormalizedName)
            .IsUnique();
    }
}
