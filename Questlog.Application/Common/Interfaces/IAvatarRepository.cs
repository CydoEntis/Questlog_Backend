using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IAvatarRepository : IBaseRepository<Avatar>
{
    Task<List<Avatar>> GetAvatarsAtNextUnlockableLevelAsync(int userLevel);
}