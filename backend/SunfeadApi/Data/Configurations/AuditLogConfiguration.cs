using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for audit log entity
/// generic change tracking for compliance and debugging
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        
        builder.HasKey(al => al.Id);
        
        builder.Property(al => al.EntityName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(al => al.EntityId)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(al => al.Action)
            .IsRequired()
            .HasMaxLength(50);
        
        // store changes as json/jsonb for postgres
        builder.Property(al => al.ChangesJson)
            .HasColumnType("text");
        
        // indexes for audit queries
        builder.HasIndex(al => new { al.EntityName, al.EntityId });
        
        builder.HasIndex(al => al.PerformedBy);
        
        builder.HasIndex(al => al.CreatedAt);
    }
}
