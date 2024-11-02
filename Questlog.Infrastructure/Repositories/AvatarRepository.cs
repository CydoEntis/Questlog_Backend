using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class AvatarRepository : BaseRepository<Avatar>, IAvatarRepository
{
    private readonly ApplicationDbContext _db;

    public AvatarRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}