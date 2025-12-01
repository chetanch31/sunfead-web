using SunfeadApi.Data.Common;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// individual items in a return request
/// normalization: allows partial returns of order items
/// </summary>
public class ReturnItem : BaseEntity
{
    public Guid Id { get; set; }
    public Guid ReturnRequestId { get; set; }
    public Guid OrderItemId { get; set; }
    public Guid VariantId { get; set; }
    
    public int Qty { get; set; }
    public string? Reason { get; set; }
    
    // navigation properties
    public ReturnRequest ReturnRequest { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;
}
