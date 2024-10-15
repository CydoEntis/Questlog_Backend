using Questlog.Application.Common.Interfaces;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IUserRepository User { get; private set; }
    public ITokenRepository Token { get; private set; }
    public IGuildRepository Guild { get; private set; }
    public IGuildMemberRepository GuildMember { get; private set; }
    public IPartyRepository Party { get; private set; }
    public IPartyMemberRepository PartyMember { get; private set; }



    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;

        User = new UserRepository(db);
        Token = new TokenRepository(db);


        Guild = new GuildRepository(db);
        GuildMember = new GuildMemberRepository(db);

        Party = new PartyRepository(db);
        PartyMember = new PartyMemberRepository(db);
    }


    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
