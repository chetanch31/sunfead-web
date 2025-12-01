using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// address entity - can belong to user or be standalone for guest orders
/// normalized: separated from user for reusability and guest checkout
/// </summary>
public class Address : BaseEntity
{
    public Guid Id { get; set; }
    
    // nullable for guest addresses
    public Guid? UserId { get; set; }
    
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Pincode { get; set; } = null!;
    public string Country { get; set; } = "India";
    public string? Gstin { get; set; }
    public bool IsDefault { get; set; } = false;
    
    // optional geocoding
    public decimal? Lat { get; set; }
    public decimal? Lng { get; set; }
    
    // navigation properties
    public User? User { get; set; }
}
