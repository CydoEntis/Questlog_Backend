using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class GuildMemberRepository : BaseRepository<GuildMember>, IGuildMemberRepository
{
    private readonly ApplicationDbContext _db;

    public GuildMemberRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


    public async Task<GuildMember> UpdateAsync(GuildMember entity)
    {
        entity.UpdatedOn = DateTime.Now;
        _db.GuildMembers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}