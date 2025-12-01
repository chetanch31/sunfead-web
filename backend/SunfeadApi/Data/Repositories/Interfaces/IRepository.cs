namespace SunfeadApi.Data.Repositories.Interfaces;

/// <summary>
/// generic repository interface for common crud operations
/// </summary>
/// <typeparam name="TEntity">entity type</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    // query operations
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        System.Linq.Expressions.Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    
    // command operations
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    
    // pagination
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        System.Linq.Expressions.Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}
