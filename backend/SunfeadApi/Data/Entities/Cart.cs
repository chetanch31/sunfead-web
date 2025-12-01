using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// shopping cart - supports both user and guest carts
/// normalization: separated from user to support guest checkout and cart expiry
/// </summary>
public class Cart : BaseEntity
{
    public Guid Id { get; set; }
    
    // nullable for guest carts
    public Guid? UserId { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // navigation properties
    public User? User { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
