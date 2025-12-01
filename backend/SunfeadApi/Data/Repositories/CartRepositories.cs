using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Variant)
            .ThenInclude(v => v.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive, cancellationToken);
    }

    public async Task<Cart?> GetCartWithItemsAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Variant)
            .ThenInclude(v => v.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);
    }

    public async Task<IEnumerable<Cart>> GetAbandonedCartsAsync(int daysOld, CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
        return await _dbSet
            .Include(c => c.CartItems)
            .Where(c => c.IsActive && c.UpdatedAt < cutoffDate)
            .ToListAsync(cancellationToken);
    }
}

public class CartItemRepository : Repository<CartItem>, ICartItemRepository
{
    public CartItemRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(ci => ci.Variant)
            .ThenInclude(v => v.Product)
            .Where(ci => ci.CartId == cartId)
            .ToListAsync(cancellationToken);
    }

    public async Task<CartItem?> GetByCartAndVariantAsync(Guid cartId, Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.VariantId == variantId, cancellationToken);
    }

    public async Task RemoveItemsByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        var items = await _dbSet.Where(ci => ci.CartId == cartId).ToListAsync(cancellationToken);
        _dbSet.RemoveRange(items);
    }
}
