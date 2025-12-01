using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// immutable address snapshot at time of order placement
/// normalization: denormalized for immutability - order address must not change if customer updates their address
/// stores full address text to preserve exact delivery information
/// </summary>
public class OrderAddressSnapshot : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    
    // denormalized address fields for immutability
    public string AddressText { get; set; } = null!; // full formatted address
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Gstin { get; set; }
    
    // navigation properties
    public Order Order { get; set; } = null!;
}
