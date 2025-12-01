using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// user account entity with optimistic concurrency
/// normalized: password storage separate from profile, addresses in separate table
/// </summary>
public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    
    // nullable fk to default address
    public Guid? DefaultAddressId { get; set; }
    
    // optimistic concurrency token
    public byte[] RowVersion { get; set; } = null!;
    
    // navigation properties
    public Address? DefaultAddress { get; set; }
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
}
