using SunfeadApi.Data.Entities;

namespace SunfeadApi.Data.Repositories.Interfaces;

// pricing repositories
public interface IPriceRepository : IRepository<Price>
{
    Task<Price?> GetActivePrice(Guid variantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Price>> GetPriceHistoryAsync(Guid variantId, CancellationToken cancellationToken = default);
    Task ExpireCurrentPriceAsync(Guid variantId, CancellationToken cancellationToken = default);
}

public interface IPriceHistoryRepository : IRepository<PriceHistory>
{
    Task<IEnumerable<PriceHistory>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default);
}

public interface ITaxRateRepository : IRepository<TaxRate>
{
    Task<TaxRate?> GetActiveTaxRateAsync(string categoryName, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaxRate>> GetActiveTaxRatesAsync(CancellationToken cancellationToken = default);
}

public interface IPriceTierRepository : IRepository<PriceTier>
{
    Task<IEnumerable<PriceTier>> GetByPriceIdAsync(Guid priceId, CancellationToken cancellationToken = default);
    Task<PriceTier?> GetApplicableTierAsync(Guid priceId, int quantity, CancellationToken cancellationToken = default);
}
