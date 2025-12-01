using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// inventory movement transaction log
/// normalization: all stock changes recorded as signed transactions for complete audit trail
/// actual inventory derived from batch + transactions; no denormalized count in variant
/// </summary>
public class InventoryTransaction : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    
    // optional batch reference
    public Guid? BatchId { get; set; }
    
    // signed quantity: positive for additions, negative for removals
    public int ChangeQty { get; set; }
    
    public InventoryReason Reason { get; set; }
    
    // reference to related entity (order id, return id, adjustment id)
    public string? ReferenceId { get; set; }
    
    // who performed the transaction
    public Guid? PerformedBy { get; set; }
    
    public string? Notes { get; set; }
    
    // navigation properties
    public ProductVariant Variant { get; set; } = null!;
    public InventoryBatch? Batch { get; set; }
}
