using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;


namespace Questlog.Infrastructure.Repositories;

public class PartyRepository : BaseRepository<Party>, IPartyRepository
{
    private readonly ApplicationDbContext _db;

    public PartyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<Party>> GetPaginatedPartiesAsync(QueryOptions<Party> options)
    {
        IQueryable<Party> query = _dbSet;

        if (options.Filter != null)
        {
            query = query.Where(options.Filter);
        }

        if (!string.IsNullOrEmpty(options.OrderOn))
        {
            query = ApplyOrdering(query, options.OrderOn, options.OrderBy);
        }

        if (!string.IsNullOrEmpty(options.IncludeProperties))
        {
            query = ApplyIncludeProperties(query, options.IncludeProperties);
        }

        return await Paginate(query, options.PageNumber, options.PageSize);
    }

    public async Task<Party> UpdateAsync(Party entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _db.Parties.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }


    private IQueryable<Party> ApplyIncludeProperties(IQueryable<Party> query, string includeProperties)
    {
        foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProp);
        }

        return query;
    }


    private IQueryable<Party> ApplyOrdering(IQueryable<Party> query, string orderOn, string orderBy)
    {
        var orderDirection = Enum.TryParse<OrderBy>(orderBy, true, out var order) ? order : OrderBy.Desc;

        return orderOn.ToLower() switch
        {
            "title" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.Title)
                : query.OrderByDescending(c => c.Title),
            "created-at" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt),
            "updated-at" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            "due-date" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            _ => query
        };
    }
}