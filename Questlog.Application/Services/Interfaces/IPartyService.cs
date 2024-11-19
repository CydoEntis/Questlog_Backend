using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IPartyService
{
    Task<ServiceResult<PartyDto>> GetPartyById(string userId, int partyId);

    Task<ServiceResult<PaginatedResult<PartyDto>>> GetAllParties(string userId,
        QueryParamsDto queryParams);

    Task<ServiceResult<PartyDto>> CreateParty(string userId,
        CreatePartyDto requestDto);

    Task<ServiceResult<PartyDto>> UpdateParty(
        UpdatePartyDto requestDto, string userId);

    Task<ServiceResult<int>> DeleteParty(int partyId);
}