namespace SunfeadApi.DTOs;

public record CartDto(
    Guid Id,
    Guid UserId,
    IEnumerable<CartItemDto> Items,
    decimal SubTotal,
    int TotalItems,
    DateTime UpdatedAt
);

public record CartItemDto(
    Guid Id,
    Guid VariantId,
    string ProductName,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public record AddToCartDto(
    Guid VariantId,
    int Quantity
);

public record UpdateCartItemDto(
    int Quantity
);
