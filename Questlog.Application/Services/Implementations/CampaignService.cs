using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Campaign;
using Questlog.Application.Common.Extensions;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;


namespace Questlog.Application.Services.Implementations;

public class CampaignService : BaseService, ICampaignService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;


    public CampaignService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
        ILogger<CampaignService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ServiceResult<CampaignDto>> GetCampaignById(int campaignId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, "Campaign Id");
        if (!campaignIdValidationResult.IsSuccess)
            return ServiceResult<CampaignDto>.Failure(campaignIdValidationResult.ErrorMessage);

        return await HandleExceptions<CampaignDto>(async () =>
        {
            var foundCampaign =
                await _unitOfWork.Campaign.GetAsync(g => g.Id == campaignId, includeProperties: "Members,Members.User");

            if (foundCampaign == null)
            {
                return ServiceResult<CampaignDto>.Failure("Campaign not found.");
            }

            var campaignResponseDTO = _mapper.Map<CampaignDto>(foundCampaign);

            return ServiceResult<CampaignDto>.Success(campaignResponseDTO);
        });
    }

    public async Task<ServiceResult<PaginatedResult<CampaignDto>>> GetAllCampaigns(string userId,
        QueryParamsDto queryParams)
    {
        try
        {
            var options = new QueryOptions<Campaign>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                OrderBy = queryParams.OrderBy,
                OrderOn = queryParams.OrderOn,
                IncludeProperties = "Members,Members.User",
                Filter = c => c.Members.Any(m => m.UserId == userId)
            };

            if (!string.IsNullOrEmpty(queryParams.SearchValue))
            {
                options.Filter = options.Filter.And(c => c.Title.Contains(queryParams.SearchValue));
            }

            var paginatedResult = await _unitOfWork.Campaign.GetPaginatedCampaignsAsync(options);

            var campaignResponseDTOs = _mapper.Map<List<CampaignDto>>(paginatedResult.Items);

            var result = new PaginatedResult<CampaignDto>(campaignResponseDTOs, paginatedResult.TotalItems,
                paginatedResult.CurrentPage, queryParams.PageSize);

            return ServiceResult<PaginatedResult<CampaignDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ServiceResult<PaginatedResult<CampaignDto>>.Failure(ex.InnerException.ToString());
        }
    }


    public async Task<ServiceResult<CampaignDto>> CreateCampaign(string userId,
        CreateCampaignDto requestDto)
    {
        try
        {
            var userValidationResult = await ValidationHelper.ValidateUserIdAsync(userId, _userManager);
            if (!userValidationResult.IsSuccess)
                return ServiceResult<CampaignDto>.Failure(userValidationResult.ErrorMessage);

            var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Create Campaign Request DTO");
            if (!campaignValidationResult.IsSuccess)
                return ServiceResult<CampaignDto>.Failure(campaignValidationResult.ErrorMessage);


            var campaign = _mapper.Map<Campaign>(requestDto);
            campaign.OwnerId = userId;

            Campaign createdCampaign = await _unitOfWork.Campaign.CreateAsync(campaign);

            var campaignOwner = new Member
            {
                UserId = userId,
                CampaignId = createdCampaign.Id,
                Role = RoleConstants.Owner,
                JoinedOn = DateTime.UtcNow,
            };

            await _unitOfWork.Member.CreateAsync(campaignOwner);

            var campaignWithLeader = await _unitOfWork.Campaign
                .GetAsync(g => g.Id == campaign.Id, includeProperties: "Members,Members.User");

            var createCampaignResponseDTO = _mapper.Map<CampaignDto>(campaignWithLeader);

            return ServiceResult<CampaignDto>.Success(createCampaignResponseDTO);
        }
        catch (Exception ex)
        {
            return ServiceResult<CampaignDto>.Failure(
                ex.InnerException.ToString());
        }
    }


    public async Task<ServiceResult<CampaignDto>> UpdateCampaign(
        UpdateCampaignDto requestDto, string userId)
    {
        try
        {
            var campaignValidationResult = ValidationHelper.ValidateObject(requestDto, "Update Campaign Request DTO");
            if (!campaignValidationResult.IsSuccess)
                return ServiceResult<CampaignDto>.Failure(campaignValidationResult.ErrorMessage);

            var campaignIdValidationResult = ValidationHelper.ValidateId(requestDto.Id, "Campaign Id");
            if (!campaignIdValidationResult.IsSuccess)
                return ServiceResult<CampaignDto>.Failure(campaignIdValidationResult.ErrorMessage);

            if (!await IsUserCampaignLeader(requestDto.Id, userId))
            {
                return ServiceResult<CampaignDto>.Failure(
                    "User is not authorized to update the campaign leader.");
            }


            var foundCampaign = await _unitOfWork.Campaign.GetAsync(g => g.Id == requestDto.Id && g.OwnerId == userId);

            if (foundCampaign == null)
            {
                return ServiceResult<CampaignDto>.Failure("Campaign not found.");
            }


            foundCampaign.Title = requestDto.Title.Trim();
            foundCampaign.Description = requestDto.Description.Trim();
            foundCampaign.Color = requestDto.Color;
            foundCampaign.UpdatedAt = DateTime.UtcNow;
            if (requestDto.DueDate.HasValue)
            {
                foundCampaign.DueDate = requestDto.DueDate.Value;
            }

            await _unitOfWork.Campaign.UpdateAsync(foundCampaign);

            var responseDto = _mapper.Map<CampaignDto>(foundCampaign);

            return ServiceResult<CampaignDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<CampaignDto>.Failure(ex.InnerException.ToString());
        }
    }

    public async Task<ServiceResult<int>> DeleteCampaign(int campaignId)
    {
        var campaignIdValidationResult = ValidationHelper.ValidateId(campaignId, "Campaign Id");
        if (!campaignIdValidationResult.IsSuccess)
            return ServiceResult<int>.Failure(campaignIdValidationResult.ErrorMessage);

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