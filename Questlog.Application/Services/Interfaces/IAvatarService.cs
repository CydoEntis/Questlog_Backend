using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IAvatarService
{
    Task<ServiceResult<List<AvatarDto>>> GetAvatarShop(string userId);
    Task<ServiceResult<List<AvatarDto>>> GetUnlockedAvatars(string userId);
    Task<ServiceResult<List<AvatarDto>>> GetNextUnlockableTier(string userId);

    Task<ServiceResult<AvatarDto>> UnlockAvatar(string userId, int avatarId);
}