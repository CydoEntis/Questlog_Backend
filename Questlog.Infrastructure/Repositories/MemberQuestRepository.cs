using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class MemberQuestRepository : BaseRepository<MemberQuest>, IMemberQuestRepository
{
    private readonly ApplicationDbContext _db;

    public MemberQuestRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
}