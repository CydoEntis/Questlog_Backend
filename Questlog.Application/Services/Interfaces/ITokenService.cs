using Questlog.Application.Common.DTOs.Auth;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(ApplicationUser user, string tokeId);
    Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
    Task<string> CreateRefreshToken(string userId, string tokenId);
    Task InvalidateToken(RefreshToken refreshToken);
    Task RevokeRefreshToken(TokenDTO tokenDTO);
}
