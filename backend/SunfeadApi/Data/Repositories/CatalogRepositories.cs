using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetActiveRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsActive && c.ParentId == null)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetChildCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);
    }
}

public class BrandRepository : Repository<Brand>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Brand?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        // Brand entity doesn't have Slug property, so use Name instead
        return await _dbSet.FirstOrDefaultAsync(b => b.Name == slug, cancellationToken);
    }

    public async Task<IEnumerable<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default)
    {
        // Brand doesn't have IsActive, return all
        return await _dbSet
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.BrandId == brandId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetWithVariantsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Variants)
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }
}

public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
{
    public ProductVariantRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(pv => pv.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(pv => pv.Product)
            .FirstOrDefaultAsync(pv => pv.Sku == sku, cancellationToken);
    }

    public async Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(pv => pv.Sku == sku, cancellationToken);
    }

    public async Task<IEnumerable<ProductVariant>> GetActiveVariantsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(pv => pv.ProductId == productId && pv.IsActive)
            .ToListAsync(cancellationToken);
    }
}
