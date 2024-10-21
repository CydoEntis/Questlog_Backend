using System.Linq.Expressions;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface ISubquestRepository : IBaseRepository<Subquest>
{
    // Task<PaginatedResult<Subquest>> GetAllAsync(SubquestQueryOptions options);
    Task<Subquest> UpdateAsync(Subquest entity);
}
