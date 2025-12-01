using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// order item with immutable snapshots
/// normalization: stores product/sku snapshots for historical accuracy and audit compliance
/// retains variant_id foreign key for post-order analytics while keeping order immutable
/// </summary>
public class OrderItem : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid VariantId { get; set; }
    
    // immutable snapshots at time of order
    public string SkuSnapshot { get; set; } = null!;
    public string ProductNameSnapshot { get; set; } = null!;
    public string VariantNameSnapshot { get; set; } = null!;
    
    public int Qty { get; set; }
    
    // price snapshots
    public decimal UnitPriceSnapshot { get; set; } // selling price at order time
    public decimal MrpSnapshot { get; set; }
    
    // reference to tax rate for historical tax compliance
    public Guid TaxRateSnapshotId { get; set; }
    
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    // navigation properties
    public Order Order { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;
    public TaxRate TaxRateSnapshot { get; set; } = null!;
    public ICollection<ReturnItem> ReturnItems { get; set; } = new List<ReturnItem>();
}
