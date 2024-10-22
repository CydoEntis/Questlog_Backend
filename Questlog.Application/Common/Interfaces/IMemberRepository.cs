using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IMemberRepository : IBaseRepository<Member>
{
    Task<PaginatedResult<Member>> GetPaginated(QueryOptions<Member> options);
    Task<Member> UpdateAsync(Member entity);
}
