using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface IPartyService
    {
        Task<ServiceResult<PartyResponseDTO>> GetPartyById(int partyId);
        Task<ServiceResult<List<PartyResponseDTO>>> GetAllPartys();
        Task<ServiceResult<PartyResponseDTO>> CreateParty(string userId, CreatePartyRequestDTO requestDTO);
        Task<ServiceResult<PartyResponseDTO>> UpdateParty(UpdatePartyRequestDTO requestDTO);
        Task<ServiceResult<int>> DeleteParty(int partyId);
    }
}
