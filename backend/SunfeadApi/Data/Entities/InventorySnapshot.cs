namespace SunfeadApi.Data.Entities;

/// <summary>
/// materialized inventory snapshot for read performance
/// normalization note: this should be implemented as a database view or maintained table
/// updated by scheduled job or trigger from inventory_batch and inventory_transaction
/// provides fast read access while keeping normalized write model in batch/transaction tables
/// todo: implement as postgres materialized view or maintain via background job
/// </summary>
public class InventorySnapshot
{
    public Guid VariantId { get; set; }
    public int TotalAvailableQty { get; set; }
    public int TotalReservedQty { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    // navigation properties
    public ProductVariant Variant { get; set; } = null!;
}
