using Questlog.Application.Common.DTOs.PartyMember;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface IPartyMemberService
    {
        Task<ServiceResult<PartyMemberResponseDTO>> GetPartyMember(int partyMemberId);
        Task<ServiceResult<List<PartyMemberResponseDTO>>> GetAllPartyMembers(int partyId);
        Task<ServiceResult<PartyMemberResponseDTO>> CreatePartyMember(CreatePartyMemberRequestDTO requestDTO);
        Task<ServiceResult<PartyMemberResponseDTO>> UpdatePartyMember(UpdatePartyMemberRequestDTO requestDTO);
        Task<ServiceResult<int>> RemovePartyMember(int partyMemberId);
    }
}
