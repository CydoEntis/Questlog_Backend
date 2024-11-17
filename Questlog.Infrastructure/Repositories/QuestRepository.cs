using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;


namespace Questlog.Infrastructure.Repositories;

public class QuestRepository : BaseRepository<Quest>, IQuestRepository
{
    private readonly ApplicationDbContext _db;

    public QuestRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<PaginatedResult<Quest>> GetPaginated(QueryOptions<Quest> options)
    {
        IQueryable<Quest> query = _dbSet;


        if (options.Filter != null)
        {
            query = query.Where(options.Filter);
        }


        query = ApplyDateFilters(query, options);

        if (!string.IsNullOrEmpty(options.SortBy))
        {
            query = ApplyOrdering(query, options.SortBy, options.OrderBy);
        }


        if (!string.IsNullOrEmpty(options.IncludeProperties))
        {
            query = ApplyIncludeProperties(query, options.IncludeProperties);
        }
        
        return await Paginate(query, options.PageNumber, options.PageSize);
    }

    public async Task<Quest> UpdateAsync(Quest entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _db.Quests.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    private IQueryable<Quest> ApplyOrdering(IQueryable<Quest> query, string filter, string orderBy)
    {
        var orderDirection = Enum.TryParse<OrderBy>(orderBy, true, out var order) ? order : OrderBy.Desc;

        return filter.ToLower() switch
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
            "priority" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedAt)
                : query.OrderByDescending(c => c.UpdatedAt),
            _ => query
        };
    }
    
    private IQueryable<Quest> ApplyDateFilters(IQueryable<Quest> query, QueryOptions<Quest> options)
    {
        // Check if StartDate is provided and is valid
        if (!string.IsNullOrEmpty(options.StartDate) && DateTime.TryParse(options.StartDate, out var startDate))
        {
            query = ApplyDateFilter(query, options.FilterDate, startDate, ">=");
        }

        // Check if EndDate is provided and is valid
        if (!string.IsNullOrEmpty(options.EndDate) && DateTime.TryParse(options.EndDate, out var endDate))
        {
            query = ApplyDateFilter(query, options.FilterDate, endDate, "<=");
        }

        return query;
    }

    private IQueryable<Quest> ApplyDateFilter(IQueryable<Quest> query, string filterDate, DateTime date, string operatorSymbol)
    {
        var dateField = filterDate switch
        {
            "created-at" => "CreatedAt",
            "updated-at" => "UpdatedAt",
            "due-date" => "DueDate",
            _ => null
        };

        if (!string.IsNullOrEmpty(dateField))
        {
            if (operatorSymbol == ">=")
            {
                query = query.Where(c => EF.Property<DateTime>(c, dateField) >= date);
            }
            else if (operatorSymbol == "<=")
            {
                query = query.Where(c => EF.Property<DateTime>(c, dateField) <= date);
            }
        }

        return query;
    }

}