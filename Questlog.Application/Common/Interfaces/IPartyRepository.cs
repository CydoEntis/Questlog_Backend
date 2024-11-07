using System.Linq.Expressions;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IPartyRepository : IBaseRepository<Party>
{
    Task<PaginatedResult<Party>> GetPaginatedPartiesAsync(
        QueryOptions<Party> queryOptions);

    Task<Party> UpdateAsync(Party entity);
}