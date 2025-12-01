using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// bulk pricing tiers for quantity-based discounts
/// normalization: separated from variant to allow multiple price breaks
/// </summary>
public class PriceTier : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    
    public int MinQty { get; set; }
    
    // null means no upper limit (e.g., 100+)
    public int? MaxQty { get; set; }
    
    public decimal Price { get; set; }
    
    // navigation properties
    public ProductVariant Variant { get; set; } = null!;
}
