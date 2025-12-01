using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// cart item with price snapshot reference
/// normalization: references price at time of add-to-cart to avoid inconsistencies during checkout
/// todo: implement atomic inventory reservation in service layer before checkout
/// </summary>
public class CartItem : BaseEntity
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid VariantId { get; set; }
    public int Qty { get; set; }
    
    // reference to price row at time of adding to cart
    public Guid UnitPriceSnapshotId { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    
    // optional reservation expiry
    public DateTime? ReservedUntil { get; set; }
    
    // navigation properties
    public Cart Cart { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;
    public Price UnitPriceSnapshot { get; set; } = null!;
}
