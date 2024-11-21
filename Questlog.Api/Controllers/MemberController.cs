using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Member;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;

namespace Questlog.Api.Controllers;

[Route("api/parties/{partyId}/members")]
[Authorize]
[ApiController]
[ServiceFilter(typeof(TokenValidationFilter))]
public class MemberController : BaseController
{
    protected ApiResponse _response;
    private readonly IMemberService _memberService;
    private readonly IMapper _mapper;

    public MemberController(IMemberService memberService, IMapper mapper)
    {
        _response = new ApiResponse();
        _memberService = memberService;
        _mapper = mapper;
    }

    [HttpGet("{memberId}")]
    public async Task<ActionResult<ApiResponse>> GetMember(int partyId, int memberId)
    {
        if (partyId <= 0) return BadRequestResponse(" Id must be provided.");
        if (memberId <= 0) return BadRequestResponse(" Member Id must be provided.");

        var result = await _memberService.GetMember(partyId, memberId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllMembers(int partyId, [FromQuery] QueryParamsDto queryParams)
    {
        if (partyId <= 0)
            return BadRequestResponse(" Id must be provided.");

        var result = await _memberService.GetAllPaginatedMembers(partyId, queryParams);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    //
    // [HttpGet("paginated")]
    // public async Task<ActionResult<ApiResponse>> GetMembersPaginated(int partyId,
    //     [FromQuery] QueryParamsDto queryParams)
    // {
    //     // Validate the  Id
    //     if (partyId <= 0)
    //         return BadRequestResponse(" Id must be provided.");
    //
    //     // Call the service to get all guild members with the specified query parameters
    //     var result = await _memberService.GetAllPaginatedMembers(partyId, queryParams);
    //
    //     // Check for success
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequestResponse(result.ErrorMessage);
    //     }
    //
    //     // Return successful response
    //     return OkResponse(result.Data);
    // }


    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateMember([FromBody] CreateMemberDto requestDto)
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
    public async Task<ActionResult<ApiResponse>> UpdateMember(int partyId, string userId,
        [FromBody] UpdateMemberDto roleRequestDto)
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
    public async Task<ActionResult<ApiResponse>> RemoveMember(int partyId, int memberId)
    {
        if (partyId <= 0) return BadRequestResponse(" Id must be provided.");
        if (memberId <= 0) return BadRequestResponse(" Member Id must be provided.");

        var result = await _memberService.RemoveMember(partyId, memberId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet("invite")]
    public async Task<ActionResult<ApiResponse>> Invite(int partyId)
    {
        // Validate the token
        var result = await _memberService.GenerateInviteLink(partyId);


        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpPost("accept-invite")]
    public async Task<ActionResult<ApiResponse>> AcceptInvite([FromBody] AcceptInviteDto acceptInviteDto)
    {
        var userId = HttpContext.Items["UserId"] as string;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return BadRequestResponse("User Id must be provided.");
        }

        if (acceptInviteDto == null || string.IsNullOrWhiteSpace(acceptInviteDto.Token))
        {
            return BadRequestResponse("Token must be provided.");
        }


        var result = await _memberService.AcceptInvite(acceptInviteDto.Token, userId);

        if (result.IsSuccess)
        {
            return OkResponse(result.Data);
        }

        return BadRequestResponse(result.ErrorMessage);
    }

    [HttpPut("update-role")]
    public async Task<ActionResult<ApiResponse>>  UpdateMemberRoles([FromBody] NewMemberRoleDto requestDto)
    {
        var userId = HttpContext.Items["UserId"] as string;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return BadRequestResponse("User Id must be provided.");
        }

        var newMemberRoles = _mapper.Map<List<MemberRole>>(requestDto.MemberRoles);
        var result = await _memberService.UpdateMemberRoles(requestDto.PartyId, newMemberRoles, userId);

        if (result.IsSuccess)
        {
            return OkResponse(result.Data);
        }

        return BadRequestResponse(result.ErrorMessage);
    }

    [HttpPut("change-creator")]
    public async Task<ActionResult<ApiResponse>> ChangeCreator([FromBody] ChangeCreatorDto creatorDto)
    {
        var userId = HttpContext.Items["UserId"] as string;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return BadRequestResponse("User Id must be provided.");
        }

        var result = await _memberService.ChangeCreatorRole(creatorDto, userId);

        if (result.IsSuccess)
        {
            return OkResponse(result.Data);
        }

        return BadRequestResponse(result.ErrorMessage);
    }
    
    [HttpDelete]
    public async Task<ActionResult<ApiResponse>> RemoveMembers([FromBody] RemoveMembersDto dto)
    {

        var userId = HttpContext.Items["UserId"] as string;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return BadRequestResponse("User Id must be provided.");
        }

        var result = await _memberService.RemoveMembers(dto, userId);

        if (result.IsSuccess)
        {
            return OkResponse(result.Data);
        }

        return BadRequestResponse(result.ErrorMessage);
    }
}