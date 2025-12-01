using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

/// <summary>
/// fluent configuration for return request entity
/// normalization: separated return items to allow partial returns
/// </summary>
public class ReturnRequestConfiguration : IEntityTypeConfiguration<ReturnRequest>
{
    public void Configure(EntityTypeBuilder<ReturnRequest> builder)
    {
        builder.ToTable("return_requests");
        
        builder.HasKey(rr => rr.Id);
        
        builder.Property(rr => rr.Reason)
            .HasMaxLength(1000);
        
        builder.Property(rr => rr.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ReturnStatus.Requested);
        
        builder.Property(rr => rr.RequestedAt)
            .IsRequired();
        
        builder.Property(rr => rr.RefundAmount)
            .HasPrecision(18, 2)
            .IsRequired();
        
        // indexes
        builder.HasIndex(rr => rr.OrderId);
        
        builder.HasIndex(rr => rr.UserId);
        
        builder.HasIndex(rr => new { rr.Status, rr.RequestedAt });
        
        // relationships
        builder.HasOne(rr => rr.Order)
            .WithMany(o => o.ReturnRequests)
            .HasForeignKey(rr => rr.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(rr => rr.User)
            .WithMany()
            .HasForeignKey(rr => rr.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
