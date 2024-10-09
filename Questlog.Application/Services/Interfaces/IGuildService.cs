using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Services.Interfaces
{
    public interface IGuildService
    {
        Task<ServiceResult<GetGuildResponseDTO>> GetGuildById(int guildId);
        Task<ServiceResult<List<GetAllGuildsResponseDTO>>> GetAllGuilds();
        Task<ServiceResult<CreateGuildResponseDTO>> CreateGuild(string userId, CreateGuildRequestDTO requestDTO);
        Task<ServiceResult<UpdateGuildResponseDTO>> UpdateGuild(UpdateGuildRequestDTO requestDTO, string userId);
        Task<ServiceResult<int>> DeleteGuild(int guildId);
    }
}
