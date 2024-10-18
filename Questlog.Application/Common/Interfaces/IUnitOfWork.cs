namespace Questlog.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    ITokenRepository Token { get; }

    ICampaignRepository Campaign { get; }
    IMemberRepository Member { get; }

    Task SaveAsync();
}
