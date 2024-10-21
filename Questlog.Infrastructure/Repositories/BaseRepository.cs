using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Queries;
using Questlog.Infrastructure.Data;
using System.Linq.Expressions;
using Questlog.Application.Common.Models;

namespace Questlog.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();

        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null)
    {
        if (_dbSet == null)
        {
            throw new InvalidOperationException("DbSet is not initialized.");
        }

        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }

        return await query.ToListAsync();
    }


    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true,
        string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }


    // Apply the base filter
    private IQueryable<T> ApplyBaseFilter(IQueryable<T> query, Expression<Func<T, bool>>? filter)
    {
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    private IQueryable<T> ApplyRoleFilter(IQueryable<T> query, string? role)
    {
        if (!string.IsNullOrEmpty(role))
        {
            query = query.Where(e => EF.Property<string>(e, "Role") == role);
        }

        return query;
    }

    private IQueryable<T> ApplyDateFilter(IQueryable<T> query, string datePropertyName, DateTime? fromDate,
        DateTime? toDate)
    {
        if (fromDate.HasValue)
        {
            query = query.Where(e => EF.Property<DateTime>(e, datePropertyName) >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(e => EF.Property<DateTime>(e, datePropertyName) <= toDate.Value);
        }

        return query;
    }

    private IQueryable<T> IncludeProperties(IQueryable<T> query, string? includeProperties)
    {
        if (includeProperties != null)
        {
            foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        return query;
    }

    private IQueryable<T> ApplyOrdering(IQueryable<T> query, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        bool ascending)
    {
        if (orderBy != null)
        {
            query = ascending ? orderBy(query) : orderBy(query).Reverse();
        }

        return query;
    }


    protected async Task<PaginatedResult<T>> Paginate(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedResult<T>(items, totalCount, pageNumber, pageSize);
    }
}