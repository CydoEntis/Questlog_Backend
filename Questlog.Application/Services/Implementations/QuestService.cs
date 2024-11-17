using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.Step;
using Questlog.Application.Common.Extensions;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;


namespace Questlog.Application.Services.Implementations;

public class QuestService : BaseService, IQuestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;


    public QuestService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        ILogger<QuestService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ServiceResult<QuestDto>> GetQuestById(int partyId, int questId)
    {
        try
        {
            var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Party Id");
            if (!partyIdValidationResult.IsSuccess)
                return ServiceResult<QuestDto>.Failure(partyIdValidationResult.ErrorMessage);

            var questIdValidationResult = ValidationHelper.ValidateId(questId, "Quest Id");
            if (!questIdValidationResult.IsSuccess)
                return ServiceResult<QuestDto>.Failure(questIdValidationResult.ErrorMessage);


            var foundQuest =
                await _unitOfWork.Quest.GetAsync(q => q.Id == questId && q.PartyId == partyId,
                    includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User,MemberQuests.User.Avatar");


            if (foundQuest == null)
            {
                return ServiceResult<QuestDto>.Failure("Quest not found.");
            }

            var questResponseDto = _mapper.Map<QuestDto>(foundQuest);

            return ServiceResult<QuestDto>.Success(questResponseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<QuestDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }
    public async Task<ServiceResult<PaginatedResult<QuestDto>>> GetAllQuests(int partyId, string userId, QueryParamsDto queryParams)
    {
        return await HandleExceptions<PaginatedResult<QuestDto>>(async () =>
        {
            var options = new QueryOptions<Quest>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                OrderBy = queryParams.OrderBy,
                SortBy = queryParams.SortBy,
                FilterDate = queryParams.FilterDate,
                IncludeProperties = "Steps,MemberQuests.AssignedMember,MemberQuests.User,MemberQuests.User.Avatar",
                Filter = c => c.PartyId == partyId,
                StartDate = queryParams.StartDate,
                EndDate = queryParams.EndDate,
            };

            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                options.Filter = options.Filter.And(c => c.Title.Contains(queryParams.Search));
            }

            var paginatedResult = await _unitOfWork.Quest.GetPaginated(options);

            var responseDtos = _mapper.Map<List<QuestDto>>(paginatedResult.Items);

            var result = new PaginatedResult<QuestDto>(responseDtos, paginatedResult.TotalItems, paginatedResult.CurrentPage, queryParams.PageSize);
            return ServiceResult<PaginatedResult<QuestDto>>.Success(result);
        });
    }




    public async Task<ServiceResult<QuestDto>> CreateQuest(string userId,
        CreateQuestDto requestDto)
    {
        try
        {
            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userValidationResult.IsSuccess)
                return ServiceResult<QuestDto>.Failure(userValidationResult.ErrorMessage);

            var partyValidationResult = ValidationHelper.ValidateObject(requestDto, "Create Quest Request DTO");
            if (!partyValidationResult.IsSuccess)
                return ServiceResult<QuestDto>.Failure(partyValidationResult.ErrorMessage);

            var quest = _mapper.Map<Quest>(requestDto);

            Quest createdQuest = await _unitOfWork.Quest.CreateAsync(quest);

            var existingMembers = await _unitOfWork.Member.GetAllAsync(m => requestDto.MemberIds.Contains(m.Id));

            foreach (var memberId in requestDto.MemberIds)
            {
                var existingMember = existingMembers.FirstOrDefault(m => m.Id == memberId);
                if (existingMember != null)
                {
                    var memberQuest = new MemberQuest
                    {
                        AssignedQuestId = createdQuest.Id,
                        AssignedMemberId = existingMember.Id,
                        UserId = existingMember.UserId
                    };
                    createdQuest.MemberQuests.Add(memberQuest);
                }
            }


            await _unitOfWork.SaveAsync();

            var questWithMembers = await _unitOfWork.Quest
                .GetAsync(q => q.Id == createdQuest.Id,
                    includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User,MemberQuests.User.Avatar");

            var createQuestResponseDTO = _mapper.Map<QuestDto>(questWithMembers);
            return ServiceResult<QuestDto>.Success(createQuestResponseDTO);
        }
        catch (Exception ex)
        {
            return ServiceResult<QuestDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }


    public async Task<ServiceResult<QuestDto>> UpdateQuest(
        UpdateQuestDto requestDto, string userId)
    {
        try
        {
            var partyValidationResult = ValidationHelper.ValidateObject(requestDto, "Update Quest Request DTO");
            if (!partyValidationResult.IsSuccess)
                return ServiceResult<QuestDto>.Failure(partyValidationResult.ErrorMessage);

            var partyIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, "Quest Id");
            if (!partyIdValidationResult.IsSuccess)
                return ServiceResult<QuestDto>.Failure(partyIdValidationResult.ErrorMessage);


            var foundQuest =
                await _unitOfWork.Quest.GetAsync(q => q.Id == requestDto.Id && q.PartyId == requestDto.PartyId,
                    includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User,MemberQuests.User.Avatar");

            foundQuest.Title = requestDto.Title.Trim();
            foundQuest.Description = requestDto.Description.Trim();
            foundQuest.UpdatedAt = DateTime.UtcNow;
            foundQuest.DueDate = requestDto.DueDate;
            foundQuest.Priority = requestDto.Priority;

            await UpdateQuestSteps(foundQuest, requestDto.Steps);

            await UpdateMemberQuests(foundQuest, requestDto.MemberIds);

            await _unitOfWork.Quest.UpdateAsync(foundQuest);

            var party = await _unitOfWork.Party.GetAsync(c => c.Id == requestDto.PartyId);
            party.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Party.SaveAsync();

            var responseDto = _mapper.Map<QuestDto>(foundQuest);
            return ServiceResult<QuestDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<QuestDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<int>> DeleteQuest(int partyId)
    {
        var partyIdValidationResult = ValidationHelper.ValidateId(partyId, "Quest Id");
        if (!partyIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(partyIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundQuest = await _unitOfWork.Quest.GetAsync(g => g.Id == partyId);

            if (foundQuest == null)
            {
                return ServiceResult<int>.Failure("Quest not found.");
            }

            await _unitOfWork.Quest.RemoveAsync(foundQuest);

            return ServiceResult<int>.Success(foundQuest.Id);
        });
    }

    public async Task<ServiceResult<QuestDto>> CompleteQuest(int questId, string userId)
    {
        try
        {
            var quest = await _unitOfWork.Quest.GetAsync(q => q.Id == questId,
                includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User,MemberQuests.User.Avatar");

            if (quest.CompletionDate != null)
                return ServiceResult<QuestDto>.Failure("Quest has already been completed.");

            var user = await _userManager.FindByIdAsync(userId);
            var memberQuest =
                await _unitOfWork.MemberQuest.GetAsync(mq => mq.AssignedQuestId == questId && mq.UserId == userId);

            if (memberQuest.IsCompleted)
                return ServiceResult<QuestDto>.Failure("Quest not assigned to user or already completed by them.");

            

            var expReward = GetExpRewardForPriority(quest.Priority);
            var currencyReward = GetCurrencyRewardForPriority(quest.Priority);

            var existingSteps = await _unitOfWork.Step.GetAllAsync(s => s.QuestId == questId);

            
            if (user != null)
            {
                foreach (var step in existingSteps)
                {
                    step.IsCompleted = true;
                    step.UpdatedAt = DateTime.UtcNow;
                }
                
                
                user.CurrentExp += expReward;
                user.Currency += currencyReward;

                memberQuest.IsCompleted = true;
                memberQuest.AwardedExp = expReward;
                memberQuest.AwardedCurrency = currencyReward;
                memberQuest.CompletionDate = DateTime.UtcNow;

                quest.CompletionDate = DateTime.Now;
                quest.IsCompleted = true;

                await _unitOfWork.SaveAsync();

                UpdateUserLevel(user);
            }

            var responseDto = _mapper.Map<QuestDto>(quest);
            return ServiceResult<QuestDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<QuestDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }


    public async Task<ServiceResult<QuestDto>> UncompleteQuest(int questId, string userId)
    {
        try
        {
            var quest = await _unitOfWork.Quest.GetAsync(q => q.Id == questId,
                includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User,MemberQuests.User.Avatar");

            var member = await _unitOfWork.Member.GetAsync(m => m.PartyId == quest.PartyId && m.UserId == userId);
            if ((!string.Equals(member.Role, "owner", StringComparison.OrdinalIgnoreCase) &&
                 !string.Equals(member.Role, "captain", StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult<QuestDto>.Failure("Only party owners or captains can uncomplete this quest.");
            }

            var memberQuest =
                await _unitOfWork.MemberQuest.GetAsync(mq => mq.AssignedQuestId == questId && mq.UserId == userId);
            if (!memberQuest.IsCompleted)
                return ServiceResult<QuestDto>.Failure("Quest not found or not completed.");

            var user = await _userManager.FindByIdAsync(userId);
 
            var existingSteps = await _unitOfWork.Step.GetAllAsync(s => s.QuestId == questId);
            if (user != null)
            {
                foreach (var step in existingSteps)
                {
                    step.IsCompleted = false;
                    step.UpdatedAt = DateTime.UtcNow;
                }
            
            
                user.CurrentExp -= memberQuest.AwardedExp;
                user.Currency -= memberQuest.AwardedCurrency;

                memberQuest.IsCompleted = false;
                memberQuest.AwardedExp = 0;
                memberQuest.AwardedCurrency = 0;
                memberQuest.CompletionDate = null;

                quest.CompletionDate = null;
                quest.IsCompleted = false;

                await _unitOfWork.SaveAsync();

                UpdateUserLevel(user);
            }

            var responseDto = _mapper.Map<QuestDto>(quest);
            return ServiceResult<QuestDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<QuestDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }


    private async Task UpdateQuestSteps(Quest foundQuest, IEnumerable<UpdateStepDto> steps)
    {
        var existingSteps = await _unitOfWork.Step.GetAllAsync(s => s.QuestId == foundQuest.Id);

        var existingStepDict = existingSteps.ToDictionary(s => s.Id);

        foreach (var step in steps)
        {
            if (step.Id.HasValue && existingStepDict.ContainsKey(step.Id.Value))
            {
                var existingStep = existingStepDict[step.Id.Value];
                existingStep.Description = step.Description.Trim();
                existingStep.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                // Create new step
                var newStep = new Step
                {
                    Description = step.Description.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    QuestId = foundQuest.Id
                };
                foundQuest.Steps.Add(newStep);
            }
        }

        var stepIdsInUpdate = steps.Select(s => s.Id).Where(id => id.HasValue).ToHashSet();
        var stepsToRemove = existingSteps.Where(s => !stepIdsInUpdate.Contains(s.Id)).ToList();
        foreach (var stepToRemove in stepsToRemove)
        {
            _unitOfWork.Step.RemoveAsync(stepToRemove);
        }
    }


    private async Task UpdateMemberQuests(Quest foundQuest, IEnumerable<int> memberIds)
    {
        var existingMemberQuests = await _unitOfWork.MemberQuest.GetAllAsync(mq => mq.AssignedQuestId == foundQuest.Id,
            includeProperties: "AssignedMember,User,User.Avatar");

        var newMemberIds = memberIds.ToHashSet();

        foreach (var memberQuest in existingMemberQuests)
        {
            if (!newMemberIds.Contains(memberQuest.AssignedMemberId))
            {
                foundQuest.MemberQuests.Remove(memberQuest);
                await _unitOfWork.MemberQuest.RemoveAsync(memberQuest);
            }
        }

        foreach (var memberId in newMemberIds)
        {
            if (!existingMemberQuests.Any(mq => mq.AssignedMemberId == memberId))
            {
                var existingMember =
                    await _unitOfWork.Member.GetAsync(m => m.Id == memberId, includeProperties: "User,User.Avatar");
                if (existingMember != null)
                {
                    var memberQuest = new MemberQuest
                    {
                        AssignedQuestId = foundQuest.Id,
                        AssignedMemberId = existingMember.Id,
                        UserId = existingMember.UserId
                    };

                    foundQuest.MemberQuests.Add(memberQuest);
                }
            }
        }
    }

    private void UpdateUserLevel(ApplicationUser user)
    {
        while (user.CurrentExp >= user.CalculateExpForLevel(user.CurrentLevel))
        {
            LevelUp(user);
        }

        while (user.CurrentExp < 0 && user.CurrentLevel > 1)
        {
            LevelDown(user);
        }
    }

    private void LevelUp(ApplicationUser user)
    {
        if (user.CurrentExp >= user.CalculateExpForLevel(user.CurrentLevel))
        {
            user.CurrentExp -= user.CalculateExpForLevel(user.CurrentLevel);
            user.CurrentLevel++;
            user.ExpToNextLevel = user.CalculateExpForLevel(user.CurrentLevel);
        }
    }

    private void LevelDown(ApplicationUser user)
    {
        if (user.CurrentLevel > 1)
        {
            user.CurrentExp += user.CalculateExpForLevel(user.CurrentLevel - 1);
            user.CurrentLevel--;

            if (user.CurrentLevel < 1)
            {
                user.CurrentLevel = 1;
                user.CurrentExp = 0;
            }

            user.ExpToNextLevel = user.CalculateExpForLevel(user.CurrentLevel);
        }

        if (user.CurrentExp < 0)
        {
            user.CurrentExp = 0;
        }
    }

    private int GetExpRewardForPriority(string priority)
    {
        return priority switch
        {
            "Critical" => LevelUpConstants.CriticalXPReward,
            "High" => LevelUpConstants.HighXPReward,
            "Medium" => LevelUpConstants.MediumXPReward,
            "Low" => LevelUpConstants.LowXPReward,
            _ => 0
        };
    }

    private int GetCurrencyRewardForPriority(string priority)
    {
        return priority switch
        {
            "Critical" => LevelUpConstants.CriticalCurrencyReward,
            "High" => LevelUpConstants.HighCurrencyReward,
            "Medium" => LevelUpConstants.MediumCurrencyReward,
            "Low" => LevelUpConstants.LowCurrencyReward,
            _ => 0
        };
    }
}