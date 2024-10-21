using System.Linq.Expressions;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IQuestRepository : IBaseRepository<Quest>
{
    // Task<PaginatedResult<Quest>> GetAllAsync(QuestQueryOptions options);
    Task<Quest> UpdateAsync(Quest entity);
}
