using System.Linq.Expressions;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IStepRepository : IBaseRepository<Step>
{
    // Task<PaginatedResult<Subquest>> GetAllAsync(SubquestQueryOptions options);
    Task<Step> UpdateAsync(Step entity);
}
