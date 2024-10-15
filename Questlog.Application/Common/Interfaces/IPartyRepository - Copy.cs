using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IPartyRepository : IBaseRepository<Party>
{
    Task<Party> UpdateAsync(Party entity);
}
