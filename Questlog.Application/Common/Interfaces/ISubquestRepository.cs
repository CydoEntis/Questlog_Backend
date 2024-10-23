using System.Linq.Expressions;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using Task = Questlog.Domain.Entities.Task;

namespace Questlog.Application.Common.Interfaces;

public interface ISubquestRepository : IBaseRepository<Task>
{
    // Task<PaginatedResult<Subquest>> GetAllAsync(SubquestQueryOptions options);
    Task<Task> UpdateAsync(Task entity);
}
