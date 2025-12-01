namespace SunfeadApi.DTOs;

// Product DTOs
public record ProductDto(
    Guid Id,
    string Name,
    string Slug,
    string? ShortDescription,
    Guid CategoryId,
    string CategoryName,
    Guid? BrandId,
    string? BrandName,
    bool IsActive,
    DateTime CreatedAt
);

public record ProductDetailDto(
    Guid Id,
    string Name,
    string Slug,
    string? ShortDescription,
    string? FullDescription,
    Guid CategoryId,
    string CategoryName,
    Guid? BrandId,
    string? BrandName,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<ProductVariantDto> Variants
);

public record ProductVariantDto(
    Guid Id,
    Guid ProductId,
    string Sku,
    string VariantName,
    int WeightInGrams,
    string? Barcode,
    bool IsActive,
    PriceDto? Price,
    int AvailableStock
);

public record CreateProductDto(
    string Name,
    string Slug,
    string? ShortDescription,
    string? FullDescription,
    Guid CategoryId,
    Guid? BrandId
);

public record UpdateProductDto(
    string Name,
    string? ShortDescription,
    string? FullDescription,
    Guid CategoryId,
    Guid? BrandId,
    bool IsActive
);

public record CreateProductVariantDto(
    string Sku,
    string VariantName,
    int WeightInGrams,
    string? Barcode
);

public record PriceDto(
    Guid Id,
    decimal BasePrice,
    decimal? SalePrice,
    string CurrencyCode,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
