using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IPartyService
{
    Task<ServiceResult<GetPartyResponseDto>> GetPartyById(int guildId, int partyId);
    Task<ServiceResult<List<GetPartyResponseDto>>> GetAllParties(int guildId, PartyQueryParamsDto queryParams);
    Task<ServiceResult<CreatePartyResponseDTO>> CreateParty(string userId, CreatePartyRequestDto requestDto, int guildId);
    Task<ServiceResult<GetPartyResponseDto>> UpdateParty(int guildId, UpdatePartyRequestDTO requestDTO);
    Task<ServiceResult<int>> DeleteParty(int guildId, int partyId);
}
