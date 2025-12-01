using Microsoft.EntityFrameworkCore;
using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;
using SunfeadApi.Data.Repositories.Interfaces;

namespace SunfeadApi.Data.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Transaction>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.OrderId == orderId)
            .OrderByDescending(t => t.InitiatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Transaction doesn't have UserId, join through Order
        return await _dbSet
            .Include(t => t.Order)
            .Where(t => t.Order != null && t.Order.UserId == userId)
            .OrderByDescending(t => t.InitiatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Transaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.TransactionReference == referenceNumber, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Order)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.InitiatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.InitiatedAt >= from && t.InitiatedAt <= to)
            .OrderByDescending(t => t.InitiatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Order)
            .Where(t => t.Order != null && t.Order.UserId == userId && t.Status == TransactionStatus.Completed)
            .SumAsync(t => t.Amount, cancellationToken);
    }
}

public class PaymentMethodRepository : Repository<Entities.PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Entities.PaymentMethod>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(pm => pm.UserId == userId && pm.IsActive)
            .OrderByDescending(pm => pm.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Entities.PaymentMethod?> GetDefaultPaymentMethodAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // Return the most recently used active payment method
        return await _dbSet
            .Where(pm => pm.UserId == userId && pm.IsActive)
            .OrderByDescending(pm => pm.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
