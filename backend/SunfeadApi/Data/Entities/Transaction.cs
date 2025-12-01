using SunfeadApi.Data.Common;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Entities;

/// <summary>
/// payment transaction ledger
/// normalization: immutable financial records separated from orders
/// never store card details - only gateway tokens and last4
/// </summary>
public class Transaction : BaseEntity
{
    public Guid Id { get; set; }
    
    // nullable for standalone transactions (refunds, adjustments)
    public Guid? OrderId { get; set; }
    
    public string TransactionReference { get; set; } = null!; // gateway transaction id
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    
    public TransactionType Type { get; set; }
    public required Enums.PaymentMethod Method { get; set; }
    public string? Gateway { get; set; } // "razorpay", "stripe", etc.
    public TransactionStatus Status { get; set; }
    
    public DateTime InitiatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    
    // store gateway response as jsonb (postgres) for audit
    public string? RawPayload { get; set; }
    
    public ReconciliationStatus ReconciliationStatus { get; set; } = ReconciliationStatus.Pending;
    
    // navigation properties
    public Order? Order { get; set; }
}
