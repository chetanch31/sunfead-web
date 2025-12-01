using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// inventory batch for tracking stock by lot/expiry
/// normalization: separates inventory from variant for batch-level expiry tracking and fifo/fefo
/// optimistic concurrency for concurrent stock updates
/// </summary>
public class InventoryBatch : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    
    // optional batch/lot number
    public string? BatchNumber { get; set; }
    
    public int QuantityReceived { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityReserved { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    
    // optional warehouse association
    public Guid? WarehouseId { get; set; }
    
    // optimistic concurrency token
    public byte[] RowVersion { get; set; } = null!;
    
    // navigation properties
    public ProductVariant Variant { get; set; } = null!;
    public Warehouse? Warehouse { get; set; }
    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();
}
