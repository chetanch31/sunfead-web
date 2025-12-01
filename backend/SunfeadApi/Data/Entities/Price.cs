using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// current active price for a variant
/// normalization: separated from variant to allow historical pricing and promotional changes
/// unique constraint: one active row per variant (where effective_to is null)
/// </summary>
public class Price : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public string Currency { get; set; } = "INR";
    public decimal Mrp { get; set; }
    public decimal SellingPrice { get; set; }
    public Guid GstRateId { get; set; }
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    
    // null means current active price
    public DateTime? EffectiveTo { get; set; }
    
    // navigation properties
    public ProductVariant Variant { get; set; } = null!;
    public TaxRate GstRate { get; set; } = null!;
}
