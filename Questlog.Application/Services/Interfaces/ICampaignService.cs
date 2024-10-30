using Questlog.Application.Common;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Campaign.Requests;
using Questlog.Application.Common.DTOs.Campaign.Responses;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface ICampaignService
{
    Task<ServiceResult<GetCampaignResponseDto>> GetCampaignById(int campaignId);

    // Task<ServiceResult<List<GetCampaignResponseDto>>> GetAllCampaigns(string userId,
    //     QueryParamsDto queryParams);

    Task<ServiceResult<PaginatedResult<GetCampaignResponseDto>>> GetAllCampaigns(string userId,
        QueryParamsDto queryParams);
    Task<ServiceResult<CreateCampaignResponseDto>> CreateCampaign(string userId, CreateCampaignRequestDto requestDto);
    Task<ServiceResult<UpdateCampaignResponseDto>> UpdateCampaignDetails(UpdateCampaignRequestDto requestDto, string userId);
    Task<ServiceResult<GetCampaignResponseDto>> UpdateCampaignLeader(int campaignId, string userId, UpdateCampaignOwnerRequestDto requestDto);
    Task<ServiceResult<int>> DeleteCampaign(int campaignId);
}
