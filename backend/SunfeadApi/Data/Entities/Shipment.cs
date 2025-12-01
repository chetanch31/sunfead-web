using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// shipment tracking for order delivery
/// normalization: separated from order to support multiple shipments per order (split shipments)
/// </summary>
public class Shipment : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    
    public string? Provider { get; set; } // "delhivery", "bluedart", etc.
    public string? ServiceType { get; set; } // "standard", "express"
    public string? AwbNumber { get; set; } // airway bill number
    public string? ManifestUrl { get; set; }
    
    public DateTime? PickupDate { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public string? TrackingUrl { get; set; }
    
    // navigation properties
    public Order Order { get; set; } = null!;
}
