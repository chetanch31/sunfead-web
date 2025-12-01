using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for transaction entity
/// normalization: immutable financial ledger separated from orders
/// never store sensitive card data - only gateway tokens/references
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.TransactionReference)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.Amount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("INR");
        
        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(t => t.Method)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(t => t.Gateway)
            .HasMaxLength(100);
        
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(t => t.InitiatedAt)
            .IsRequired();
        
        // store gateway payload as text/jsonb for postgres
        builder.Property(t => t.RawPayload)
            .HasColumnType("text");
        
        builder.Property(t => t.ReconciliationStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ReconciliationStatus.Pending);
        
        // indexes
        builder.HasIndex(t => t.OrderId);
        
        builder.HasIndex(t => t.TransactionReference);
        
        builder.HasIndex(t => new { t.Status, t.InitiatedAt });
        
        builder.HasIndex(t => t.ReconciliationStatus);
        
        // relationships - do not cascade delete for audit trail
        builder.HasOne(t => t.Order)
            .WithMany(o => o.Transactions)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
