using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IAvatarService
{
    Task<ServiceResult<List<AvatarShopDto>>> GetAvatarShop(string userId);
    Task<ServiceResult<List<AvatarDto>>> GetUnlockedAvatars(string userId);
    Task<ServiceResult<List<AvatarShopDto>>> GetNextUnlockableTier(string userId);

    Task<ServiceResult<List<AvatarShopDto>>> UnlockAvatar(string userId, int avatarId);
}