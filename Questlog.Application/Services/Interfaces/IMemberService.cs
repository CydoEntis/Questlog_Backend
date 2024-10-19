using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.GuildMember.Request;
using Questlog.Application.Common.DTOs.GuildMember.Response;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IMemberService
{
    Task<ServiceResult<GetMemberResponseDto>> GetMember(int campaignId, int memberId);
    // Task<ServiceResult<List<GetMemberResponseDto>>> GetAllMembers(int campaignId, MembersQueryParamsDto queryParams);
    Task<ServiceResult<CreateMemberResponseDto>> CreateMember(CreateMemberRequestDto requestDto);
    Task<ServiceResult<UpdateMemberRoleResponseDto>> UpdateMember(UpdateMemberRoleRequestDto roleRequestDto);
    Task<ServiceResult<int>> RemoveMember(int campaignId, int memberId);
}
