using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Repositories.Interfaces;

// catalog repositories
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetActiveRootCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetChildCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default);
}

public interface IBrandRepository : IRepository<Brand>
{
    Task<Brand?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);
}

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetWithVariantsAsync(Guid productId, CancellationToken cancellationToken = default);
}

public interface IProductVariantRepository : IRepository<ProductVariant>
{
    Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetActiveVariantsAsync(Guid productId, CancellationToken cancellationToken = default);
}
