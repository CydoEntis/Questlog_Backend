using Questlog.Application.Common.DTOs.Auth;
using Questlog.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Questlog.Application.Services.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(ApplicationUser user, string tokeId);
    Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
    Task<string> CreateRefreshToken(string userId, string tokenId);
    Task InvalidateToken(RefreshToken refreshToken);
    Task RevokeRefreshToken(TokenDTO tokenDTO);
}
