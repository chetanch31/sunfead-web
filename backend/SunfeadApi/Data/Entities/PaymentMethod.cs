using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// stored payment method (tokenized) for repeat purchases
/// normalization: separated from user for multiple payment methods
/// security: never store full card numbers, only gateway tokens and last4
/// </summary>
public class PaymentMethod : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Provider { get; set; } = null!; // "razorpay", "stripe"
    
    // tokenized payment method from gateway
    public string? Token { get; set; }
    public string? Last4 { get; set; }
    public string? CardType { get; set; } // "visa", "mastercard", "rupay"
    
    public bool IsActive { get; set; } = true;
    
    // navigation properties
    public User User { get; set; } = null!;
}
