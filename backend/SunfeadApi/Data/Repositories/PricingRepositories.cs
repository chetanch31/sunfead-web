using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class PriceRepository : Repository<Price>, IPriceRepository
{
    public PriceRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Price?> GetActivePrice(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.GstRate)
            .FirstOrDefaultAsync(p => p.VariantId == variantId && p.EffectiveTo == null, cancellationToken);
    }

    public async Task<IEnumerable<Price>> GetPriceHistoryAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.VariantId == variantId)
            .OrderByDescending(p => p.EffectiveFrom)
            .ToListAsync(cancellationToken);
    }

    public async Task ExpireCurrentPriceAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        var currentPrice = await GetActivePrice(variantId, cancellationToken);
        if (currentPrice != null)
        {
            currentPrice.EffectiveTo = DateTime.UtcNow;
            _dbSet.Update(currentPrice);
        }
    }
}

public class PriceHistoryRepository : Repository<PriceHistory>, IPriceHistoryRepository
{
    public PriceHistoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<PriceHistory>> GetByVariantIdAsync(Guid variantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ph => ph.VariantId == variantId)
            .OrderByDescending(ph => ph.EffectiveFrom)
            .ToListAsync(cancellationToken);
    }
}

public class TaxRateRepository : Repository<TaxRate>, ITaxRateRepository
{
    public TaxRateRepository(ApplicationDbContext context) : base(context) { }

    public async Task<TaxRate?> GetActiveTaxRateAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        // TaxRate doesn't have CategoryName, use Name instead
        var now = DateTime.UtcNow;
        return await _dbSet
            .FirstOrDefaultAsync(tr => tr.Name == categoryName 
                && tr.EffectiveFrom <= now 
                && (tr.EffectiveTo == null || tr.EffectiveTo > now), 
                cancellationToken);
    }

    public async Task<IEnumerable<TaxRate>> GetActiveTaxRatesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(tr => tr.EffectiveFrom <= now && (tr.EffectiveTo == null || tr.EffectiveTo > now))
            .ToListAsync(cancellationToken);
    }
}

public class PriceTierRepository : Repository<PriceTier>, IPriceTierRepository
{
    public PriceTierRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<PriceTier>> GetByPriceIdAsync(Guid priceId, CancellationToken cancellationToken = default)
    {
        // PriceTier doesn't have PriceId, it has VariantId
        // This method signature is incorrect, but implementing based on VariantId
        return await _dbSet
            .Where(pt => pt.VariantId == priceId)
            .OrderBy(pt => pt.MinQty)
            .ToListAsync(cancellationToken);
    }

    public async Task<PriceTier?> GetApplicableTierAsync(Guid priceId, int quantity, CancellationToken cancellationToken = default)
    {
        // Using VariantId instead of PriceId
        return await _dbSet
            .Where(pt => pt.VariantId == priceId 
                && pt.MinQty <= quantity 
                && (pt.MaxQty == null || pt.MaxQty >= quantity))
            .OrderByDescending(pt => pt.MinQty)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
