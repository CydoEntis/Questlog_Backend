using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Queries;
using Questlog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();

            return entity;
        }

        public async Task<List<T>> GetAllAsync(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet;

            query = ApplyBaseFilter(query, options.Filter);

            if (!string.IsNullOrEmpty(options.Role))
                query = ApplyRoleFilter(query, options.Role);

            if (options.FromDate.HasValue || options.ToDate.HasValue)
                query = ApplyDateFilter(query, options.DatePropertyName, options.FromDate, options.ToDate);

            if (!string.IsNullOrEmpty(options.IncludeProperties))
                query = IncludeProperties(query, options.IncludeProperties);

            if (options.OrderBy != null)
                query = ApplyOrdering(query, options.OrderBy, options.Ascending);

            return await Paginate(query, options.PageNumber, options.PageSize);
        }





        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
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
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
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

        private IQueryable<T> ApplyDateFilter(IQueryable<T> query, string datePropertyName, DateTime? fromDate, DateTime? toDate)
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

        private IQueryable<T> ApplyOrdering(IQueryable<T> query, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, bool ascending)
        {
            if (orderBy != null)
            {
                query = ascending ? orderBy(query) : orderBy(query).Reverse();
            }
            return query;
        }

        private async Task<List<T>> Paginate(IQueryable<T> query, int pageNumber, int pageSize)
        {
            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
