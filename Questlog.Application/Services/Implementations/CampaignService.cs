using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Campaign.Requests;
using Questlog.Application.Common.DTOs.Campaign.Responses;
using Questlog.Application.Common.DTOs.Campaign.Requests;
using Questlog.Application.Common.DTOs.Campaign.Responses;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.Enums;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Queries;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;


namespace Questlog.Application.Services.Implementations;

public class CampaignService : BaseService, ICampaignService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;


    public CampaignService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<CampaignService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;

    }

    public async Task<ServiceResult<GetCampaignResponseDto>> GetCampaignById(int campaignId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, "Campaign Id");
        if (!campaignIdValidationResult.IsSuccess) return ServiceResult<GetCampaignResponseDto>.Failure(campaignIdValidationResult.ErrorMessage);

        return await HandleExceptions<GetCampaignResponseDto>(async () =>
        {
            var foundCampaign = await _unitOfWork.Campaign.GetAsync(g => g.Id == campaignId, includeProperties: "Members,Members.User");

            if (foundCampaign == null)
            {
                return ServiceResult<GetCampaignResponseDto>.Failure("Campaign not found.");
            }

            var campaignResponseDTO = _mapper.Map<GetCampaignResponseDto>(foundCampaign);

            return ServiceResult<GetCampaignResponseDto>.Success(campaignResponseDTO);
        });
    }

    public async Task<ServiceResult<List<GetCampaignResponseDto>>> GetAllCampaigns(string userId, CampaignQueryParamsDto queryParams)
    {
        return await HandleExceptions<List<GetCampaignResponseDto>>(async () =>
        {

            var options = new QueryOptions<Campaign>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                IsAscending = queryParams.OrderBy == OrderByOptions.Asc.ToString(),
                FromDate = queryParams.CreatedDateFrom,
                ToDate = queryParams.CreatedDateTo,
                IncludeProperties = "Members,Members.User",
                DatePropertyName = "CreatedAt",
                Filter = g => g.Members.Any(m => m.UserId == userId)
            };

            options.OrderBy = queryParams.SortBy switch
            {
                "CreateAt" => query => query.OrderBy(g => g.CreatedAt),
                _ => query => query.OrderBy(g => g.Id)
            };

            var campaigns = await _unitOfWork.Campaign.GetAllAsync(options);

            if (campaigns == null || !campaigns.Any())
            {
                return ServiceResult<List<GetCampaignResponseDto>>.Success(new List<GetCampaignResponseDto>());
            }
            
            List<GetCampaignResponseDto> campaignResponseDTOs = _mapper.Map<List<GetCampaignResponseDto>>(campaigns);

            return ServiceResult<List<GetCampaignResponseDto>>.Success(campaignResponseDTOs);
        });
    }

    public async Task<ServiceResult<CreateCampaignResponseDto>> CreateCampaign(string userId, CreateCampaignRequestDto requestDto)
    {
        var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
        if (!userValidationResult.IsSuccess) return ServiceResult<CreateCampaignResponseDto>.Failure(userValidationResult.ErrorMessage);

        var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Create Campaign Request DTO");
        if (!campaignValidationResult.IsSuccess) return ServiceResult<CreateCampaignResponseDto>.Failure(campaignValidationResult.ErrorMessage);

        return await HandleExceptions<CreateCampaignResponseDto>(async () =>
        {
            var campaign = _mapper.Map<Campaign>(requestDto);
            campaign.OwnerId = userId;

            Campaign createdCampaign = await _unitOfWork.Campaign.CreateAsync(campaign);

            var campaignOwner = new Member
            {
                UserId = userId,
                CampaignId = createdCampaign.Id,
                Role = RoleConstants.Leader,
                JoinedOn = DateTime.UtcNow,
            };

            await _unitOfWork.Member.CreateAsync(campaignOwner);

            var campaignWithLeader = await _unitOfWork.Campaign
                       .GetAsync(g => g.Id == campaign.Id, includeProperties: "Members,Members.User");

            var createCampaignResponseDTO = _mapper.Map<CreateCampaignResponseDto>(campaignWithLeader);

            return ServiceResult<CreateCampaignResponseDto>.Success(createCampaignResponseDTO);
        });
    }


    public async Task<ServiceResult<UpdateCampaignDetailsResponseDto>> UpdateCampaignDetails(UpdateCampaignDetailsRequestDto requestDto, string userId)
    {
        var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Update Campaign Request DTO");
        if (!campaignValidationResult.IsSuccess) return ServiceResult<UpdateCampaignDetailsResponseDto>.Failure(campaignValidationResult.ErrorMessage);

        var campaignIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, "Campaign Id");
        if (!campaignIdValidationResult.IsSuccess) return ServiceResult<UpdateCampaignDetailsResponseDto>.Failure(campaignIdValidationResult.ErrorMessage);

        if (!await IsUserCampaignLeader(requestDto.Id, userId))
        {
            return ServiceResult<UpdateCampaignDetailsResponseDto>.Failure("User is not authorized to update the campaign leader.");
        }

        return await HandleExceptions<UpdateCampaignDetailsResponseDto>(async () =>
        {
            var foundCampaign = await _unitOfWork.Campaign.GetAsync(g => g.Id == requestDto.Id && g.OwnerId == userId);

            if (foundCampaign == null)
            {
                return ServiceResult<UpdateCampaignDetailsResponseDto>.Failure("Campaign not found.");
            }



            foundCampaign.Name = requestDto.Name.Trim();
            foundCampaign.Description = requestDto.Description.Trim();
            foundCampaign.Color = requestDto.Color;
            foundCampaign.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Campaign.UpdateAsync(foundCampaign);

            var responseDto = _mapper.Map<UpdateCampaignDetailsResponseDto>(foundCampaign);

            return ServiceResult<UpdateCampaignDetailsResponseDto>.Success(responseDto);
        });
    }

    public async Task<ServiceResult<GetCampaignResponseDto>> UpdateCampaignLeader(int campaignId, string userId, UpdateCampaignOwnerRequestDto requestDto)
    {
        var validations = new[]
        {
                ValidationHelper.ValidateId(campaignId, "Campaign Id"),
                ValidationHelper.ValidateId(requestDto.UserId, "Userid")
            };

        var failedValidation = validations.FirstOrDefault(v => !v.IsSuccess);
        if (failedValidation != null)
            return ServiceResult<GetCampaignResponseDto>.Failure(failedValidation.ErrorMessage);

        if (campaignId != requestDto.CampaignId)
            return ServiceResult<GetCampaignResponseDto>.Failure("Campaign member must be from same campaign");

        if (!await IsUserCampaignLeader(campaignId, userId))
            return ServiceResult<GetCampaignResponseDto>.Failure("User is not authorized to update the campaign leader.");

        return await HandleExceptions<GetCampaignResponseDto>(async () =>
        {
            var foundCampaign = await _unitOfWork.Campaign.GetAsync(g => g.Id == campaignId);
            if (foundCampaign is null)
                return ServiceResult<GetCampaignResponseDto>.Failure("Campaign not found");


            foundCampaign.OwnerId = requestDto.UserId;
            await _unitOfWork.Campaign.UpdateAsync(foundCampaign);

            var responseDto = _mapper.Map<GetCampaignResponseDto>(foundCampaign);

            return ServiceResult<GetCampaignResponseDto>.Success(responseDto);
        });
    }



    public async Task<ServiceResult<int>> DeleteCampaign(int campaignId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, "Campaign Id");
        if (!campaignIdValidationResult.IsSuccess) return ServiceResult<int>.Failure(campaignIdValidationResult.ErrorMessage);

        return await HandleExceptions<int>(async () =>
        {
            var foundCampaign = await _unitOfWork.Campaign.GetAsync(g => g.Id == campaignId);

            if (foundCampaign == null)
            {
                return ServiceResult<int>.Failure("Campaign not found.");
            }

            // Delete the campaign
            await _unitOfWork.Campaign.RemoveAsync(foundCampaign);

            return ServiceResult<int>.Success(foundCampaign.Id);
        });
    }

    private async Task<bool> IsUserCampaignLeader(int campaignId, string userId)
    {
        var campaign = await _unitOfWork.Campaign.GetAsync(g => g.Id == campaignId);
        return campaign?.OwnerId == userId;
    }
}


