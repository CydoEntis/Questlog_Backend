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
        Task<ServiceResult<PartyResponseDTO>> GetPartyById(int guildId, int partyId);
        Task<ServiceResult<List<PartyResponseDTO>>> GetAllParties(int guildId);
        Task<ServiceResult<PartyResponseDTO>> CreateParty(string userId, CreatePartyRequestDTO requestDTO, int guildId);
        Task<ServiceResult<PartyResponseDTO>> UpdateParty(int guildId, UpdatePartyRequestDTO requestDTO);
        Task<ServiceResult<int>> DeleteParty(int guildId, int partyId);
    }
}
