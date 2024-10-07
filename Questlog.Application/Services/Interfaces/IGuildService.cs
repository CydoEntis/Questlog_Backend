using Questlog.Application.Common.DTOs.Guild;
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
        Task<ServiceResult<GuildResponseDTO>> GetGuildById(int guildId);
        Task<ServiceResult<List<GuildResponseDTO>>> GetAllGuilds();
        Task<ServiceResult<GuildResponseDTO>> CreateGuild(string userId, CreateGuildRequestDTO requestDTO);
        Task<ServiceResult<GuildResponseDTO>> UpdateGuild(UpdateGuildRequestDTO requestDTO);
        Task<ServiceResult<int>> DeleteGuild(int guildId);
    }
}
