using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
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


    
    // public async Task<PaginatedResult<Campaign>> GetAllAsync(CampaignQueryOptions options)
    // {
    //     IQueryable<Campaign> query = _dbSet;
    //
    //     if (options.Filter != null)
    //     {
    //         query = query.Where(options.Filter);
    //     }
    //
    //     if (!string.IsNullOrEmpty(options.OrderOn))
    //     {
    //         query = ApplyOrdering(query, options.OrderOn, options.OrderBy);
    //     }
    //
    //     if (!string.IsNullOrEmpty(options.IncludeProperties))
    //     {
    //         query = ApplyIncludeProperties(query, options.IncludeProperties);
    //     }
    //
    //     return await Paginate(query, options.PageNumber, options.PageSize);
    // }

    public async Task<Member> UpdateAsync(Member entity)
    {
        entity.UpdatedOn = DateTime.Now;
        _db.Members.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
    
    // private IQueryable<Campaign> ApplyIncludeProperties(IQueryable<Campaign> query, string includeProperties)
    // {
    //     foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //     {
    //         query = query.Include(includeProp);
    //     }
    //     return query; 
    // }


    // private IQueryable<Campaign> ApplyOrdering(IQueryable<Campaign> query, string orderOn, string orderBy)
    // {
    //     var orderDirection = Enum.TryParse<OrderBy>(orderBy, true, out var order) ? order : OrderBy.Desc;
    //
    //     return orderOn.ToLower() switch
    //     {
    //         "name" => orderDirection == OrderBy.Asc
    //             ? query.OrderBy(c => c.Name)
    //             : query.OrderByDescending(c => c.Name),
    //         "createdat" => orderDirection == OrderBy.Asc
    //             ? query.OrderBy(c => c.CreatedAt)
    //             : query.OrderByDescending(c => c.CreatedAt),
    //         "updatedat" => orderDirection == OrderBy.Asc
    //             ? query.OrderBy(c => c.UpdatedAt)
    //             : query.OrderByDescending(c => c.UpdatedAt),
    //         "duedate" => orderDirection == OrderBy.Asc
    //             ? query.OrderBy(c => c.UpdatedAt)
    //             : query.OrderByDescending(c => c.UpdatedAt),
    //         _ => query
    //     };
    // }
}