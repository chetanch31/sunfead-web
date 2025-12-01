using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// order entity with optimistic concurrency
/// normalization: keeps minimal snapshots for immutability (names/prices in order_item)
/// preserves foreign keys to variant for analytics while maintaining historical accuracy
/// </summary>
public class Order : BaseEntity
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = null!; // unique human-readable order number
    
    // nullable for guest orders
    public Guid? UserId { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    // financial summary
    public decimal SubtotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    
    public DateTime? PlacedAt { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    
    // optimistic concurrency token
    public byte[] RowVersion { get; set; } = null!;
    
    // navigation properties
    public User? User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public OrderAddressSnapshot? AddressSnapshot { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    public ICollection<ReturnRequest> ReturnRequests { get; set; } = new List<ReturnRequest>();
    public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
}
