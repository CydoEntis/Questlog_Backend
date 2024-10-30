using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Campaign.Requests;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/campaigns")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class CampaignController : BaseController
{
    protected ApiResponse _response;
    private readonly ICampaignService _campaignService;

    public CampaignController(ICampaignService campaignService)
    {
        _response = new ApiResponse();
        _campaignService = campaignService;
    }

    [HttpGet("{campaignId}")]
    public async Task<ActionResult<ApiResponse>> GetCampaign(int campaignId)
    {
        if (campaignId <= 0) return BadRequestResponse("Campaign Id must be provided.");

        var result = await _campaignService.GetCampaignById(campaignId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllCampaigns([FromQuery] QueryParamsDto queryParams)
    {
        string userId = HttpContext.Items["UserId"] as string;
    
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

         
        var result = await _campaignService.GetAllCampaigns(userId, queryParams);
    
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }
    
        return OkResponse(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateCampaign([FromBody] CreateCampaignRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequestResponse("CreateCampaignRequestDTO cannot be null.");
        }

        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _campaignService.CreateCampaign(userId, requestDto);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return CreatedResponse(result.Data);

        //var createdCampaignId = result.Data.Id; // Assuming Id is a property of CampaignResponseDTO
        //var locationUri = Url.Action("GetCampaign", "Campaign", new { campaignId = createdCampaignId }, HttpContext.Request.Scheme);

        //return CreatedResponse(new { Id = createdCampaignId, Location = locationUri, Message = "Campaign created successfully" });
    }

    [HttpPut("{campaignId}/details")]
    public async Task<ActionResult<ApiResponse>> UpdateCampaignDetails(int campaignId, [FromBody] UpdateCampaignRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequestResponse("UpdateCampaignRequestDTO cannot be null.");
        }

        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _campaignService.UpdateCampaignDetails(requestDto, userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpPut("{campaignId}/leader")]
    public async Task<ActionResult<ApiResponse>> UpdateCampaignLeader(int campaignId, [FromBody] UpdateCampaignOwnerRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequestResponse("UpdateCampaignRequestDTO cannot be null.");
        }

        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _campaignService.UpdateCampaignLeader(campaignId, userId, requestDto);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpDelete("{campaignId}")]
    public async Task<ActionResult<ApiResponse>> DeleteCampaign(int campaignId)
    {
        if (campaignId <= 0) return BadRequestResponse("Campaign Id must be provided.");

        var result = await _campaignService.DeleteCampaign(campaignId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}
