using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface IUserRepository : IBaseRepository<ApplicationUser>
{
    Task<bool> isUserUnique(string email);
    Task<ApplicationUser> GetUserById(string userId);
}