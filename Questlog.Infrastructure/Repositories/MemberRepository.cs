using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class MemberRepository : BaseRepository<Member>, IMemberRepository
{
    private readonly ApplicationDbContext _db;

    public MemberRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


    public async Task<Member> UpdateAsync(Member entity)
    {
        entity.UpdatedOn = DateTime.Now;
        _db.Members.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}