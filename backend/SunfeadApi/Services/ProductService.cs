using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, bool? isActive = null, Guid? categoryId = null)
    {
        var (products, total) = await _unitOfWork.Products.GetPagedAsync(
            page,
            pageSize,
            p => (!isActive.HasValue || p.IsActive == isActive.Value) &&
                 (!categoryId.HasValue || p.CategoryId == categoryId.Value)
        );

        var productDtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Slug,
            p.ShortDescription,
            p.CategoryId,
            p.Category?.Name ?? "",
            p.BrandId,
            p.Brand?.Name,
            p.IsActive,
            p.CreatedAt
        ));

        return new PagedResult<ProductDto>(
            productDtos,
            page,
            pageSize,
            total,
            (int)Math.Ceiling(total / (double)pageSize)
        );
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return null;

        return await MapToProductDetailDto(product);
    }

    public async Task<ProductDetailDto?> GetProductBySlugAsync(string slug)
    {
        var product = await _unitOfWork.Products.GetBySlugAsync(slug);
        if (product == null) return null;

        return await MapToProductDetailDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
    {
        var products = await _unitOfWork.Products.GetByCategoryIdAsync(categoryId);
        
        return products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Slug,
            p.ShortDescription,
            p.CategoryId,
            p.Category?.Name ?? "",
            p.BrandId,
            p.Brand?.Name,
            p.IsActive,
            p.CreatedAt
        ));
    }

    public async Task<ProductDetailDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Slug = dto.Slug,
            ShortDescription = dto.ShortDescription,
            FullDescription = dto.FullDescription,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            IsActive = true
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return await MapToProductDetailDto(product);
    }

    public async Task<ProductDetailDto?> UpdateProductAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return null;

        product.Name = dto.Name;
        product.ShortDescription = dto.ShortDescription;
        product.FullDescription = dto.FullDescription;
        product.CategoryId = dto.CategoryId;
        product.BrandId = dto.BrandId;
        product.IsActive = dto.IsActive;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return await MapToProductDetailDto(product);
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ProductVariantDto> AddVariantAsync(Guid productId, CreateProductVariantDto dto)
    {
        var variant = new ProductVariant
        {
            ProductId = productId,
            Sku = dto.Sku,
            VariantName = dto.VariantName,
            WeightInGrams = dto.WeightInGrams,
            Barcode = dto.Barcode,
            IsActive = true,
            RowVersion = new byte[8]
        };

        await _unitOfWork.ProductVariants.AddAsync(variant);
        await _unitOfWork.SaveChangesAsync();

        return new ProductVariantDto(
            variant.Id,
            variant.ProductId,
            variant.Sku,
            variant.VariantName,
            variant.WeightInGrams,
            variant.Barcode,
            variant.IsActive,
            null,
            0
        );
    }

    public async Task<int> GetAvailableStockAsync(Guid variantId)
    {
        return await _unitOfWork.InventoryBatches.GetTotalAvailableQuantityAsync(variantId);
    }

    private async Task<ProductDetailDto> MapToProductDetailDto(Product product)
    {
        var variants = await _unitOfWork.ProductVariants.GetByProductIdAsync(product.Id);
        
        var variantDtos = new List<ProductVariantDto>();
        foreach (var variant in variants)
        {
            var price = await _unitOfWork.Prices.GetActivePrice(variant.Id);
            var stock = await _unitOfWork.InventoryBatches.GetTotalAvailableQuantityAsync(variant.Id);

            variantDtos.Add(new ProductVariantDto(
                variant.Id,
                variant.ProductId,
                variant.Sku,
                variant.VariantName,
                variant.WeightInGrams,
                variant.Barcode,
                variant.IsActive,
                price != null ? new PriceDto(
                    price.Id,
                    price.Mrp,
                    price.SellingPrice,
                    price.Currency,
                    price.EffectiveFrom,
                    price.EffectiveTo
                ) : null,
                stock
            ));
        }

        return new ProductDetailDto(
            product.Id,
            product.Name,
            product.Slug,
            product.ShortDescription,
            product.FullDescription,
            product.CategoryId,
            product.Category?.Name ?? "",
            product.BrandId,
            product.Brand?.Name,
            product.IsActive,
            product.CreatedAt,
            variantDtos
        );
    }
}
