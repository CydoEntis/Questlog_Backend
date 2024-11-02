using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class UnlockedAvatarRepository : BaseRepository<UnlockedAvatar>, IUnlockedAvatarRepository
{
    private readonly ApplicationDbContext _db;

    public UnlockedAvatarRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}