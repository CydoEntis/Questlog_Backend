using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IGuildService
{
    Task<ServiceResult<GetGuildResponseDTO>> GetGuildById(int guildId);
    Task<ServiceResult<List<GetAllGuildsResponseDTO>>> GetAllGuilds(GuildQueryParamsDTO queryParams);
    Task<ServiceResult<CreateGuildResponseDTO>> CreateGuild(string userId, CreateGuildRequestDTO requestDTO);
    Task<ServiceResult<UpdateGuildDetailsResponseDTO>> UpdateGuildDetails(UpdateGuildDetailsRequestDTO requestDTO, string userId);
    Task<ServiceResult<GetGuildResponseDTO>> UpdateGuildLeader(int guildId, string userId, UpdateGuildLeaderRequestDTO requestDTO);
    Task<ServiceResult<int>> DeleteGuild(int guildId);
}
