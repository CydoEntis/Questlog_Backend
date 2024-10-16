using Questlog.Application.Common.DTOs.PartyMember;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IPartyMemberService
{
    Task<ServiceResult<PartyMemberResponseDTO>> GetPartyMember(int partyMemberId);
    //Task<ServiceResult<List<PartyMemberResponseDTO>>> GetAllPartyMembers(int partyId);
    Task<ServiceResult<PartyMemberResponseDTO>> CreatePartyMember(CreatePartyMemberRequestDto requestDTO);
    Task<ServiceResult<PartyMemberResponseDTO>> UpdatePartyMember(UpdatePartyMemberRequestDTO requestDTO);
    Task<ServiceResult<int>> RemovePartyMember(int partyMemberId);
}
