namespace SunfeadApi.Data.Enums;

/// <summary>
/// order lifecycle status
/// </summary>
public enum OrderStatus
{
    Pending,
    Confirmed,
    Processing,
    Shipped,
    Delivered,
    Cancelled,
    Refunded
}

/// <summary>
/// payment processing status
/// </summary>
public enum PaymentStatus
{
    Pending,
    Authorized,
    Captured,
    Failed,
    Refunded,
    PartiallyRefunded
}

/// <summary>
/// payment method type
/// </summary>
public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    UPI,
    NetBanking,
    Wallet,
    CashOnDelivery,
    BankTransfer
}

/// <summary>
/// transaction type for financial ledger
/// </summary>
public enum TransactionType
{
    Payment,
    Refund,
    Reversal,
    Chargeback
}

/// <summary>
/// transaction processing status
/// </summary>
public enum TransactionStatus
{
    Initiated,
    Processing,
    Completed,
    Failed,
    Cancelled
}

/// <summary>
/// inventory movement reason for audit trail
/// </summary>
public enum InventoryReason
{
    Purchase,
    Sale,
    Return,
    Adjustment,
    Damage,
    Expired,
    Transfer,
    Reserved,
    Released
}

/// <summary>
/// coupon type for promotional discounts
/// </summary>
public enum CouponType
{
    Percentage,
    FixedAmount,
    FreeShipping
}

/// <summary>
/// return request processing status
/// </summary>
public enum ReturnStatus
{
    Requested,
    Approved,
    Rejected,
    PickupScheduled,
    Received,
    Refunded,
    Cancelled
}

/// <summary>
/// shipment tracking status
/// </summary>
public enum ShipmentStatus
{
    Pending,
    PickupScheduled,
    Picked,
    InTransit,
    OutForDelivery,
    Delivered,
    Failed,
    Returned
}

/// <summary>
/// bulk inquiry processing status
/// </summary>
public enum BulkInquiryStatus
{
    New,
    UnderReview,
    QuoteSent,
    Negotiating,
    Accepted,
    Rejected,
    Closed
}

/// <summary>
/// reconciliation status for payment transactions
/// </summary>
public enum ReconciliationStatus
{
    Pending,
    Reconciled,
    Mismatch,
    ManualReview
}
