using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<bool> isUserUnique(string email);
    //Task RefreshAccessToken(TokenDTO tokenDTO);
    //Task RevokeRefreshToken(TokenDTO tokenDTO);
    Task<ApplicationUser> GetByEmail(string email);
    Task<ApplicationUser> GetUserById(string userId);


}
