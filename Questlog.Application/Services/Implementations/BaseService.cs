using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Implementations;

public class BaseService
{
    protected readonly ILogger<BaseService> _logger;

    public BaseService(ILogger<BaseService> logger)
    {
        _logger = logger;
    }

    protected async Task<ServiceResult<T>> HandleExceptions<T>(Func<Task<ServiceResult<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "An error occurred while interacting with the database.");
            return ServiceResult<T>.Failure("An error occurred while interacting with the database. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            return ServiceResult<T>.Failure("An unexpected error occurred. Please try again.");
        }
    }

    protected Expression<Func<TEntity, bool>> BuildSearchFilter<TEntity>(
        string searchBy,
        string searchValue,
        Dictionary<string, Func<TEntity, string>> searchProperties)
    {
        if (searchProperties.TryGetValue(searchBy.ToLower(), out var searchProperty))
        {
            return entity => searchProperty(entity).Contains(searchValue);
        }

        return entity => entity.ToString().Contains(searchValue);
    }


    protected Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> BuildOrdering<TEntity>(
        string sortBy,
        Dictionary<string, Expression<Func<TEntity, object>>> sortProperties)
    {
        if (sortProperties.TryGetValue(sortBy.ToLower(), out var sortExpression))
        {
            return query => query.OrderBy(sortExpression);
        }

        return query => query.OrderBy(entity => entity.GetType().GetProperty("Id").GetValue(entity));
    }
}