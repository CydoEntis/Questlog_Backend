using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Campaign;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface ICampaignService
{
    Task<ServiceResult<CampaignDto>> GetCampaignById(int campaignId);

    Task<ServiceResult<PaginatedResult<CampaignDto>>> GetAllCampaigns(string userId,
        QueryParamsDto queryParams);

    Task<ServiceResult<CampaignDto>> CreateCampaign(string userId,
        CreateCampaignDto requestDto);

    Task<ServiceResult<CampaignDto>> UpdateCampaign(
        UpdateCampaignDto requestDto, string userId);

    Task<ServiceResult<int>> DeleteCampaign(int campaignId);
}