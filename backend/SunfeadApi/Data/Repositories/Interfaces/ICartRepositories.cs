using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Repositories.Interfaces;

// cart repositories
public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Cart?> GetCartWithItemsAsync(Guid cartId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cart>> GetAbandonedCartsAsync(int daysOld, CancellationToken cancellationToken = default);
}

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetByCartAndVariantAsync(Guid cartId, Guid variantId, CancellationToken cancellationToken = default);
    Task RemoveItemsByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default);
}
