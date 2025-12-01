using SunfeadApi.Data.Enums;

namespace SunfeadApi.DTOs;

public record OrderDto(
    Guid Id,
    string OrderNumber,
    Guid UserId,
    OrderStatus Status,
    decimal SubTotal,
    decimal TaxAmount,
    decimal ShippingCost,
    decimal DiscountAmount,
    decimal GrandTotal,
    DateTime CreatedAt,
    IEnumerable<OrderItemDto> Items
);

public record OrderItemDto(
    Guid Id,
    Guid VariantId,
    string ProductName,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal TaxAmount,
    decimal TotalPrice
);

public record CreateOrderDto(
    Guid? CouponId,
    Guid ShippingAddressId,
    Guid BillingAddressId,
    string? SpecialInstructions
);

public record OrderSummaryDto(
    Guid Id,
    string OrderNumber,
    OrderStatus Status,
    decimal GrandTotal,
    DateTime CreatedAt,
    int ItemCount
);
