using AutoMapper;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.DTOs.User;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;

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

    public async Task<ServiceResult<List<AvatarDto>>> GetAvatarShop(string userId)
    {
        try
        {
            var allAvatars = await _unitOfWork.Avatar.GetAllAsync();

            var unlockedAvatars = await _unitOfWork.UnlockedAvatar.GetAllAsync(ua => ua.UserId == userId);

            var unlockedAvatarsSet = new HashSet<int>(unlockedAvatars.Select(ua => ua.AvatarId));

            var avatarDto = allAvatars.Select(avatar => new AvatarDto()
            {
                Id = avatar.Id,
                Name = avatar.Name,
                DisplayName = avatar.Name,
                Tier = avatar.Tier,
                UnlockLevel = avatar.UnlockLevel,
                Cost = avatar.Cost,
                IsUnlocked = unlockedAvatarsSet.Contains(avatar.Id)
            }).ToList();

            return ServiceResult<List<AvatarDto>>.Success(avatarDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<AvatarDto>>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<List<AvatarDto>>> GetUnlockedAvatars(string userId)
    {
        try
        {
            var unlockedAvatars =
                await _unitOfWork.UnlockedAvatar.GetAllAsync(ua => ua.UserId == userId, includeProperties: "Avatar");


            var avatarDtos = unlockedAvatars.Select(avatar => new AvatarDto()
            {
                Id = avatar.Id,
                Name = avatar.Avatar.DisplayName
            }).ToList();

            return ServiceResult<List<AvatarDto>>.Success(avatarDtos);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<AvatarDto>>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<List<AvatarDto>>> GetNextUnlockableTier(string userId)
    {
        try
        {
            var user = await _unitOfWork.User.GetUserById(userId);
            if (user == null)
            {
                return ServiceResult<List<AvatarDto>>.Failure("User not found.");
            }

            var userLevel = user.CurrentLevel;
            var avatars = await _unitOfWork.Avatar.GetAvatarsAtNextUnlockableLevelAsync(userLevel);

            var avatarDtos = _mapper.Map<List<AvatarDto>>(avatars);


            return ServiceResult<List<AvatarDto>>.Success(avatarDtos);
        }
        catch (Exception ex)
        {
            return ServiceResult<List<AvatarDto>>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<AvatarDto>> UnlockAvatar(string userId, int avatarId)
    {
        try
        {
            // Retrieve the user
            var user = await _unitOfWork.User.GetUserById(userId);
            if (user == null)
            {
                return ServiceResult<AvatarDto>.Failure("User not found.");
            }

            // Check user's level and currency
            var userLevel = user.CurrentLevel;
            var userCurrency = user.Currency;

            // Retrieve the avatar
            var avatar = await _unitOfWork.Avatar.GetAsync(a => a.Id == avatarId);
            if (avatar == null)
            {
                return ServiceResult<AvatarDto>.Failure("Avatar not found.");
            }

            // Check if user meets level and currency requirements
            if (userLevel < avatar.UnlockLevel)
                return ServiceResult<AvatarDto>.Failure("Level requirement not met.");

            if (userCurrency < avatar.Cost)
                return ServiceResult<AvatarDto>.Failure("User does not have enough currency.");

            // Check if the avatar is already unlocked
            var existingUnlockedAvatar = await _unitOfWork.UnlockedAvatar.GetAsync(
                ua => ua.AvatarId == avatarId && ua.UserId == userId);

            if (existingUnlockedAvatar != null)
            {
                return ServiceResult<AvatarDto>.Failure("Avatar already unlocked.");
            }

            var newUnlockedAvatar = new UnlockedAvatar
            {
                UserId = user.Id,
                AvatarId = avatarId,
                UnlockedAt = DateTime.UtcNow
            };

            await _unitOfWork.UnlockedAvatar.CreateAsync(newUnlockedAvatar);

            user.AvatarId = avatarId;

            await _unitOfWork.SaveAsync();

            var unlockedAvatarDto = new AvatarDto
            {
                Id = avatar.Id,
                Name = avatar.Name,
                Tier = avatar.Tier,
                UnlockLevel = avatar.UnlockLevel,
                Cost = avatar.Cost
            };

            return ServiceResult<AvatarDto>.Success(unlockedAvatarDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<AvatarDto>.Failure(ex.InnerException?.Message ?? ex.Message);
        }
    }

}