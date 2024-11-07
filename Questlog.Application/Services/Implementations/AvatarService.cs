using AutoMapper;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Application.Services.Implementations;

public class AvatarService : BaseService, IAvatarService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public AvatarService(IUnitOfWork unitOfWork,
        ILogger<PartyService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<AvatarShopDto>>> GetAvatarShop(string userId)
    {
        try
        {
            var allAvatars = await _unitOfWork.Avatar.GetAllAsync();

            var unlockedAvatars = await _unitOfWork.UnlockedAvatar.GetAllAsync(ua => ua.UserId == userId);

            var unlockedAvatarsSet = new HashSet<int>(unlockedAvatars.Select(ua => ua.AvatarId));

            var avatarShopDtos = allAvatars.Select(avatar => new AvatarShopDto()
            {
                Id = avatar.Id,
                Name = avatar.Name,
                Tier = avatar.Tier,
                UnlockLevel = avatar.UnlockLevel,
                DisplayName = avatar.DisplayName,
                Cost = avatar.Cost,
                IsUnlocked = unlockedAvatarsSet.Contains(avatar.Id)
            }).ToList();

            return ServiceResult<List<AvatarShopDto>>.Success(avatarShopDtos);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<AvatarShopDto>>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }
}