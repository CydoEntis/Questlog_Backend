using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.Quest.Request;
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
                        UserId = existingMember.UserId // Assuming UserId is a property in the Member entity
                    };
                    createdQuest.MemberQuests.Add(memberQuest);
                }
            }

            foreach (var step in requestDto.Steps)
            {
                var newStep = new Step
                {
                    Description = step,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                createdQuest.Steps.Add(newStep);
            }

            await _unitOfWork.SaveAsync();

            var questWithMembers = await _unitOfWork.Quest
                .GetAsync(q => q.Id == createdQuest.Id,
                    includeProperties: "Tasks,MemberQuests.AssignedMember,MemberQuests.User");

            var createQuestResponseDTO = _mapper.Map<CreateQuestResponseDto>(questWithMembers);
            return ServiceResult<CreateQuestResponseDto>.Success(createQuestResponseDTO);
        }
        catch (Exception ex)
        {
            return ServiceResult<CreateQuestResponseDto>.Failure(
                ex.InnerException?.Message ?? ex.Message);
        }
    }


    // public async Task<ServiceResult<UpdateQuestDetailsResponseDto>> UpdateQuestDetails(
    //     UpdateQuestDetailsRequestDto requestDto, string userId)
    // {
    //     var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Update Quest Request DTO");
    //     if (!campaignValidationResult.IsSuccess)
    //         return ServiceResult<UpdateQuestDetailsResponseDto>.Failure(campaignValidationResult.ErrorMessage);
    //
    //     var campaignIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, "Quest Id");
    //     if (!campaignIdValidationResult.IsSuccess)
    //         return ServiceResult<UpdateQuestDetailsResponseDto>.Failure(campaignIdValidationResult.ErrorMessage);
    //
    //     if (!await IsUserQuestLeader(requestDto.Id, userId))
    //     {
    //         return ServiceResult<UpdateQuestDetailsResponseDto>.Failure(
    //             "User is not authorized to update the campaign leader.");
    //     }
    //
    //     return await HandleExceptions<UpdateQuestDetailsResponseDto>(async () =>
    //     {
    //         var foundQuest = await _unitOfWork.Quest.GetAsync(g => g.Id == requestDto.Id && g.OwnerId == userId);
    //
    //         if (foundQuest == null)
    //         {
    //             return ServiceResult<UpdateQuestDetailsResponseDto>.Failure("Quest not found.");
    //         }
    //
    //
    //         foundQuest.Name = requestDto.Name.Trim();
    //         foundQuest.Description = requestDto.Description.Trim();
    //         foundQuest.Color = requestDto.Color;
    //         foundQuest.UpdatedAt = DateTime.UtcNow;
    //         foundQuest.DueDate = requestDto.DueDate;
    //
    //
    //         await _unitOfWork.Quest.UpdateAsync(foundQuest);
    //
    //         var responseDto = _mapper.Map<UpdateQuestDetailsResponseDto>(foundQuest);
    //
    //         return ServiceResult<UpdateQuestDetailsResponseDto>.Success(responseDto);
    //     });
    // }

    // public async Task<ServiceResult<GetQuestResponseDto>> UpdateQuestLeader(int campaignId, string userId,
    //     UpdateQuestOwnerRequestDto requestDto)
    // {
    //     var validations = new[]
    //     {
    //         ValidationHelper.ValidateId(campaignId, "Quest Id"),
    //         ValidationHelper.ValidateId(requestDto.UserId, "Userid")
    //     };
    //
    //     var failedValidation = validations.FirstOrDefault(v => !v.IsSuccess);
    //     if (failedValidation != null)
    //         return ServiceResult<GetQuestResponseDto>.Failure(failedValidation.ErrorMessage);
    //
    //     if (campaignId != requestDto.QuestId)
    //         return ServiceResult<GetQuestResponseDto>.Failure("Quest member must be from same campaign");
    //
    //     if (!await IsUserQuestLeader(campaignId, userId))
    //         return ServiceResult<GetQuestResponseDto>.Failure(
    //             "User is not authorized to update the campaign leader.");
    //
    //     return await HandleExceptions<GetQuestResponseDto>(async () =>
    //     {
    //         var foundQuest = await _unitOfWork.Quest.GetAsync(g => g.Id == campaignId);
    //         if (foundQuest is null)
    //             return ServiceResult<GetQuestResponseDto>.Failure("Quest not found");
    //
    //
    //         foundQuest.OwnerId = requestDto.UserId;
    //         await _unitOfWork.Quest.UpdateAsync(foundQuest);
    //
    //         var responseDto = _mapper.Map<GetQuestResponseDto>(foundQuest);
    //
    //         return ServiceResult<GetQuestResponseDto>.Success(responseDto);
    //     });
    // }


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

    // private async Task<bool> IsUserQuestLeader(int campaignId, string userId)
    // {
    //     var campaign = await _unitOfWork.Quest.GetAsync(g => g.Id == campaignId);
    //     return campaign?.OwnerId == userId;
    // }
}