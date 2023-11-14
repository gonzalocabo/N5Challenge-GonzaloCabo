namespace N5Challenge.Infrastructure.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> Add(T entity, CancellationToken cancellationToken = default);
    Task<T> Update(T entity, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}
