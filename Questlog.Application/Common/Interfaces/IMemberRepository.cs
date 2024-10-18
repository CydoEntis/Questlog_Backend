using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IMemberRepository : IBaseRepository<Member>
{
    Task<Member> UpdateAsync(Member entity);
}
