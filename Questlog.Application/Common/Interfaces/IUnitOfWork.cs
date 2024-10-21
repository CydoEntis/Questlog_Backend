namespace Questlog.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    ITokenRepository Token { get; }

    ICampaignRepository Campaign { get; }
    IMemberRepository Member { get; }

    IQuestRepository Quest { get; }

    ISubquestRepository Subquest { get; }

    
    Task SaveAsync();
}
