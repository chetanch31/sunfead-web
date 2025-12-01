using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// historical price records for audit and analytics
/// normalization: keeps complete price change history for reporting and compliance
/// </summary>
public class PriceHistory : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public decimal Mrp { get; set; }
    public decimal SellingPrice { get; set; }
    public Guid GstRateId { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    
    // navigation properties
    public ProductVariant Variant { get; set; } = null!;
    public TaxRate GstRate { get; set; } = null!;
}
