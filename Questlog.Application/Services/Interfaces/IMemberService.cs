using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Member.Request;
using Questlog.Application.Common.DTOs.Member.Response;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IMemberService
{
    Task<ServiceResult<GetMemberResponseDto>> GetMember(int campaignId, int memberId);

    Task<ServiceResult<List<GetMemberResponseDto>>> GetAllMembers(int campaignId);

    Task<ServiceResult<PaginatedResult<GetMemberResponseDto>>> GetAllPaginatedMembers(int campaignId,
        QueryParamsDto queryParams);

    Task<ServiceResult<CreateMemberResponseDto>> CreateMember(CreateMemberRequestDto requestDto);
    Task<ServiceResult<UpdateMemberRoleResponseDto>> UpdateMember(UpdateMemberRoleRequestDto roleRequestDto);
    Task<ServiceResult<int>> RemoveMember(int campaignId, int memberId);
}