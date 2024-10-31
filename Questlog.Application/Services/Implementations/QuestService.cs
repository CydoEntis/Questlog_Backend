using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.Quest.Request;
using Questlog.Application.Common.DTOs.Task.Request;
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

    public async Task<ServiceResult<GetQuestResponseDto>> GetQuestById(int campaignId, int questId)
    {
        try
        {
            var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, "Campaign Id");
            if (!campaignIdValidationResult.IsSuccess)
                return ServiceResult<GetQuestResponseDto>.Failure(campaignIdValidationResult.ErrorMessage);

            var questIdValidationResult = ValidationHelper.ValidateId(questId, "Quest Id");
            if (!questIdValidationResult.IsSuccess)
                return ServiceResult<GetQuestResponseDto>.Failure(questIdValidationResult.ErrorMessage);


            var foundQuest =
                await _unitOfWork.Quest.GetAsync(q => q.Id == questId && q.CampaignId == campaignId,
                    includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User");


            if (foundQuest == null)
            {
                return ServiceResult<GetQuestResponseDto>.Failure("Quest not found.");
            }

            var questResponseDto = _mapper.Map<GetQuestResponseDto>(foundQuest);

            return ServiceResult<GetQuestResponseDto>.Success(questResponseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<GetQuestResponseDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<PaginatedResult<GetQuestResponseDto>>> GetAllQuests(int campaignId, string userId,
        QueryParamsDto queryParams)
    {
        return await HandleExceptions<PaginatedResult<GetQuestResponseDto>>(async () =>
        {
            var options = new QueryOptions<Quest>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                OrderBy = queryParams.OrderBy,
                OrderOn = queryParams.OrderOn,
                IncludeProperties = "Steps,MemberQuests.AssignedMember,MemberQuests.User",
                Filter = c => c.CampaignId == campaignId
            };

            if (!string.IsNullOrEmpty(queryParams.SearchValue))
            {
                options.Filter = options.Filter.And(c => c.Title.Contains(queryParams.SearchValue));
            }


            var paginatedResult = await _unitOfWork.Quest.GetPaginated(options);
            var campaignResponseDTOs = _mapper.Map<List<GetQuestResponseDto>>(paginatedResult.Items);

            var result = new PaginatedResult<GetQuestResponseDto>(campaignResponseDTOs, paginatedResult.TotalItems,
                paginatedResult.CurrentPage, queryParams.PageSize);

            return ServiceResult<PaginatedResult<GetQuestResponseDto>>.Success(result);
        });
    }


    public async Task<ServiceResult<CreateQuestResponseDto>> CreateQuest(string userId,
        CreateQuestRequestDto requestDto)
    {
        try
        {
            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userValidationResult.IsSuccess)
                return ServiceResult<CreateQuestResponseDto>.Failure(userValidationResult.ErrorMessage);

            var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Create Quest Request DTO");
            if (!campaignValidationResult.IsSuccess)
                return ServiceResult<CreateQuestResponseDto>.Failure(campaignValidationResult.ErrorMessage);

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
                    includeProperties: "Steps,MemberQuests.AssignedMember,MemberQuests.User");

            var createQuestResponseDTO = _mapper.Map<CreateQuestResponseDto>(questWithMembers);
            return ServiceResult<CreateQuestResponseDto>.Success(createQuestResponseDTO);
        }
        catch (Exception ex)
        {
            return ServiceResult<CreateQuestResponseDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }


    public async Task<ServiceResult<UpdateQuestResponseDto>> UpdateQuest(
        UpdateQuestRequestDto requestDto, string userId)
    {
        try
        {
            var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Update Quest Request DTO");
            if (!campaignValidationResult.IsSuccess)
                return ServiceResult<UpdateQuestResponseDto>.Failure(campaignValidationResult.ErrorMessage);

            var campaignIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, "Quest Id");
            if (!campaignIdValidationResult.IsSuccess)
                return ServiceResult<UpdateQuestResponseDto>.Failure(campaignIdValidationResult.ErrorMessage);


            var foundQuest =
                await _unitOfWork.Quest.GetAsync(q => q.Id == requestDto.Id && q.CampaignId == requestDto.CampaignId);

            foundQuest.Title = requestDto.Title.Trim();
            foundQuest.Description = requestDto.Description.Trim();
            foundQuest.UpdatedAt = DateTime.UtcNow;
            foundQuest.DueDate = requestDto.DueDate;

            await UpdateQuestSteps(foundQuest, requestDto.Steps);

            await UpdateMemberQuests(foundQuest, requestDto.MemberIds);

            await _unitOfWork.Quest.UpdateAsync(foundQuest);

            var responseDto = _mapper.Map<UpdateQuestResponseDto>(foundQuest);
            return ServiceResult<UpdateQuestResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<UpdateQuestResponseDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<ServiceResult<int>> DeleteQuest(int campaignId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, "Quest Id");
        if (!campaignIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(campaignIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundQuest = await _unitOfWork.Quest.GetAsync(g => g.Id == campaignId);

            if (foundQuest == null)
            {
                return ServiceResult<int>.Failure("Quest not found.");
            }

            // Delete the campaign
            await _unitOfWork.Quest.RemoveAsync(foundQuest);

            return ServiceResult<int>.Success(foundQuest.Id);
        });
    }


    private async Task UpdateQuestSteps(Quest foundQuest, IEnumerable<UpdateStepsRequestDto> steps)
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
            includeProperties: "AssignedMember,User");

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
                    await _unitOfWork.Member.GetAsync(m => m.Id == memberId, includeProperties: "AssignedMember,User");
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
}