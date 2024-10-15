using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IGuildMemberRepository : IBaseRepository<GuildMember>
{
    Task<GuildMember> UpdateAsync(GuildMember entity);
}
