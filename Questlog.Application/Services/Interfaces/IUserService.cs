using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.DTOs.User;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<UserDto>> GetUserById(string userId);
    Task<ServiceResult<AvatarDto>> UpdateAvatar(string userId, int avatarId);
    Task<ServiceResult<UserDto>> UpdateDisplayName(string userId, string displayName);
}