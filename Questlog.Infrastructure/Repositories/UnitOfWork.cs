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
    public IMemberQuestRepository MemberQuest { get; private set; }

    public IQuestRepository Quest { get; private set; }

    public IStepRepository Step { get; private set; }

    public IInviteTokenRepository InviteToken { get; private set; }

    public IAvatarRepository Avatar { get; private set; }


    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;

        User = new UserRepository(db);
        Token = new TokenRepository(db);


        Campaign = new CampaignRepository(db);
        Member = new MemberRepository(db);
        MemberQuest = new MemberQuestRepository(db);
        Quest = new QuestRepository(db);
        Step = new StepRepository(db);
        InviteToken = new InviteTokenRepository(db);
        Avatar = new AvatarRepository(db);
    }


    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}