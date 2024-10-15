
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.GuildMember.Request;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/guilds/{guildId}/members")]
[Authorize]
[ApiController]
[ServiceFilter(typeof(TokenValidationFilter))]
public class GuildMemberController : BaseController
{
    protected ApiResponse _response;
    private readonly IGuildMemberService _guildMemberService;

    public GuildMemberController(IGuildMemberService guildMemberService)
    {
        _response = new ApiResponse();
        _guildMemberService = guildMemberService;
    }
    [HttpGet("{guildMemberId}")]
    public async Task<ActionResult<ApiResponse>> GetGuildMember(int guildId, int guildMemberId)
    {
        if (guildId <= 0) return BadRequestResponse("Guild Id must be provided.");
        if (guildMemberId <= 0) return BadRequestResponse("Guild Member Id must be provided.");

        var result = await _guildMemberService.GetGuildMember(guildId, guildMemberId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }


    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllGuildMembers(int guildId, [FromQuery] GuildMembersQueryParamsDTO queryParams)
    {
        // Validate the Guild Id
        if (guildId <= 0)
            return BadRequestResponse("Guild Id must be provided.");

        // Call the service to get all guild members with the specified query parameters
        var result = await _guildMemberService.GetAllGuildMembers(guildId, queryParams);

        // Check for success
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        // Return successful response
        return OkResponse(result.Data);
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateGuildMember([FromBody] CreateGuildMemberRequestDTO requestDTO)
    {
        if (requestDTO == null)
        {
            return BadRequestResponse("CreateGuildMemberRequestDTO cannot be null.");
        }

        var result = await _guildMemberService.CreateGuildMember(requestDTO);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return CreatedResponse(result.Data);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<ApiResponse>> UpdateGuildMember(int guildId, string userId, [FromBody] UpdateGuildMemberRequestDTO requestDTO)
    {
        if (requestDTO == null)
        {
            return BadRequestResponse("UpdateGuildMemberRequestDTO cannot be null.");
        }

        var result = await _guildMemberService.UpdateGuildMember(requestDTO);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpDelete("{guildMemberId}")]
    public async Task<ActionResult<ApiResponse>> RemoveGuildMember(int guildId, int guildMemberId)
    {
        if (guildId <= 0) return BadRequestResponse("Guild Id must be provided.");
        if (guildMemberId <= 0) return BadRequestResponse("Guild Member Id must be provided.");

        var result = await _guildMemberService.RemoveGuildMember(guildId, guildMemberId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

}
