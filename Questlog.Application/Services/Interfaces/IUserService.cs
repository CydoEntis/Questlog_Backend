using Questlog.Application.Common.DTOs.User;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<UserDto>> GetUserById(string userId);
}