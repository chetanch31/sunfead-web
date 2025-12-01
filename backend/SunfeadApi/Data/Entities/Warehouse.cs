using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// warehouse location for inventory management
/// normalization: supports multi-warehouse inventory tracking
/// </summary>
public class Warehouse : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    // navigation properties
    public ICollection<WarehouseLocation> Locations { get; set; } = new List<WarehouseLocation>();
    public ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();
}
