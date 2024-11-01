using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class InviteTokenRepository : BaseRepository<InviteToken>, IInviteTokenRepository
{
    private readonly ApplicationDbContext _db;

    public InviteTokenRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<InviteToken> UpdateAsync(InviteToken entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _db.InviteTokens.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}