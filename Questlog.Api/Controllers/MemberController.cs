
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Member.Request;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/campaigns/{campaignId}/members")]
[Authorize]
[ApiController]
[ServiceFilter(typeof(TokenValidationFilter))]
public class MemberController : BaseController
{
    protected ApiResponse _response;
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _response = new ApiResponse();
        _memberService = memberService;
    }
    [HttpGet("{memberId}")]
    public async Task<ActionResult<ApiResponse>> GetMember(int campaignId, int memberId)
    {
        if (campaignId <= 0) return BadRequestResponse(" Id must be provided.");
        if (memberId <= 0) return BadRequestResponse(" Member Id must be provided.");

        var result = await _memberService.GetMember(campaignId, memberId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllMembers(int campaignId, [FromQuery] QueryParamsDto queryParams)
    {
        // Validate the  Id
        if (campaignId <= 0)
            return BadRequestResponse(" Id must be provided.");
    
        // Call the service to get all guild members with the specified query parameters
        var result = await _memberService.GetAllPaginatedMembers(campaignId, queryParams);
    
        // Check for success
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }
    
        // Return successful response
        return OkResponse(result.Data);
    }
    

    [HttpGet("paginated")]
    public async Task<ActionResult<ApiResponse>> GetMembersPaginated(int campaignId, [FromQuery] QueryParamsDto queryParams)
    {
        // Validate the  Id
        if (campaignId <= 0)
            return BadRequestResponse(" Id must be provided.");
    
        // Call the service to get all guild members with the specified query parameters
        var result = await _memberService.GetAllPaginatedMembers(campaignId, queryParams);
    
        // Check for success
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }
    
        // Return successful response
        return OkResponse(result.Data);
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateMember([FromBody] CreateMemberRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequestResponse("CreateMemberRequestDTO cannot be null.");
        }

        var result = await _memberService.CreateMember(requestDto);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return CreatedResponse(result.Data);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<ApiResponse>> UpdateMember(int campaignId, string userId, [FromBody] UpdateMemberRoleRequestDto roleRequestDto)
    {
        if (roleRequestDto == null)
        {
            return BadRequestResponse("UpdateMemberRequestDTO cannot be null.");
        }

        var result = await _memberService.UpdateMember(roleRequestDto);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpDelete("{memberId}")]
    public async Task<ActionResult<ApiResponse>> RemoveMember(int campaignId, int memberId)
    {
        if (campaignId <= 0) return BadRequestResponse(" Id must be provided.");
        if (memberId <= 0) return BadRequestResponse(" Member Id must be provided.");

        var result = await _memberService.RemoveMember(campaignId, memberId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

}
