using Questlog.Application.Common.Interfaces;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IUserRepository User { get; private set; }
    public ITokenRepository Token { get; private set; }
    public ICampaignRepository Campaign { get; private set; }
    public IMemberRepository Member { get; private set; }

    public IQuestRepository Quest { get; private set; }

    public ITaskRepository Task { get; private set; }



    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;

        User = new UserRepository(db);
        Token = new TokenRepository(db);


        Campaign = new CampaignRepository(db);
        Member = new MemberRepository(db);
        Quest = new QuestRepository(db);
        Task = new TaskRepository(db);

        
    }


    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
