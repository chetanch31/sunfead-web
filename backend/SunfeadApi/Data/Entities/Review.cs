using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// customer product review
/// </summary>
public class Review : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    
    public int Rating { get; set; } // 1-5
    public string? Title { get; set; }
    public string? Body { get; set; }
    
    public bool VerifiedPurchase { get; set; } = false;
    public bool IsApproved { get; set; } = false;
    
    // navigation properties
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
