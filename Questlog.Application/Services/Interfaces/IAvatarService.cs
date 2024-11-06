using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IAvatarService
{
    Task<ServiceResult<List<AvatarShopDto>>> GetAvatarShop(string userId);
}