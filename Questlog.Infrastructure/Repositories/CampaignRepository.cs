using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;


namespace Questlog.Infrastructure.Repositories;

public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
{
    private readonly ApplicationDbContext _db;

    public CampaignRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<Campaign>> GetPaginatedCampaignsAsync(QueryOptions<Campaign> queryOptions)
    {
        IQueryable<Campaign> query = _dbSet;

        if (queryOptions.Filter != null)
        {
            query = query.Where(queryOptions.Filter);
        }

        if (!string.IsNullOrEmpty(queryOptions.SearchValue))
        {
            query = query.Where(c => c.Title.Contains(queryOptions.SearchValue));
        }

        if (!string.IsNullOrEmpty(queryOptions.OrderOn))
        {
            query = ApplyOrdering(query, queryOptions.OrderOn, queryOptions.OrderBy);
        }

        return await Paginate(query, queryOptions.PageNumber, queryOptions.PageSize);
    }


    public async Task<PaginatedResult<Campaign>> GetAllAsync(QueryOptions<Campaign> options)
    {
        IQueryable<Campaign> query = _dbSet;

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


    public async Task<Campaign> UpdateAsync(Campaign entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _db.Campaigns.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }


    private IQueryable<Campaign> ApplyIncludeProperties(IQueryable<Campaign> query, string includeProperties)
    {
        foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProp);
        }

        return query;
    }


    private IQueryable<Campaign> ApplyOrdering(IQueryable<Campaign> query, string orderOn, string orderBy)
    {
        var orderDirection = Enum.TryParse<OrderBy>(orderBy, true, out var order) ? order : OrderBy.Desc;

        return orderOn.ToLower() switch
        {
            "name" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.Title)
                : query.OrderByDescending(c => c.Title),
            "createdat" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt),
            "updatedat" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            "duedate" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            _ => query
        };
    }
}