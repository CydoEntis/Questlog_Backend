using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IPartyMemberRepository : IBaseRepository<PartyMember>
{
    Task<PartyMember> UpdateAsync(PartyMember entity);
}
