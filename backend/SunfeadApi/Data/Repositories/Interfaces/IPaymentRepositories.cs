using SunfeadApi.Data.Entities;
using SunfeadApi.Data.Enums;

namespace SunfeadApi.Data.Repositories.Interfaces;

// payment repositories
public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

public interface IPaymentMethodRepository : IRepository<Entities.PaymentMethod>
{
    Task<IEnumerable<Entities.PaymentMethod>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Entities.PaymentMethod?> GetDefaultPaymentMethodAsync(Guid userId, CancellationToken cancellationToken = default);
}
