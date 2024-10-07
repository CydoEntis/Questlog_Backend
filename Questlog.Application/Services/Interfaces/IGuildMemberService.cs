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
    public interface IGuildMemberService
    {
        Task<ServiceResult<GuildMemberResponseDTO>> GetGuildMember(int guildId, string userId);
        Task<ServiceResult<List<GuildMemberResponseDTO>>> GetAllGuildMembers(int guildId);
        Task<ServiceResult<GuildMemberResponseDTO>> CreateGuildMember(CreateGuildMemberRequestDTO requestDTO);
        Task<ServiceResult<GuildMemberResponseDTO>> UpdateGuildMember(UpdateGuildMemberRequestDTO requestDTO);
        Task<ServiceResult<int>> RemoveGuildMember(int guildId);
    }
}
