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