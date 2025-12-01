using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Configurations;

public class BulkInquiryConfiguration : IEntityTypeConfiguration<BulkInquiry>
{
    public void Configure(EntityTypeBuilder<BulkInquiry> builder)
    {
        builder.ToTable("bulk_inquiries");
        
        builder.HasKey(bi => bi.Id);
        
        builder.Property(bi => bi.ContactName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(bi => bi.CompanyName)
            .HasMaxLength(300);
        
        builder.Property(bi => bi.Phone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(bi => bi.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(bi => bi.Gstin)
            .HasMaxLength(15);
        
        builder.Property(bi => bi.Pincode)
            .HasMaxLength(10);
        
        builder.Property(bi => bi.CsvFileUrl)
            .HasMaxLength(1000);
        
        builder.Property(bi => bi.PackagingPreference)
            .HasMaxLength(500);
        
        builder.Property(bi => bi.BudgetPerUnit)
            .HasPrecision(18, 2);
        
        builder.Property(bi => bi.Message)
            .HasMaxLength(2000);
        
        builder.Property(bi => bi.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(BulkInquiryStatus.New);
        
        // indexes
        builder.HasIndex(bi => bi.UserId);
        
        builder.HasIndex(bi => new { bi.Status, bi.CreatedAt });
        
        builder.HasIndex(bi => bi.AssignedTo);
        
        // relationships
        builder.HasOne(bi => bi.User)
            .WithMany()
            .HasForeignKey(bi => bi.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
