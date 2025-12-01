using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// physical location details for warehouse
/// normalization: allows warehouse to have multiple service locations
/// </summary>
public class WarehouseLocation : BaseEntity
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public string Pincode { get; set; } = null!;
    public string AddressText { get; set; } = null!;
    
    // navigation properties
    public Warehouse Warehouse { get; set; } = null!;
}
