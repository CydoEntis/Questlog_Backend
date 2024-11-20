using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Member;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Services.Interfaces;

public interface IMemberService
{
    Task<ServiceResult<MemberDto>> GetMember(int campaignId, int guildMemberId);

    Task<ServiceResult<List<MemberDto>>> GetAllMembers(int campaignId);

    Task<ServiceResult<PaginatedResult<MemberDto>>> GetAllPaginatedMembers(int campaignId,
        QueryParamsDto queryParams);

    Task<ServiceResult<MemberDto>> CreateMember(CreateMemberDto requestDto);

    Task<ServiceResult<MemberDto>> UpdateMember(
        UpdateMemberDto roleRequestDto);

    Task<ServiceResult<int>> RemoveMember(int campaignId, int guildMemberId);

    Task<ServiceResult<string>> GenerateInviteLink(int campaignId);

    Task<ServiceResult<string>> AcceptInvite(string token, string userId);
    Task<ServiceResult<List<MemberDto>>> UpdateMemberRoles(int partyId,
        List<MemberRole> updatedMemberRoles, string currentUserId);
    Task<ServiceResult<string>> ChangeCreatorRole(int partyId, int newCreatorId, string currentUserId);
}