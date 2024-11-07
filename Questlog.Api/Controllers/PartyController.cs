using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/parties")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class PartyController : BaseController
{
    private readonly IPartyService _partyService;

    public PartyController(IPartyService partyService)
    {
        _partyService = partyService;
    }

    [HttpGet("{partyId}")]
    public async Task<ActionResult<ApiResponse>> GetParty(int partyId)
    {
        if (partyId <= 0) return BadRequestResponse("Party Id must be provided.");

        var result = await _partyService.GetPartyById(partyId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllParties([FromQuery] QueryParamsDto queryParams)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }


        var result = await _partyService.GetAllParties(userId, queryParams);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateParty([FromBody] CreatePartyDto requestDto)
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

        var result = await _partyService.CreateParty(userId, requestDto);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return CreatedResponse(result.Data);

        //var createdCampaignId = result.Data.Id; // Assuming Id is a property of CampaignResponseDTO
        //var locationUri = Url.Action("GetCampaign", "Campaign", new { campaignId = createdCampaignId }, HttpContext.Request.Scheme);

        //return CreatedResponse(new { Id = createdCampaignId, Location = locationUri, Message = "Campaign created successfully" });
    }

    [HttpPut("{partyId}/details")]
    public async Task<ActionResult<ApiResponse>> UpdatePartyDetails(int partyId,
        [FromBody] UpdatePartyDto requestDto)
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

        var result = await _partyService.UpdateParty(requestDto, userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }


    [HttpDelete("{partyId}")]
    public async Task<ActionResult<ApiResponse>> DeleteParty(int partyId)
    {
        if (partyId <= 0) return BadRequestResponse("Party Id must be provided.");

        var result = await _partyService.DeleteParty(partyId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}