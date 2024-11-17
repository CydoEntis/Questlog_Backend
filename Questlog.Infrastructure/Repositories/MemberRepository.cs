using Questlog.Application.Common;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class MemberRepository : BaseRepository<Member>, IMemberRepository
{
    private readonly ApplicationDbContext _db;

    public MemberRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


    public async Task<PaginatedResult<Member>> GetPaginated(QueryOptions<Member> options)
    {
        IQueryable<Member> query = _dbSet;

        if (options.Filter != null)
        {
            query = query.Where(options.Filter);
        }

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

    public async Task<Member> UpdateAsync(Member entity)
    {
        entity.UpdatedOn = DateTime.Now;
        _db.Members.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }


    private IQueryable<Member> ApplyOrdering(IQueryable<Member> query, string orderOn, string orderBy)
    {
        var orderDirection = Enum.TryParse<OrderBy>(orderBy, true, out var order) ? order : OrderBy.Desc;

        return orderOn.ToLower() switch
        {
            "name" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.User.DisplayName)
                : query.OrderByDescending(c => c.User.DisplayName),
            "created-at" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.JoinedOn)
                : query.OrderByDescending(c => c.JoinedOn),
            "updated-at" => orderDirection == OrderBy.Asc
                ? query.OrderBy(c => c.UpdatedOn)
                : query.OrderByDescending(c => c.UpdatedOn),
            _ => query
        };
    }
}