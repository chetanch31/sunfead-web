using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// return/refund request for orders
/// normalization: separated return items to allow partial returns
/// </summary>
public class ReturnRequest : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    
    public string? Reason { get; set; }
    public ReturnStatus Status { get; set; } = ReturnStatus.Requested;
    
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    
    public decimal RefundAmount { get; set; }
    
    // navigation properties
    public Order Order { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<ReturnItem> ReturnItems { get; set; } = new List<ReturnItem>();
}
