using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface ITokenRepository
{
    Task<RefreshToken> GetRefreshToken(string refreshToken);
    Task AddRefreshToken(RefreshToken refreshToken);
    Task InvalidateAllUsersTokens(string userId, string tokenId);
}
