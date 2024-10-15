using Questlog.Application.Queries;
using System.Linq.Expressions;

namespace Questlog.Application.Common.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(QueryOptions<T> options);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
    Task<T> CreateAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAsync();
}
