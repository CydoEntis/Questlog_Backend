using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.User;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Implementations;

public class UserService : BaseService, IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IUnitOfWork unitOfWork,
        ILogger<UserService> logger, IMapper mapper, UserManager<ApplicationUser> userManager) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ServiceResult<UserDto>> GetUserById(string userId)
    {
        try
        {
            var foundUser =
                await _unitOfWork.User.GetAsync(u => u.Id == userId,
                    includeProperties: "Avatar,UnlockedAvatars,UnlockedAvatars.Avatar");
            if (foundUser == null)
            {
                return ServiceResult<UserDto>.Failure("User not found.");
            }

            var userDto = _mapper.Map<UserDto>(foundUser);

            return ServiceResult<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<UserDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<AvatarDto>> UpdateAvatar(string userId, int avatarId)
    {
        try
        {
            var foundUser = await _unitOfWork.User.GetAsync(u => u.Id == userId, includeProperties: "Avatar");
            if (foundUser == null)
            {
                return ServiceResult<AvatarDto>.Failure("User not found.");
            }


            var unlockedAvatar =
                await _unitOfWork.UnlockedAvatar.GetAsync(ua => ua.AvatarId == avatarId && ua.UserId == userId,
                    includeProperties: "Avatar");

            foundUser.AvatarId = unlockedAvatar.AvatarId;
            await _unitOfWork.SaveAsync();


            var avatarDto = new AvatarDto()
            {
                Id = unlockedAvatar.Avatar.Id,
                Name = unlockedAvatar.Avatar.Name,
                DisplayName = unlockedAvatar.Avatar.DisplayName,
                Tier = unlockedAvatar.Avatar.Tier,
                UnlockLevel = unlockedAvatar.Avatar.UnlockLevel,
                Cost = unlockedAvatar.Avatar.Cost,
                IsUnlocked = unlockedAvatar.IsUnlocked,
                UnlockedAt = unlockedAvatar.UnlockedAt
            };

            return ServiceResult<AvatarDto>.Success(avatarDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<AvatarDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<UserDto>> UpdateDisplayName(string userId, string displayName)
    {
        try
        {
            var foundUser = await _unitOfWork.User.GetAsync(u => u.Id == userId, includeProperties: "Avatar");
            if (foundUser == null)
            {
                return ServiceResult<UserDto>.Failure("User not found.");
            }


            foundUser.DisplayName = displayName;
            await _unitOfWork.SaveAsync();


            var userDto = _mapper.Map<UserDto>(foundUser);

            return ServiceResult<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<UserDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

public async Task<ServiceResult<UserStatsDto>> GetUserStats(string userId)
{
    try
    {
        var parties = await _unitOfWork.Party.GetAllAsync(p => p.Members.Any(m => m.UserId == userId)) ?? new List<Party>();
        var quests = await _unitOfWork.MemberQuest.GetAllAsync(q => q.UserId == userId) ?? new List<MemberQuest>();

        var completedQuests = quests.Where(q => q.IsCompleted);
        var inProgressQuests = quests.Where(q => !q.IsCompleted);
        var pastDueQuests = quests.Where(q => q.AssignedQuest != null && q.AssignedQuest.DueDate < DateTime.UtcNow);

        var currentDate = DateTime.UtcNow;
        var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

        var completedQuestsDuringMonth = quests.Where(q =>
            q.IsCompleted &&
            q.CompletionDate != null && 
            q.CompletionDate.Value >= startOfMonth && 
            q.CompletionDate.Value <= endOfMonth);

        var questsGroupedByDay = completedQuestsDuringMonth
            .GroupBy(q => q.CompletionDate.Value.Date)  
            .Select<IGrouping<DateTime, MemberQuest>, QuestCompletionByDayDto>(g => new QuestCompletionByDayDto
            {
                Date = g.Key.ToString("MM-dd-yyyy"),  
                QuestCount = g.Count()
            })
            .OrderBy(g => g.Date)
            .ToList();

        var questCountsPerMonth = quests
            .Where(q => q.CompletionDate != null)
            .GroupBy(q => new { q.CompletionDate.Value.Year, q.CompletionDate.Value.Month })
            .Select(g => new QuestCompletionOverTimeDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                QuestCount = g.Count()
            })
            .OrderBy(g => g.Year)
            .ThenBy(g => g.Month)
            .ToList();

        var userStatsDto = new UserStatsDto()
        {
            TotalParties = parties.Count(),
            TotalQuests = quests.Count(),
            CompletedQuests = completedQuests.Count(),
            InProgressQuests = inProgressQuests.Count(),
            PastDueQuests = pastDueQuests.Count(),
            QuestsCompletedCurrentMonth = questCountsPerMonth,
            QuestsCompletedByDay = questsGroupedByDay  
        };

        return ServiceResult<UserStatsDto>.Success(userStatsDto);
    }
    catch (Exception ex)
    {
        return ServiceResult<UserStatsDto>.Failure(
            ex.InnerException?.Message ?? ex.Message);
    }
}


}