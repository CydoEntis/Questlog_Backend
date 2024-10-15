using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IGuildRepository : IBaseRepository<Guild>
{
    Task<Guild> UpdateAsync(Guild entity);
}
